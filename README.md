# Teamspeak3.Metrics

Exposes metrics from your Teamspeak3 Server.

## Runtime Requirements

* Teamspeak3 Server

## Usage

* Run
```
./teamspeak3.metrics
```
* Overwrite default appsettings
```
./teamspeak3.metrics my-settings.json
```

### Sample settings
```json
{
  "Kestrel": {
    "EndPoints": {
      "Http": {
        "Url": "http://localhost:1234"
      }
    }
  },
  "App": {
    "TeamSpeak": {    
      "Host": "127.0.0.1",
      "QueryUsername": "",
      "QueryPassword": ""
    }
  },
  "Serilog": {
    "WriteTo": [
      {
        "Name": "LiterateConsole"
      },
      {
        "Name": "RollingFile",
        "Args": {
          "pathFormat": "../logs/teamspeak3-metrics-{Date}.txt",
          "buffered": true
        }
      }
    ]
  }
}
```

### Output

```json
{
    "BytesReceived": 365148648,
    "BytesSent": 660032568,
    "Clients": [
    ],
    "ClientsOnline": 1,
    "CollectedAt": "2018-03-04T16:19:23.310556+01:00",
    "CollectionDuration": 223,
    "ServerId": 1,
    "ServerName": "russianbee",
    "Status": "online",
    "TotalPing": 0,
    "Uptime": "1879236"
}
```