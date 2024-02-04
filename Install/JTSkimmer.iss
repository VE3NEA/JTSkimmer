; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "JT Skimmer"
#define MyAppVersion "0.94 beta"
#define MyAppPublisher "Alex VE3NEA"
#define MyAppURL "https://ve3nea.github.io/JTSkimmer"
#define MyAppExeName "JTSkimmer.exe"

#define public Dependency_Path_NetCoreCheck "..\Vendor\netcorecheck\"
#include "..\Vendor\netcorecheck\CodeDependencies.iss"

[Setup]
AppId={{48DE1FF3-0D54-4FF7-B292-CDAC1615ABB1}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\Afreet\{#MyAppName}
DisableProgramGroupPage=yes
OutputBaseFilename=JTSkimmerSetup
SetupIconFile=C:\Proj\DSP\JTSkimmer\JTSkimmer.ico
Compression=lzma
SolidCompression=true
ArchitecturesInstallIn64BitMode=x64
DisableStartupPrompt=true
ShowLanguageDialog=no

[Languages]
Name: english; MessagesFile: compiler:Default.isl

[Tasks]
Name: desktopicon; Description: {cm:CreateDesktopIcon}; GroupDescription: {cm:AdditionalIcons}; Flags: unchecked

[Files]
Source: ..\bin\x64\Release\JTSkimmer.exe; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\airspy.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\CSCore.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\fa-solid-900.ttf; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\JTSkimmer.deps.json; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\JTSkimmer.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\JTSkimmer.pdb; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\JTSkimmer.runtimeconfig.json; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\libfftw3f-3.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\libgcc_s_sjlj-1.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\libliquid.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\libusb-1.0.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\MathNet.Numerics.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\Newtonsoft.Json.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\pthreadVC2.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\rtlsdr.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\Serilog.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\Serilog.Sinks.File.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\SharpGL.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\SharpGL.SceneGraph.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\SharpGL.WinForms.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\WeifenLuo.WinFormsUI.Docking.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\WeifenLuo.WinFormsUI.Docking.ThemeVS2015.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\WsjtxUtils.WsjtxMessages.dll; DestDir: {app}; Flags: ignoreversion
Source: ..\bin\x64\Release\sdrplay_api.dll; DestDir: {app}; Flags: ignoreversion
Source: .\OmniRigSetup.exe; DestDir: {app}; Flags: deleteafterinstall overwritereadonly ignoreversion

[Icons]
Name: {commonprograms}\{#MyAppName}; Filename: {app}\{#MyAppExeName}
Name: {commondesktop}\{#MyAppName}; Filename: {app}\{#MyAppExeName}; Tasks: desktopicon

[Run]
Filename: {app}\{#MyAppExeName}; Description: {cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}; Flags: nowait postinstall skipifsilent
Filename: {app}\OmniRigSetup.exe; Parameters: /SILENT

[Code]
function InitializeSetup: Boolean;
begin
  Dependency_AddDotNet70Desktop;
  Result := True;
end;
