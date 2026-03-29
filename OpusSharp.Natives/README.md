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
- ios: ios/native/libopus.xcframework
- win-arm64: win-arm64/native/opus.dll
- win-x64: win-x64/native/opus.dll
- win-x86: win-x86/native/opus.dll

# Statically Linked Runtimes

> [!NOTE]
> iOS is included in the package as `libopus.xcframework` and linked automatically for .NET iOS projects. WASM still
> needs to be manually linked.

- wasm

> [!NOTE]
> - ARM32 is no longer supported by the windows OS and is no longer provided in this package.
> - Android builds are 16kb aligned.
> - Binaries have been updated to commit [788cc89](https://github.com/xiph/opus/commit/788cc89ce4f2c42025d8c70ec1b4457dc89cd50f)
