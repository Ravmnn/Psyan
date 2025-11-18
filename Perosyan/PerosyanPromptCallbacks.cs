using PrettyPrompt;
using PrettyPrompt.Highlighting;

using Perosyan.Analyzer;


namespace Perosyan;




public class PerosyanPromptCallbacks : PromptCallbacks
{
    protected override Task<IReadOnlyCollection<FormatSpan>> HighlightCallbackAsync(string text, CancellationToken cancellationToken)
    {
        var formatSpans = new List<FormatSpan>();

        var tokens = new Lexer(text).Tokenize();
        var orthography = new OrthographicAnalyzer();

        // TODO: please cleanup this

        foreach (var token in tokens)
        {
            orthography.Word = token.Lexeme;

            var errorIndex = orthography.GetOrthographyErrorIndex();

            if (token.Type != TokenType.Word || errorIndex is not { } index)
                continue;

            var absoluteIndex = index + token.Location.Start;

            formatSpans.Add(new FormatSpan(token.Location.Start, index, AnsiColor.Red));
            formatSpans.Add(new FormatSpan(absoluteIndex, 1, new ConsoleFormat(AnsiColor.Red, Underline: true)));
            formatSpans.Add(new FormatSpan(absoluteIndex + 1, token.Lexeme.Length - index, AnsiColor.Red));
        }

        return Task.FromResult<IReadOnlyCollection<FormatSpan>>(formatSpans.AsReadOnly());
    }
}
