using Spectre.Console;

using Perosyan.Analyzer.Exceptions;


namespace Perosyan;




public static class Log
{
    public static void Error(GrammarException exception)
        => AnsiConsole.MarkupLine(exception.Format());
}
