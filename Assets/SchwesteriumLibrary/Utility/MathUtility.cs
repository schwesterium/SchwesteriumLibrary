using System.Runtime.CompilerServices;
using UnityEngine;

public static class MathUtility
{
    //====円周率に関する定数====
    public const float PI2 =            6.28318530f;
    public const float HALF_PI =        1.57079632f;
    public const float INV_PI =         0.31830989f;
    public const float INV_SQRT_PI =    0.56418958f;
    public const float INV_SQRT_PI2 =   0.39894228f;

    //===逆数定数====
    public const float INV2 =           0.5f;
    public const float INV3 =           0.33333333f;
    public const float INV4 =           0.25f;
    public const float INV5 =           0.2f;
    public const float INV6 =           0.16666667f;
    public const float INV7 =           0.14285714f;
    public const float INV8 =           0.125f;
    public const float INV9 =           0.11111111f;

    //====逆平方根定数====
    public const float INV_SQRT2 =      0.70710678f;
    public const float INV_SQRT3 =      0.57735027f;
    public const float INV_SQRT5 =      0.44721360f;

    /// <summary>
    /// 整数が偶数かどうかを判定する
    /// </summary>
    /// <param name="value"></param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEven(int value) => (value & 1) == 0;

    //===各波については、https://www.desmos.com/calculator/7kdyrhniw4 ここからグラフが見れます====

    /// <summary>
    /// 正弦波
    /// </summary>
    /// <param name="a">振幅</param>
    /// <param name="f">周波数</param>
    /// <param name="t">時間</param>
    /// <param name="phi">初期位相</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SineWave(float a, float f, float t, float phi)
    {
        return a * Mathf.Sin(PI2 * f * t - phi);
    }
    /// <summary>
    /// 矩形波
    /// </summary>
    /// <param name="a">振幅</param>
    /// <param name="f">周波数</param>
    /// <param name="t">時間</param>
    /// <param name="phi">初期位相</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SquareWave(float a, float f, float t, float phi)
    {
        return a * Mathf.Sign(Mathf.Sin(PI2 * f * t - phi));
    }
    /// <summary>
    /// 三角波
    /// </summary>
    /// <param name="a">振幅</param>
    /// <param name="f">周波数</param>
    /// <param name="t">時間</param>
    /// <param name="phi">初期位相</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float PyramidalWave(float a, float f, float t, float phi)
    {
        return 2f * a * INV_PI * Mathf.Asin(Mathf.Sin(PI2 * f * t - phi));
    }
    /// <summary>
    /// のこぎり波
    /// </summary>
    /// <param name="a">振幅</param>
    /// <param name="f">周波数</param>
    /// <param name="t">時間</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float SawtoothWave(float a, float f, float t)
    {
        return 2f * a * (f * t - Mathf.Floor(f * t)) - a;
    }
    //==============================================================================
}