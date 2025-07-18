@ECHO OFF
ECHO Building KeyzOutpostFunerals 1.5 - Started

dotnet --list-sdks | findstr /R /C:"8\..*" 1>nul
if %errorlevel% neq 0 (
    ECHO .NET 8 SDK not found. Please install it using the following command:
    ECHO winget install Microsoft.DotNet.SDK.8
    PAUSE > NUL
    EXIT /B
)

@ECHO ON
dotnet restore 1.5/Source/KeyzOutpostFunerals.sln
dotnet build 1.5/Source/KeyzOutpostFunerals.sln /p:Configuration=Release
@ECHO OFF
ECHO Building KeyzOutpostFunerals 1.5 - Complete
ECHO Press any key to exit...
PAUSE > NUL
