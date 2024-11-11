namespace OpusSharp.Docs
{
    public class Program
    {
        public static async Task<int> Main(string[] args) =>
          await Bootstrapper
            .Factory
            .CreateDocs(args)
            .AddSourceFiles("../OpusSharp.Core/**/{!.git,!bin,!obj,!packages,!*.Tests,}/**/*.cs")
            .AddSourceFiles("../../OpusSharp.Core/**/{!.git,!bin,!obj,!packages,!*.Tests,}/**/*.cs")
            .DeployToGitHubPages(
              "avionblock",
              "OpusSharp",
              Config.FromSetting<string>("GITHUB_TOKEN")
            )
            .RunAsync();
    }
}