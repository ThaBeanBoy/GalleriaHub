@echo off
cls

@REM moving to root directory
cd %~dp0


@REM Restoring server dependancies & tools
cd ../server
dotnet restore
dotnet tool restore

@REM Restoring client dependancies & tools
cd ../client
npm install