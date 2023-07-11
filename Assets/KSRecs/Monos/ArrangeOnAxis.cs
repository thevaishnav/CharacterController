[System.Serializable]
public class ArrangeOnAxis
{
    public enum ArrangeMode
    {
        DontArrange,
        ZigZag,
        StartStep,
        StartEnd,
        CenterStep,
        Scatter
    }

    public ArrangeMode mode = ArrangeMode.DontArrange;
    public float val1;
    public float val2;
    public int alternate;

    public float GetNextValue(int counter, int totalCount, float defVal)
    {
        if (alternate > 1) counter /= alternate;

        if (mode == ArrangeMode.DontArrange) return defVal;
        if (mode == ArrangeMode.ZigZag) return ((((float)counter) % 2f) == 0f) ? val1 : val2;
        if (mode == ArrangeMode.StartStep) return val2 * counter + val1;
        if (mode == ArrangeMode.StartEnd) return ((val2 - val1) * counter / totalCount) + val1;
        if (mode == ArrangeMode.CenterStep) return val1 + (val2 * (counter + 0.5f - ((float)totalCount) / 2f));
        return defVal;
    }
}