using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TeamSpeak3.Metrics.Test
{
    public class TelnetServer : IDisposable
    {
        private readonly Dictionary<string, string> _setup;
        private bool _isRunning = false;

        public static readonly int Port = 10011;
        public static readonly string Ip = "127.0.0.1";

        public TelnetServer()
        {
            _setup = new Dictionary<string, string>();

            new Thread(StartSocket).Start();
        }

        private void StartSocket()
        {
            Console.WriteLine("Socket start");
            Console.WriteLine();

            var localEndPoint = new IPEndPoint(IPAddress.Parse(Ip), Port);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(localEndPoint);
            socket.Listen(10);

            var handler = socket.Accept();

            _isRunning = true;
            while (_isRunning)
            {
                var messageBuffer = new byte[256];
                var messageLength = handler.Receive(messageBuffer);

                var message = Encoding.ASCII.GetString(messageBuffer, 0, messageLength).Trim();

                if (string.IsNullOrEmpty(message))
                {
                    Thread.Sleep(100);
                    continue;
                }

                Console.WriteLine($"Received {messageLength} chars: \"{message}\"");

                if (_setup.TryGetValue(message, out var value))
                {
                    Console.WriteLine($"Responded: {value}");
                    Console.WriteLine();

                    handler.Send(Encoding.ASCII.GetBytes(value));
                }
                else
                {
                    Console.WriteLine("ERROR: No response setup");
                }

            }

            handler.Shutdown(SocketShutdown.Both);
            handler.Dispose();

            Console.WriteLine("Socket shutdown");
        }

        public void Setup(string received, string response)
        {
            _setup.Add(received, response);
        }

        public void Dispose()
        {
            _isRunning = false;
        }

        public void ClearSetup()
        {
            _setup.Clear();
        }
    }
}