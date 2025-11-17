using Perosyan.Analyzer.Exceptions;


namespace Perosyan;




public static class ExceptionFormatter
{
    public static string Format(this GrammarException exception) => exception switch
    {
        OrthographicException e => e.Format(),

        _ => exception.ToString()
    };


    public static string Format(this OrthographicException exception)
    {
        var word = exception.Word;
        word = word.Insert(exception.Index + 1, "[/]");
        word = word.Insert(exception.Index, "[underline red]");

        return $"{exception.Message}, at word [green]\"{word}\"[/].";
    }
}
