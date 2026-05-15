using System.Runtime.InteropServices;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

    [Fact]
    public void MultistreamCtlNoArgumentImports_TargetShimSymbols()
    {
        AssertLibraryImportEntryPoint(
            typeof(NativeOpus),
            nameof(NativeOpus.opus_ms_encoder_ctl),
            "opussharp_ms_encoder_ctl");
        AssertLibraryImportEntryPoint(
            typeof(NativeOpus),
            nameof(NativeOpus.opus_ms_decoder_ctl),
            "opussharp_ms_decoder_ctl");
        AssertLibraryImportEntryPoint(
            typeof(StaticNativeOpus),
            nameof(StaticNativeOpus.opus_ms_encoder_ctl),
            "opussharp_ms_encoder_ctl");
        AssertLibraryImportEntryPoint(
            typeof(StaticNativeOpus),
            nameof(StaticNativeOpus.opus_ms_decoder_ctl),
            "opussharp_ms_decoder_ctl");
    }

    [Fact]
    public void CtlOverloadsWithVariadicArguments_AreManagedWrappers()
    {
        foreach (var method in GetPublicCtlMethods(typeof(NativeOpus), typeof(StaticNativeOpus))
                     .Where(method => method.GetParameters().Length > 2))
        {
            Assert.Empty(method.GetCustomAttributes<LibraryImportAttribute>());
            Assert.Empty(method.GetCustomAttributes<DllImportAttribute>());
        }
    }

    [Fact]
    public void ManagedCtlShimImports_AreDeclaredInNativeShimSource()
    {
        var shimSource = File.ReadAllText(Path.Combine(
            TestNativeLibraryBootstrapper.GetRepositoryRoot(),
            "OpusSharp.Natives",
            "opus_shim.c"));

        foreach (var symbol in GetManagedCtlShimSymbols())
        {
            Assert.Contains($"SHIM_EXPORT int {symbol}(", shimSource);
        }
    }

    [Fact]
    public void NativeRuntimeLibrary_ExportsManagedCtlShimSymbols()
    {
        var libraryPath = TestNativeLibraryBootstrapper.GetOutputNativeLibraryPath();
        var handle = NativeLibrary.Load(libraryPath);

        try
        {
            foreach (var symbol in GetManagedCtlShimSymbols())
            {
                Assert.True(
                    NativeLibrary.TryGetExport(handle, symbol, out _),
                    $"Expected '{Path.GetFileName(libraryPath)}' to export '{symbol}'.");
            }
        }
        finally
        {
            NativeLibrary.Free(handle);
        }
    }

    [Fact]
    public void IosNativeArchives_ContainManagedCtlShimSymbols()
    {
        var iosNativeRoot = Path.Combine(
            TestNativeLibraryBootstrapper.GetRepositoryRoot(),
            "OpusSharp.Natives",
            "runtimes",
            "ios",
            "native",
            "libopus.xcframework");
        var archivePaths = Directory.GetFiles(iosNativeRoot, "libopus.a", SearchOption.AllDirectories);

        Assert.NotEmpty(archivePaths);

        foreach (var archivePath in archivePaths)
        {
            foreach (var symbol in GetManagedCtlShimSymbols())
            {
                AssertFileContainsAsciiSymbol(archivePath, symbol);
            }
        }
    }

    [Fact]
    public void SupportedNativeRuntimeLibraries_ContainManagedCtlShimSymbols()
    {
        var nativeFiles = GetSupportedNativeRuntimeFiles().ToArray();

        Assert.NotEmpty(nativeFiles);

        foreach (var nativeFile in nativeFiles)
        {
            foreach (var symbol in GetManagedCtlShimSymbols())
            {
                AssertFileContainsAsciiSymbol(nativeFile, symbol);
            }
        }
    }

    [Fact]
    public void PackageSmoke_UsesCurrentPackageVersion()
    {
        var props = File.ReadAllText(Path.Combine(
            TestNativeLibraryBootstrapper.GetRepositoryRoot(),
            "samples",
            "PackageSmoke",
            "Directory.Build.props"));

        Assert.Contains(@"<Import Project=""..\..\Directory.Build.props"" />", props);
        Assert.Contains("<OpusSharpSmokeVersion>$(OpusSharpVersion)</OpusSharpSmokeVersion>", props);
    }

    [Fact]
    public void NativesPackage_ExcludesUnsupportedWindowsRuntimes()
    {
        var project = File.ReadAllText(Path.Combine(
            TestNativeLibraryBootstrapper.GetRepositoryRoot(),
            "OpusSharp.Natives",
            "OpusSharp.Natives.csproj"));

        Assert.Contains("runtimes\\win-arm\\**\\*", project);
        Assert.Contains("runtimes\\win-x86\\**\\*", project);
    }

    [Fact]
    public void WindowsNativeBuildWorkflow_HandlesDecoratedX86OpusSymbols()
    {
        var workflow = File.ReadAllText(Path.Combine(
            TestNativeLibraryBootstrapper.GetRepositoryRoot(),
            ".github",
            "workflows",
            "OpusCompile.yml"));

        Assert.Contains("_?opus_\\S+", workflow);
        Assert.Contains("$raw.Substring(1) + '=' + $raw", workflow);
    }

    private static void AssertLibraryImportEntryPoint(Type type, string methodName, string expectedEntryPoint)
    {
        var method = Assert.Single(
            type.GetMethods(BindingFlags.Public | BindingFlags.Static),
            method => method.Name == methodName && method.GetParameters().Length == 2);
        var importAttribute = Assert.Single(method.GetCustomAttributes<LibraryImportAttribute>());

        Assert.Equal(expectedEntryPoint, importAttribute.EntryPoint);
    }

    private static IEnumerable<MethodInfo> GetPublicCtlMethods(params Type[] types)
    {
        return types.SelectMany(type => type.GetMethods(BindingFlags.Public | BindingFlags.Static))
            .Where(method =>
                method.Name.StartsWith("opus_", StringComparison.Ordinal) &&
                method.Name.EndsWith("_ctl", StringComparison.Ordinal));
    }

    private static IEnumerable<string> GetManagedCtlShimSymbols()
    {
        return GetCtlImportMethods(typeof(NativeOpus), typeof(StaticNativeOpus))
            .Select(GetImportedSymbol)
            .Where(symbol => symbol.StartsWith("opussharp_", StringComparison.Ordinal))
            .Distinct()
            .Order(StringComparer.Ordinal);
    }

    private static IEnumerable<MethodInfo> GetCtlImportMethods(params Type[] types)
    {
        return types.SelectMany(type => type.GetMethods(
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static))
            .Where(method =>
                method.Name.Contains("_ctl", StringComparison.Ordinal) &&
                method.GetCustomAttributes<LibraryImportAttribute>().Any());
    }

    private static string GetImportedSymbol(MethodInfo method)
    {
        var importAttribute = Assert.Single(method.GetCustomAttributes<LibraryImportAttribute>());

        return string.IsNullOrEmpty(importAttribute.EntryPoint)
            ? method.Name
            : importAttribute.EntryPoint;
    }

    private static IEnumerable<string> GetSupportedNativeRuntimeFiles()
    {
        var nativesRoot = Path.Combine(
            TestNativeLibraryBootstrapper.GetRepositoryRoot(),
            "OpusSharp.Natives");
        var runtimesRoot = Path.Combine(nativesRoot, "runtimes");
        var readmePath = Path.Combine(nativesRoot, "README.md");
        var runtimePathPattern = new Regex(@"^- [^:]+: (?<path>\S+)$", RegexOptions.CultureInvariant);

        foreach (var line in File.ReadLines(readmePath))
        {
            var match = runtimePathPattern.Match(line);

            if (!match.Success)
            {
                continue;
            }

            var relativePath = match.Groups["path"].Value.Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(runtimesRoot, relativePath);

            if (File.Exists(fullPath))
            {
                if (IsNativeLibraryFile(fullPath))
                {
                    yield return fullPath;
                }

                continue;
            }

            Assert.True(Directory.Exists(fullPath), $"Expected supported native runtime path '{fullPath}' to exist.");

            foreach (var nativeFile in Directory.GetFiles(fullPath, "*", SearchOption.AllDirectories)
                         .Where(IsNativeLibraryFile))
            {
                yield return nativeFile;
            }
        }
    }

    private static bool IsNativeLibraryFile(string filePath)
    {
        return Path.GetExtension(filePath) switch
        {
            ".a" or ".dll" or ".dylib" or ".so" => true,
            _ => false
        };
    }

    private static void AssertFileContainsAsciiSymbol(string filePath, string symbol)
    {
        var fileBytes = File.ReadAllBytes(filePath);
        var symbolBytes = Encoding.ASCII.GetBytes(symbol);

        Assert.True(
            IndexOf(fileBytes, symbolBytes) >= 0,
            $"Expected '{filePath}' to contain symbol '{symbol}'.");
    }

    private static int IndexOf(byte[] source, byte[] value)
    {
        for (var i = 0; i <= source.Length - value.Length; i++)
        {
            var found = true;

            for (var j = 0; j < value.Length; j++)
            {
                if (source[i + j] == value[j])
                {
                    continue;
                }

                found = false;
                break;
            }

            if (found)
            {
                return i;
            }
        }

        return -1;
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

        public static string GetRepositoryRoot()
        {
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../"));
        }

        public static string GetOutputNativeLibraryPath()
        {
            EnsureInitialized();

            return Path.Combine(AppContext.BaseDirectory, Path.GetFileName(GetNativeLibraryPath()));
        }

        private static string GetNativeLibraryPath()
        {
            var runtimeFolder = GetRuntimeFolder();
            var fileName = GetNativeLibraryFileName();
            var path = Path.Combine(
                GetRepositoryRoot(),
                "OpusSharp.Natives",
                "runtimes",
                runtimeFolder,
                "native",
                fileName);

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
