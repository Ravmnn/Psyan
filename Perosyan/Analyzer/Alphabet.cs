namespace Perosyan.Analyzer;




public static class Alphabet
{
    public static readonly char[] Vowels = ['a', 'e', 'i', 'o', 'u'];

    // 'c' sounds like "ch"
    // "maca" (pc) -> "masha" (en)
    public static readonly char[] Consonants = ['b', 'c', 'f', 'k', 'l', 'm', 'n', 'p', 'r', 's', 't'];




    public static bool IsSyllableValid(string syllable)
    {
        switch (syllable.Length)
        {
            case < 1 or > 3:
                break;

            // alone vowels, like in "a-la-fa-ve-te"
            //                        ^
            case 1:
                return Vowels.Contains(syllable[0]) && !Consonants.Contains(syllable[0]);

            case 2:
                return Consonants.Contains(syllable[0]) && Vowels.Contains(syllable[1]);

            case 3:
                return Consonants.Contains(syllable[0]) && Vowels.Contains(syllable[1])
                                                        && syllable[2] is 's';
        }

        return false;
    }
}
