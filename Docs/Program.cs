using System.Threading.Tasks;
using Statiq.App;
using Statiq.Docs;

namespace MySite
{
    public class Program
    {
        public static async Task<int> Main(string[] args) =>
          await Bootstrapper
            .Factory
            .CreateDocs(args)
            .AddSourceFiles("../OpusSharp.Core/**/{!.git,!bin,!obj,!packages,!*.Tests,}/**/*.cs")
            .AddSourceFiles("../../OpusSharp.Core/**/{!.git,!bin,!obj,!packages,!*.Tests,}/**/*.cs")
            .RunAsync();
    }
}