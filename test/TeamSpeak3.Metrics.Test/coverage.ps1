if (Test-Path "TestResults")
{
    Remove-Item "TestResults" -Recurse
}

dotnet test --collect:"XPlat Code Coverage"
dotnet msbuild /t:GenerateCoverageReport

# Open report
TestResults\Report\index.htm