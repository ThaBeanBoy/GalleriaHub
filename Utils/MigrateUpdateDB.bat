@echo off
cls


@REM moving to the server directory
cd %~dp0
cd ../server

@REM echo 
set /p MigrationName=Name you migration: 

dotnet dotnet-ef migrations add %MigrationName%
dotnet dotnet-ef database update