namespace Psyan.Analyzer.Exceptions;




public class OrthographicException : GrammarException
{
    public string Word { get; }
    public int Index { get; }




    public OrthographicException(string word, int index)
    {
        Word = word;
        Index = index;
    }

    public OrthographicException(string word, int index, string message) : base(message)
    {
        Word = word;
        Index = index;
    }

    public OrthographicException(string word, int index, string message, Exception inner) : base(message, inner)
    {
        Word = word;
        Index = index;
    }
}
