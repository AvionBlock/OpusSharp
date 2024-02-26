# OpusSharp
OpusSharp aims to be a cross platform C# compatible version of the native opus codec/library. The code uses the native compiled DLL's with instructions on how to compile you're own. Currently Windows and Android binaries are only available.

# Packaging as nuget
To use this library, you will need to package it so it can dynamically be loaded onto you're project without having to declare or make you're own library specific to a platform as that is handled by the nuget file.

First, Open the `.sln` file in an IDE of you're choice, then right click on the solution and build.

To package, Make sure to have nuget already installed either by an EXE in the same directory as this repository or install via PATH, Then open a command prompt/terminal in the repository's directory and type the following command:
`nuget pack OpusSharp.nusepc -Version 1.0.0 -OutputDirectory <YourLocalNugetFeedDirectory>`.

# Using the nuget package
Just install it onto you're directory, If you cannot find you're package in the nuget package manager of you're project, you will need to add you're local nuget feed to you're IDE: https://learn.microsoft.com/en-us/nuget/consume-packages/install-use-packages-visual-studio#package-sources
