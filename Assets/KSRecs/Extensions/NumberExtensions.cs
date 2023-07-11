using KSRecs.Utils;

namespace KSRecs.Extensions
{
    public static class NumberExtensions
    {

        #region Integers
        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMax) to Range 2 (newMin to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static int ConvertRange(int value, int oldMin, int oldMax, int newMin, int newMax) => NumberUtils.ConvertRange(value, oldMin, oldMax, newMin, newMax);

        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMid to oldMax) to Range 2 (newMin to newMid to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMid">Middle bond (virtual not actual, think of it as a divider) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMid">Middle bond (virtual not actual, think of it as a divider) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static int ConvertRange(int value, int oldMin, int oldMid, int oldMax, int newMin, int newMid, int newMax) => NumberUtils.ConvertRange(value, oldMin, oldMid, oldMax, newMin, newMid, newMax);

        /// <summary>
        /// Converts a number from Range 1 (Range specified by two elements at ith & (i+1)th index in oldMilestones array such that (i+1 element) > number > (i element) ) to respective range in newMilestones
        /// </summary>
        /// <returns>Converted number</returns>
        public static int ConvertMilestones(int value, int[] oldMilestones, int[] newMilestones) => NumberUtils.ConvertMilestones(value, oldMilestones, newMilestones);

        /// <summary>
        /// Converts a number from Range 1 (minimum to maximum) to Range 2 (maximum to minimum)
        /// </summary>
        /// <returns>Converted number</returns>
        public static int ReverseRange(int value, int minimum, int maximum) => NumberUtils.ConvertRange(value, minimum, maximum, maximum, minimum);

        /// <returns>Closest multiple of stepSize to value</returns>
        public static int Round(int value, int stepSize) => NumberUtils.Round(value, stepSize);
        #endregion

        #region Floats
        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMax) to Range 2 (newMin to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static float ConvertRange(float value, float oldMin, float oldMax, float newMin, float newMax) => NumberUtils.ConvertRange(value, oldMin, oldMax, newMin, newMax);

        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMid to oldMax) to Range 2 (newMin to newMid to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMid">Middle bond (virtual not actual, think of it as a divider) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMid">Middle bond (virtual not actual, think of it as a divider) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static float ConvertRange(float value, float oldMin, float oldMid, float oldMax, float newMin, float newMid, float newMax) => NumberUtils.ConvertRange(value, oldMin, oldMid, oldMax, newMin, newMid, newMax);

        /// <summary>
        /// Converts a number from Range 1 (Range specified by two elements at ith & (i+1)th index in oldMilestones array such that (i+1 element) > number > (i element) ) to respective range in newMilestones
        /// </summary>
        /// <returns>Converted number</returns>
        public static float ConvertMilestones(float value, float[] oldMilestones, float[] newMilestones) => NumberUtils.ConvertMilestones(value, oldMilestones, newMilestones);

        /// <summary>
        /// Converts a number from Range 1 (minimum to maximum) to Range 2 (maximum to minimum)
        /// </summary>
        /// <returns>Converted number</returns>
        public static float ReverseRange(float value, float minimum, float maximum) => NumberUtils.ConvertRange(value, minimum, maximum, maximum, minimum);

        /// <returns>Closest multiple of stepSize to value</returns>
        public static float Round(float value, float stepSize) => NumberUtils.Round(value, stepSize);
        #endregion

        #region Longs
        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMax) to Range 2 (newMin to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static long ConvertRange(long value, long oldMin, long oldMax, long newMin, long newMax) => NumberUtils.ConvertRange(value, oldMin, oldMax, newMin, newMax);

        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMid to oldMax) to Range 2 (newMin to newMid to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMid">Middle bond (virtual not actual, think of it as a divider) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMid">Middle bond (virtual not actual, think of it as a divider) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static long ConvertRange(long value, long oldMin, long oldMid, long oldMax, long newMin, long newMid, long newMax) => NumberUtils.ConvertRange(value, oldMin, oldMid, oldMax, newMin, newMid, newMax);

        /// <summary>
        /// Converts a number from Range 1 (Range specified by two elements at ith & (i+1)th index in oldMilestones array such that (i+1 element) > number > (i element) ) to respective range in newMilestones
        /// </summary>
        /// <returns>Converted number</returns>
        public static long ConvertMilestones(long value, long[] oldMilestones, long[] newMilestones) => NumberUtils.ConvertMilestones(value, oldMilestones, newMilestones);

        /// <summary>
        /// Converts a number from Range 1 (minimum to maximum) to Range 2 (maximum to minimum)
        /// </summary>
        /// <returns>Converted number</returns>
        public static long ReverseRange(long value, long minimum, long maximum) => NumberUtils.ConvertRange(value, minimum, maximum, maximum, minimum);

        /// <returns>Closest multiple of stepSize to value</returns>
        public static long Round(long value, long stepSize) => NumberUtils.Round(value, stepSize);
        #endregion

        #region Doubles
        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMax) to Range 2 (newMin to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static double ConvertRange(double value, double oldMin, double oldMax, double newMin, double newMax) => NumberUtils.ConvertRange(value, oldMin, oldMax, newMin, newMax);

        /// <summary>
        /// Converts a number from Range 1 (oldMin to oldMid to oldMax) to Range 2 (newMin to newMid to newMax)
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <param name="oldMin">Lower bond (inclusive) of old range</param>
        /// <param name="oldMid">Middle bond (virtual not actual, think of it as a divider) of old range</param>
        /// <param name="oldMax">Upper bond (exclusive) of old range</param>
        /// <param name="newMin">Lower bond (inclusive) of new range</param>
        /// <param name="newMid">Middle bond (virtual not actual, think of it as a divider) of new range</param>
        /// <param name="newMax">Upper bond (exclusive) of new range</param>
        /// <returns>Converted number</returns>
        public static double ConvertRange(double value, double oldMin, double oldMid, double oldMax, double newMin, double newMid, double newMax) => NumberUtils.ConvertRange(value, oldMin, oldMid, oldMax, newMin, newMid, newMax);

        /// <summary>
        /// Converts a number from Range 1 (Range specified by two elements at ith & (i+1)th index in oldMilestones array such that (i+1 element) > number > (i element) ) to respective range in newMilestones
        /// </summary>
        /// <returns>Converted number</returns>
        public static double ConvertMilestones(double value, double[] oldMilestones, double[] newMilestones) => NumberUtils.ConvertMilestones(value, oldMilestones, newMilestones);

        /// <summary>
        /// Converts a number from Range 1 (minimum to maximum) to Range 2 (maximum to minimum)
        /// </summary>
        /// <returns>Converted number</returns>
        public static double ReverseRange(double value, double minimum, double maximum) => NumberUtils.ConvertRange(value, minimum, maximum, maximum, minimum);

        /// <returns>Closest multiple of stepSize to value</returns>
        public static double Round(double value, double stepSize) => NumberUtils.Round(value, stepSize);
        #endregion

    }
}