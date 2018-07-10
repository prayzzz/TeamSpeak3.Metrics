# Teamspeak3.Metrics

Exposes metrics from your Teamspeak3 Server.

## Runtime Requirements

* Teamspeak3 Server

## Output

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

## Development Requirements

* .NET Core 2.1 SDK

## Dev Setup

* set environment variables

```
TeamSpeak3Metrics_App__TeamSpeak__Ip=
TeamSpeak3Metrics_App__TeamSpeak__Port=
TeamSpeak3Metrics_App__TeamSpeak__QueryPort=
TeamSpeak3Metrics_App__TeamSpeak__QueryUsername=
TeamSpeak3Metrics_App__TeamSpeak__QueryPassword=
```