@echo off  
net.exe session 1>NUL 2>NUL && (
    goto gotAdmin
) || (
    goto UACPrompt
)
   
:UACPrompt  
    echo Set UAC = CreateObject^("Shell.Application"^) > "%temp%\getadmin.vbs" 
    echo UAC.ShellExecute "%~s0", "", "", "runas", 1 >> "%temp%\getadmin.vbs" 
    "%temp%\getadmin.vbs" 
    exit /B  
   
:gotAdmin  
    if exist "%temp%\getadmin.vbs" ( del "%temp%\getadmin.vbs" )  
 goto begin
:begin
set name=NineBizlogistics
net stop %name%
sc delete %name%
Pause