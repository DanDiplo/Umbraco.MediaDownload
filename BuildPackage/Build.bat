rem Runs the build process that creates the NuGet and Umbraco packages
rem to debug add /bl to the MSBuild.exe task and use http://msbuildlog.com/ to view the binary log

rem Update this to copy the folder from your Umbraco solution that contains the DiploAuditLogViewer folder within App_Plugins.
rem This copies the DiploAuditLogViewer folder from your Umbraco solution into a folder called Umbraco that is then used to create the packages

xcopy ..\Umbraco.TestSite\App_Plugins\DiploMedia  .\Umbraco\App_Plugins\DiploMedia /e /i /h /s /y

rem Update this path to MSBuild if different on your system. Use a recent version that recognises latest C# syntax.

Call "%programfiles%\Microsoft Visual Studio\2022\Community\MSBuild\Current\Bin\MSBuild.exe" package.build.xml /bl /p:Configuration=Release

@RD /S /Q ..\BuildPackage\Umbraco\