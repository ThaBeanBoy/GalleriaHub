@echo off
cls

@REM moving to root directory
cd %~dp0

echo Which would you like to run?
echo 1. Server
echo 2. Client

choice /c 12 /n /m "Choice: "

rem Capture the user's choice
set userChoice=%errorlevel%

if %userChoice% equ 1 (
    echo Running Server
    cd ./server
    call run    
) else if %userChoice% equ 2 (
    echo Running Client
    
    cd ./client
    call run
) else (
    echo Invalid choice
)