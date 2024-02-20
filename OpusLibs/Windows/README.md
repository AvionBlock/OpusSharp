# How to compile
## Open in Visual Studio.
Navigate to `opus/win32/VS2015/opus.sln` and open it, NOTE - IT DOES NOT MATTER WHAT VERSION OF VISUAL STUDIO YOU USE, JUST MAKE SURE YOU HAVE THE C++ MODULES INSTALLED INTO VISUAL STUDIO VIA THE INSTALLER.
## Compiling
Right click on `opus` project in the solution explorer and click on `build`.
## What you need
Depending on what architecture you built for, include the DLL file into the output of you're application

- `opus/win32/VS2015/x64/ReleaseDLL/opus.dll` *Assuming you selected `ReleaseDLL` on `x64` architecture.*
- `opus/win32/VS2015/Win32/ReleaseDLL/opus.dll` *Assuming you selected `ReleaseDLL` on `Win32` architecture.*

## Including into build
Copy the `DLL` file into you're project and set it's `Copy to Output Directory` value to `Copy if newer` *Visual Studio Users*.
