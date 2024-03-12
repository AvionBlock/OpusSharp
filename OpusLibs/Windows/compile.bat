@echo off
CLS
ECHO 1: x64
ECHO 2: Win32

CHOICE /C 21 /N /M "Select Architecture:"

IF ERRORLEVEL 2 GOTO Compile64
IF ERRORLEVEL 1 GOTO CompileWin32
GOTO END

:Compile64
SET Arch=x64
GOTO CHECKOPUS

:CompileWin32
SET Arch=Win32
GOTO CHECKOPUS

:CHECKOPUS
IF EXIST opus/ (
 GOTO BUILD
) ELSE (
 ECHO Opus folder not found.
 CHOICE /N /M "Do you want to clone the repository? Y,N:"

 IF ERRORLEVEL 2 ECHO Cancelling... & GOTO END
 IF ERRORLEVEL 1 GOTO CLONE
)

:BUILD
IF EXIST build/ RMDIR /S /Q build
ECHO Set Architecture %Arch%
ECHO ===============BUILDING PROJECT===============
cmake ./opus -A %Arch% -D OPUS_BUILD_SHARED_LIBRARY=ON -B build
ECHO ==================BUILD END===================
ECHO.
ECHO.
ECHO ===============BUILDING LIBRARY===============
cmake --build ./build --config Release
ECHO ==================BUILD END===================
ECHO.
ECHO.
ECHO To find the DLL if the build is successful, You can find it in build/Release/opus.dll
PAUSE
GOTO END

:CLONE
ECHO =================CLONE START==================
git clone https://github.com/xiph/opus.git
ECHO ==================CLONE END===================
ECHO.
ECHO.
GOTO BUILD

:END