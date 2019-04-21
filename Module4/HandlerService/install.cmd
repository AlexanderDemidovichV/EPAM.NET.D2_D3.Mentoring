dotnet publish -c Debug -r win10-x64
.\bin\Debug\netcoreapp2.2\win10-x64\HandlerService.exe install -instance:FirstInstanceOfMyService
pause