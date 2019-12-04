if not DEFINED IS_MINIMIZED set IS_MINIMIZED=1 && start "" /min "%~dpnx0" %* && exit
InstallUtil /u ..\bin\Release\IFB.CRM.Services.Lead.exe
exit