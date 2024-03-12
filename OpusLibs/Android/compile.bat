@echo off
CLS
IF EXIST opus/ (
 GOTO BUILD
) ELSE (
 ECHO Opus folder not found.
 CHOICE /N /M "Do you want to clone the repository? Y,N:"

 IF ERRORLEVEL 2 ECHO Cancelling... & GOTO END
 IF ERRORLEVEL 1 GOTO CLONE
)

:BUILD
ECHO ===============BUILDING LIBRARY===============
ndk-build NDK_PROJECT_PATH=. APP_BUILD_SCRIPT=Android.mk
ECHO ==================BUILD END===================
ECHO.
ECHO.
ECHO To find the SO files if the build is successful, You can find them in obj/local/arm64-v8a/libopus.so, obj/local/armeabi-v7a/libopus.so, obj/local/x86/libopus.so, obj/local/x86_64/libopus.so
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