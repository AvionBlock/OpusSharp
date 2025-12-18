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

ARM32 is no longer supported by the windows OS.

Android builds are 16kb aligned.

iOS and WASM builds are not included as they need to be statically linked.

Binaries have been updated to commit [2785f8d](https://github.com/xiph/opus/commit/2785f8de02135bba3c1e6823a7d5b79ebd1b9473)
