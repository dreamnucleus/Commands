# Commands
msbuild /t:pack /p:Configuration=Release .\Slipstream.CommonDotNet.Commands\Slipstream.CommonDotNet.Commands.csproj

$latestCommands = Get-ChildItem -Path .\Slipstream.CommonDotNet.Commands\bin\Release\ | Sort-Object LastAccessTime -Descending | Select-Object -First 1

nuget add $latestCommands.FullName -source D:\NuGet

# Commands Autofac
msbuild /t:pack /p:Configuration=Release .\Slipstream.CommonDotNet.Commands.Autofac\Slipstream.CommonDotNet.Commands.Autofac.csproj

$latestCommandsAutofac = Get-ChildItem -Path .\Slipstream.CommonDotNet.Commands.Autofac\bin\Release\ | Sort-Object LastAccessTime -Descending | Select-Object -First 1

nuget add $latestCommandsAutofac.FullName -source D:\NuGet

# Commands Results
msbuild /t:pack /p:Configuration=Release .\Slipstream.CommonDotNet.Commands.Results\Slipstream.CommonDotNet.Commands.Results.csproj

$latestCommandsResults = Get-ChildItem -Path .\Slipstream.CommonDotNet.Commands.Results\bin\Release\ | Sort-Object LastAccessTime -Descending | Select-Object -First 1

nuget add $latestCommandsResults.FullName -source D:\NuGet