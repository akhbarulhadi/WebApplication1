[Setup]
AppName=P1F Apps
AppVersion=1.0
DefaultDirName={pf}\P1F Apps
DefaultGroupName=P1F Apps
OutputDir=output
OutputBaseFilename=SetupP1FApps
Compression=lzma
SolidCompression=yes
CreateAppDir=yes

[Files]
Source: "publish\*"; DestDir: "{app}"; Flags: recursesubdirs

[Icons]
Name: "{group}\P1F Apps"; Filename: "{app}\WebApplication1.exe"
Name: "{group}\Buka Aplikasi"; Filename: "http://localhost:5000"
Name: "{userdesktop}\P1F Apps"; Filename: "{app}\WebApplication1.exe"; Tasks: desktopicon

[Tasks]
Name: "desktopicon"; Description: "Buat shortcut di desktop"; GroupDescription: "Shortcut:"

[Run]
Filename: "{app}\WebApplication1.exe"; Description: "Jalankan P1F Apps sekarang"; Flags: nowait postinstall skipifsilent
