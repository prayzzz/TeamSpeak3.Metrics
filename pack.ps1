Write-Host "Executing dotnet pack"

$Version = "1.0.0" # git describe --tags
dotnet pack src/TeamSpeak3.Metrics/TeamSpeak3.Metrics.csproj --force -c Release -p:Version = $Version
dotnet pack src/TeamSpeak3.Metrics.AspNetCore/TeamSpeak3.Metrics.AspNetCore.csproj --force -c Release -p:Version = $Version

# For Prerelase packages:
# dotnet pack src/TeamSpeak3.Metrics/TeamSpeak3.Metrics.csproj --force -c Release -p:Version=3.0.0 -p:PackageVersion=3.0.0-alpha01 -p:FileVersion=3.0.0-alpha01 -p:InformationalVersion=3.0.0-alpha01