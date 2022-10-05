using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace KSRecs.Utils
{
    public static class StringUtils
    {
        public static string PySubString(string source, int start, int end = -1, int step = 1)
        {
            if (step == 0) throw new ArgumentException("Step of 0 is not allowed.");

            int endNew = end;
            if (endNew < 0) endNew = source.Length + 1 + endNew;

            string toReturn = "";
            for (int i = start; i < endNew; i += step)
            {
                toReturn += source[i];
            }

            return toReturn;
        }

        public static string ToSentenceCase(string source)
        {
            string lower = source.ToLower();
            return char.ToUpper(lower[0]) + lower.Substring(1);
        }

        public static string CapitalEachWord(string source)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(source.ToLower()); 
        }

        public static string VariableToDisplayName(string source)
        {
            string toReturn = "";
            bool skip = true;
            foreach (char c in source)
            {
                if (char.IsUpper(c))
                {
                    if (skip) skip = false;
                    else toReturn += " ";
                    toReturn += c;
                }
                else
                {
                    toReturn += c;
                }
            }

            return toReturn;
        }

        public static string DisplayToVariableName(string source)
        {
            string toReturn = "";
            bool capitalize = true;
            foreach (char c in source)
            {
                if (c == ' ')
                {
                    capitalize = true;
                    continue;
                }

                if (capitalize)
                {
                    toReturn += char.ToUpper(c);
                    capitalize = false;
                }
                else
                {
                    toReturn += char.ToLower(c);
                }
            }

            return toReturn;
        }

        public static string[] Split(string source, string separator)
        {
            return Regex.Split(source, separator);
        }
    }
}