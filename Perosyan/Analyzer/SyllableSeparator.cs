using Perosyan.Analyzer.Exceptions;


namespace Perosyan.Analyzer;




public readonly record struct Syllable(string Substring, int Start)
{
    public int End => Start + Substring.Length;
}




public class SyllableSeparator(string sourceWord)
{
    private List<Syllable> _syllables = [];


    public string Word { get; set; } = sourceWord;




    public Syllable[] Split()
    {
        var word = Word.Trim();

        if (word.Length == 0)
            return [];

        _syllables = [];

        // the first letter may be an alone vowel: a-la-fa-ve-te
        //                                         ^
        TryToAddSingleSyllable(word);

        var startIndex = _syllables.Count > 0 ? 1 : 0;
        for (var i = startIndex; i < word.Length; i++)
        {
            // single-syllables are: a, e, i, o, u
            // bi-syllables are: ka, ke, sa, se, pi...
            // tri-syllables are: kas, kes, sas, ses, pis, ton, pin...

            if (AddedTriSyllableDelimiterToLast(word, i) || AddedBiSyllable(word, ref i))
                continue;

            throw new OrthographicException(word, i, "Invalid syllable construction");
        }

        return _syllables.ToArray();
    }


    private bool AddedTriSyllableDelimiterToLast(string word, int i)
    {
        if (IsCurrentCharacterATriSyllableDelimiter(word, i))
        {
            var lastSyllable = _syllables[^1];
            _syllables[^1] = lastSyllable with { Substring = lastSyllable.Substring + word[i] };

            return true;
        }

        return false;
    }


    private bool AddedBiSyllable(string word, ref int i)
    {
        if (IsNextSequenceASyllable(word, i))
        {
            var start = i;
            var end = i + 2;

            _syllables.Add(new Syllable(word[start .. end], i));
            i++;

            return true;
        }

        return false;
    }


    private void TryToAddSingleSyllable(string word)
    {
        if (word.Length > 0 && Alphabet.Vowels.Contains(word[0]))
            _syllables.Add(new Syllable(word[0].ToString(), 0));
    }


    private bool IsCurrentCharacterATriSyllableDelimiter(string word, int i)
        => i >= 2 && word[i] == 's' && _syllables.Last().Substring.Last() != 's';


    private bool IsNextSequenceASyllable(string word, int i)
        => i + 1 < word.Length && Alphabet.Consonants.Contains(word[i]) && Alphabet.Vowels.Contains(word[i + 1]);
}
