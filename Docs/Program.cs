namespace OpusSharp.Docs
{
    public class Program
    {
        public static async Task<int> Main(string[] args) =>
          await Bootstrapper
            .Factory
            .CreateDocs(args)
            .AddSourceFiles("../../OpusSharp/OpusSharp.Core/**/{!.git,!bin,!obj,!packages,!*.Tests,}/**/*.cs")
            .DeployToGitHubPages(
              "avionblock",
              "avionblock.github.io",
              Config.FromSetting<string>("GITHUB_TOKEN")
            )
            .RunAsync();
    }
}