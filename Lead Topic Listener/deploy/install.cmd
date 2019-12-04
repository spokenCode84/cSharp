if not DEFINED IS_MINIMIZED set IS_MINIMIZED=1 && start "" /min "%~dpnx0" %* && exit
InstallUtil ..\bin\Release\IFB.CRM.Services.Lead.exe
exit