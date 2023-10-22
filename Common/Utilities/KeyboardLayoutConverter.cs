using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    public static class KeyboardLayoutConverterExtensions
    {
        private static readonly Dictionary<char, char> PersianToEnglishMapping = new Dictionary<char, char>
{
    // ... other mappings
    { 'ش', 'a' }, { 'ذ', 'b' },  { 'ز', 'c' }, { 'ی', 'd' },
    { 'ث', 'e' }, { 'ب', 'f' },  { 'ل', 'g' }, { 'ا', 'h' },
    { 'ه', 'i' }, { 'ت', 'j' },  { 'ن', 'k' }, { 'م', 'l' },
    { 'ئ', 'm' }, { 'د', 'n' },  { 'خ', 'o' }, { 'ح', 'p' },
    { 'ض', 'q' }, { 'ق', 'r' },  { 'س', 's' }, { 'ف', 't' },
    { 'ع', 'u' }, { 'ر', 'v' },  { 'ص', 'w' }, { 'ط', 'x' },
    { 'غ', 'y' }, { 'ظ', 'z' } 
    // ... other mappings
};

        private static readonly Dictionary<char, char> EnglishToPersianMapping = new Dictionary<char, char>
{
    { 'ش', 'a' }, { 'ذ', 'b' },  { 'ز', 'c' }, { 'ی', 'd' },
    { 'ث', 'e' }, { 'ب', 'f' },  { 'ل', 'g' }, { 'ا', 'h' },
    { 'ه', 'i' }, { 'ت', 'j' },  { 'ن', 'k' }, { 'م', 'l' },
    { 'ئ', 'm' }, { 'د', 'n' },  { 'خ', 'o' }, { 'ح', 'p' },
    { 'ض', 'q' }, { 'ق', 'r' },  { 'س', 's' }, { 'ف', 't' },
    { 'ع', 'u' }, { 'ر', 'v' },  { 'ص', 'w' }, { 'ط', 'x' },
    { 'غ', 'y' }, { 'ظ', 'z' }
    // ... other mappings
};

        public static string Convert(this string input, bool isPersianToEnglish)
        {
            StringBuilder output = new StringBuilder();
            Dictionary<char, char> mapping = isPersianToEnglish ? PersianToEnglishMapping : EnglishToPersianMapping;

            foreach (char c in input)
            {
                if (mapping.ContainsKey(c))
                {
                    output.Append(mapping[c]);
                }
                else
                {
                    output.Append(c); // leave character as-is if it's not in the mapping
                }
            }

            return output.ToString();
        }
    }
}
