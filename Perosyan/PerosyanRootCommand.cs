using System.CommandLine;


namespace Perosyan;




public class PerosyanRootCommand : RootCommand
{
    public ParseResult Result { get; }




    public Option<string> Source { get; }
    public Option<string> SourceFile { get; }

    public Option<bool> Interactive { get; }




    public PerosyanRootCommand(string[] args) : base("Syntactic analyzer for the Perocine language.")
    {
        Add(Source = new Option<string>("--source", "-s"));
        Add(SourceFile = new Option<string>("--source-file", "-f"));
        Add(Interactive = new Option<bool>("--interactive", "-i"));


        SetAction(async _ =>
        {
            await Program.Run(this, GetOptions());
        });

        Result = Parse(args);
    }




    public PerosyanOptions GetOptions()
        => new PerosyanOptions
        {
            Source = Result.GetValue(Source),
            SourceFile = Result.GetValue(SourceFile),

            Interactive = Result.GetValue(Interactive)
        };
}
