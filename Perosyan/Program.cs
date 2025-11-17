using System.CommandLine.Help;

using Spectre.Console;

using Perosyan.Analyzer;
using Perosyan.Analyzer.Exceptions;


namespace Perosyan;




class Program
{
    public static void Main(string[] args)
    {
        _ = new PerosyanRootCommand(args);
    }




    public static void Run(PerosyanRootCommand root, PerosyanOptions options)
    {
        if (options.Interactive)
        {
            RunInteractive(options);
            return;
        }

        if (options.Source is null && options.SourceFile is null)
        {
            new HelpAction().Invoke(root.Result);
            return;
        }

        RunPassive(options);
    }




    private static void RunPassive(PerosyanOptions options)
    {
        var source = options.Source ?? File.ReadAllText(options.SourceFile!);
        var tokens = new Lexer(source).Tokenize();

        var wordsSyllables = new List<Syllable[]>();

        try
        {
            foreach (var token in tokens)
                wordsSyllables.Add(new SyllableSeparator(token.Lexeme).Split());

            foreach (var wordSyllable in wordsSyllables)
            {
                for (var i = 0; i < wordSyllable.Length; i++)
                {
                    var syllable = wordSyllable[i];
                    AnsiConsole.Write($"{syllable.Substring}{(i + 1 < wordSyllable.Length ? "-" : "")}");
                }

                AnsiConsole.WriteLine();
            }
        }
        catch (OrthographicException exception)
        {
            Log.Error(exception);
        }
    }




    public static void RunInteractive(PerosyanOptions options)
    {

    }
}
