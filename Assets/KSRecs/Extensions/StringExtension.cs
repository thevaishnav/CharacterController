using KSRecs.Utils;

namespace Assets.KSRecs.StringExtensions
{
    public static class StringExtensions
    {
        public static string PySubString(this string source, int start, int end = -1, int step = 1) => StringUtils.PySubString(source, start, end, step);
        public static string ToSentenceCase(this string source) => StringUtils.ToSentenceCase(source);
        public static string CapitalEachWord(this string source) => StringUtils.CapitalEachWord(source);
        public static string VariableToDisplayName(this string source) => StringUtils.VariableToDisplayName(source);
        public static string DisplayToVariableName(this string source) => StringUtils.DisplayToVariableName(source);
        public static string[] Split(this string source, string separator) => StringUtils.Split(source, separator);
    }
}