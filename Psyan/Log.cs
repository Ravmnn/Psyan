using Spectre.Console;

using Psyan.Analyzer.Exceptions;


namespace Psyan;




public static class Log
{
    public static void Error(GrammarException exception)
        => AnsiConsole.MarkupLine(exception.Format());
}
