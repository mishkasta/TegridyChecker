# build using configuration

dotnet build -c Debug;

# publish self-contained executable

dotnet publish -r win-x64 -o "./publish/self-contained-executable";

# publish runtime-dependent executable

dotnet publish -r win-x64 --self-contained=false -o "./publish/runtime-dependent-executable";

# publish cross-platform library

dotnet publish

# publish single project

dotnet publish .\TegridyChecker.Foundation\TegridyChecker.Foundation.csproj -r win-x64 -o "./publish/TegridyChecker.Foundation";

# run with command args

dotnet .\TegridyChecker.dll Logging:Console:LogLevel:Default="Debug"

# run with specified environment

dotnet .\TegridyChecker.dll ENVIRONMENT=Development
