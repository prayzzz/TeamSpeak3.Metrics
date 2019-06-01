using System.IO;
using System.Reflection;
using Moq;

namespace TeamSpeak3.Metrics.Test
{
    // ReSharper disable once InconsistentNaming
    public static class TH
    {
        public static Mock<T> CreateMock<T>() where T : class
        {
            return new Mock<T>(MockBehavior.Strict);
        }

        public static string ReadEmbeddedFile(Assembly assembly, string filePath)
        {
            var name = assembly.GetName().Name;
            using (var stream = assembly.GetManifestResourceStream(name + "." + filePath))
            using (var str = new StreamReader(stream))
            {
                return str.ReadToEnd();
            }
        }
    }
}
