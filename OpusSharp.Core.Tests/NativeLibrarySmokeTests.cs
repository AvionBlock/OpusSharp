using System.Runtime.InteropServices;
using OpusSharp.Core;
using Xunit;

namespace OpusSharp.Core.Tests;

public sealed class NativeLibrarySmokeTests
{
    public NativeLibrarySmokeTests()
    {
        TestNativeLibraryBootstrapper.EnsureInitialized();
    }

    [Fact]
    public void Version_ReturnsNonEmptyString()
    {
        var version = OpusInfo.Version();

        Assert.False(string.IsNullOrWhiteSpace(version));
    }

    [Fact]
    public void StringError_ReturnsKnownMessage()
    {
        var error = OpusInfo.StringError((int)OpusErrorCodes.OPUS_INVALID_PACKET);

        Assert.False(string.IsNullOrWhiteSpace(error));
    }

    [Fact]
    public void OpusException_PreservesMessageAndInnerException()
    {
        var inner = new InvalidOperationException("inner");
        var exception = new OpusException("outer", inner);

        Assert.Equal("outer", exception.Message);
        Assert.Same(inner, exception.InnerException);
    }

    [Fact]
    public void PublicEnumValues_MatchExpectedOpusConstants()
    {
        Assert.Equal(-4, (int)OpusErrorCodes.OPUS_INVALID_PACKET);
        Assert.Equal(2048, (int)OpusPredefinedValues.OPUS_APPLICATION_VOIP);
        Assert.Equal(4002, (int)EncoderCTL.OPUS_SET_BITRATE);
    }

    [Fact]
    public void DefaultOpusExceptionCtor_ProducesExceptionInstance()
    {
        var exception = new OpusException();

        Assert.NotNull(exception);
    }

    private static class TestNativeLibraryBootstrapper
    {
        private static bool _initialized;

        public static void EnsureInitialized()
        {
            if (_initialized)
            {
                return;
            }

            var sourcePath = GetNativeLibraryPath();
            var destinationPath = Path.Combine(AppContext.BaseDirectory, Path.GetFileName(sourcePath));

            if (!File.Exists(destinationPath))
            {
                File.Copy(sourcePath, destinationPath, overwrite: true);
            }

            _initialized = true;
        }

        private static string GetNativeLibraryPath()
        {
            var repoRoot = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));
            var runtimeFolder = GetRuntimeFolder();
            var fileName = GetNativeLibraryFileName();
            var path = Path.Combine(repoRoot, "OpusSharp.Natives", "runtimes", runtimeFolder, "native", fileName);

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Native opus library was not found at '{path}'.");
            }

            return path;
        }

        private static string GetRuntimeFolder()
        {
            if (OperatingSystem.IsMacOS())
            {
                return RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.Arm64 => "osx-arm64",
                    Architecture.X64 => "osx-x64",
                    _ => throw new PlatformNotSupportedException("Unsupported macOS architecture.")
                };
            }

            if (OperatingSystem.IsLinux())
            {
                return RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.Arm => "linux-arm",
                    Architecture.Arm64 => "linux-arm64",
                    Architecture.X64 => "linux-x64",
                    Architecture.X86 => "linux-x86",
                    _ => throw new PlatformNotSupportedException("Unsupported Linux architecture.")
                };
            }

            if (OperatingSystem.IsWindows())
            {
                return RuntimeInformation.ProcessArchitecture switch
                {
                    Architecture.Arm64 => "win-arm64",
                    Architecture.X64 => "win-x64",
                    Architecture.X86 => "win-x86",
                    _ => throw new PlatformNotSupportedException("Unsupported Windows architecture.")
                };
            }

            throw new PlatformNotSupportedException("Unsupported host operating system.");
        }

        private static string GetNativeLibraryFileName()
        {
            if (OperatingSystem.IsWindows())
            {
                return "opus.dll";
            }

            if (OperatingSystem.IsLinux())
            {
                return "opus.so";
            }

            if (OperatingSystem.IsMacOS())
            {
                return "opus.dylib";
            }

            throw new PlatformNotSupportedException("Unsupported host operating system.");
        }
    }
}
