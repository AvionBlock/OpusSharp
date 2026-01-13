# Supported Runtimes
- android-arm: android-arm/native/libopus.so
- android-arm64: android-arm64/native/libopus.so
- android-x64: android-x64/native/libopus.so
- android-x86: android-x86/native/libopus.so
- linux-arm: linux-arm/native/opus.so
- linux-arm64: linux-arm64/native/opus.so
- linux-x64: linux-x64/native/opus.so
- linux-x86: linux-x86/native/opus.so
- osx-arm64: osx-arm64/native/opus.dylib
- osx-x64: osx-x64/native/opus.dylib
- win-arm64: win-arm64/native/opus.dll
- win-x64: win-x64/native/opus.dll
- win-x86: win-x86/native/opus.dll

# Statically Linked Runtimes

> [!NOTE]
> These are not included in the library, these can be downloaded separately in the provided link below!
> 
> https://github.com/AvionBlock/OpusSharp/actions/runs/20946608512

- ios-device-arm64
- ios-simulator-arm64
- ios-simulator-x86_64
- ios-universal
- wasm

> [!NOTE]
> - ARM32 is no longer supported by the windows OS and is no longer provided in this package.
> - Android builds are 16kb aligned.
> - Binaries have been updated to commit [c5a745b](https://github.com/xiph/opus/commit/c5a745b665831704e54ffdfb8f018573af11290f)
