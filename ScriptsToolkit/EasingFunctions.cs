







[System.Serializable]
public struct EasingFunctions
{
    public static float Linear(float x)
    {
        return x;
    }
    public static float Squared(float x)
    {
        return x*x;
    }
    public static float InverseSquared(float x)
    {
        return 1 - (1 - x) * (1 - x);
    }
    public static float SmoothSquared(float x)
    {
        return x < 0.5 ? x * x * 2 : (1 - (1 - x) * (1 - x) * 2);
    }
    public static float Cube(float x)
    {
        return x * x * x;
    }
}
