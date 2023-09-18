@echo off
cls

@echo off
cls

@REM moving to root directory
cd %~dp0

echo Which would you like to run?
echo 1. Server
echo 2. Client

choice /c 12 /n /m "Choice: "

set /p packageName="Package Name: "

rem Capture the user's choice
set userChoice=%errorlevel%

if %userChoice% equ 1 (
    cd ../server

    dotnet add package %packageName%
) else if %userChoice% equ 2 (
    cd ../client

    npm install %packageName%
) else (
    echo Invalid choice
)