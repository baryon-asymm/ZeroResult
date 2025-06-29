@echo off
SET scriptdir=%~dp0
dotnet run --project "%scriptdir%src\CodeGen\CodeGen.csproj"
pause
