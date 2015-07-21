using UnityEngine;
using System.Collections;

public class Utils
{
    private static readonly float zeroBuffer = 0.01f;

    public static Vector2 ZeroIfCloseToZero(Vector2 input)
    {
        Vector2 output = new Vector2();
        float x, y;
        x = Mathf.Abs(input.x) < zeroBuffer ? 0f : input.x;
        y = Mathf.Abs(input.y) < zeroBuffer ? 0f : input.y;
        output.Set(x, y);
        return output;
    }
}
