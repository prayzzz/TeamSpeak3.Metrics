# TeamSpeak3.Metrics

![Nuget](https://img.shields.io/nuget/v/TeamSpeak3.Metrics.svg?style=flat-square)

Library to collect metrics from your TeamSpeak3 servers written in C# using .NET Standard 2.0

## Installation

Install it using [NuGet](https://www.nuget.org/packages/TeamSpeak3.Metrics/):
```
Install-Package TeamSpeak3.Metrics.AspNetCore
```

<small>You can also use package `TeamSpeak3.Metrics`, for other application types.</small>

## Usage

Add to `appsettings.json`
```json
{  
  "App": {
    "TS3Server": {
      "Host": "127.0.0.1",
      "Port": 9987,
      "QueryPort": 10011,
      "QueryUsername": "",
      "QueryPassword": ""
    }
  }  
}
```

In `Startup.cs`
```csharp
public void ConfigureServices(IServiceCollection services)
{
    // ...

    services.Configure<ServerOptions>(_configuration.GetSection("App:TS3Server"));

    services.AddTeamSpeak3Metrics()
            .AsHostedService();

    // ...
}        
```

Make sure the used Query account is in `AdminServerQuery` group.
See https://forum.teamspeak.com/threads/138692-Admin-Server-Query-Error-id-512-msg-invalid-sclientID?p=465167#post465167 how to achieve this.
  
## Contributing

1. Fork it!
2. Create your feature branch: `git checkout -b my-new-feature`
3. Commit your changes: `git commit -am 'Add some feature'`
4. Push to the branch: `git push origin my-new-feature`
5. Submit a pull request

## License

[MIT License](https://github.com/prayzzz/teamspeak3-metrics/blob/master/LICENSE.txt)