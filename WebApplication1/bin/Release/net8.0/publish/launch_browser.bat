@echo off
start "" WebApplication1.exe
timeout /t 3 >nul
start "" http://localhost:5000
