using Psyan.Analyzer.Exceptions;


namespace Psyan.Analyzer;




public readonly record struct Syllable(string Substring, int Start)
{
    public int End => Start + Substring.Length;
}




public class OrthographicAnalyzer(string sourceWord = "")
{
    private List<Syllable> _syllables = [];


    public string Word { get; set; } = sourceWord;



    public Syllable[] Split()
        => TrySplit(out var errorIndex) ?? throw new OrthographicException(Word, errorIndex!.Value, "Invalid orthography");


    public Syllable[]? TrySplit(out int? errorIndex)
    {
        errorIndex = null;
        _syllables = [];


        var word = Word.Trim();

        if (word.Length == 0)
            return [];

        // the first letter may be an alone vowel: a-la-fa-ve-te
        //                                         ^
        TryToAddSingleSyllable(word);

        var startIndex = _syllables.Count > 0 ? 1 : 0;
        for (var i = startIndex; i < word.Length; i++)
        {
            // single-syllables are: a, e, i, o, u
            // bi-syllables are: ka, ke, sa, se, pi...
            // tri-syllables are: kas, kes, sas, ses, pis...

            if (AddedTriSyllableDelimiterToLast(word, i) || AddedBiSyllable(word, ref i))
                continue;

            errorIndex = i;
            return null;
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




    public int? GetOrthographyErrorIndex()
    {
        TrySplit(out var errorIndex);
        return errorIndex;
    }


    public bool IsCorrect()
        => GetOrthographyErrorIndex() is null;




    public static bool IsSyllableValid(string syllable)
    {
        switch (syllable.Length)
        {
            case < 1 or > 3:
                break;

            // alone vowels, like in "a-la-fa-ve-te"
            //                        ^
            case 1:
                return Alphabet.Vowels.Contains(syllable[0]) && !Alphabet.Consonants.Contains(syllable[0]);

            case 2:
                return Alphabet.Consonants.Contains(syllable[0]) && Alphabet.Vowels.Contains(syllable[1]);

            case 3:
                return Alphabet.Consonants.Contains(syllable[0]) && Alphabet.Vowels.Contains(syllable[1])
                                                                 && syllable[2] is 's';
        }

        return false;
    }
}
