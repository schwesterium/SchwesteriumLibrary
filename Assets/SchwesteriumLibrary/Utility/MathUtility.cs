/*
Author : schwesterium
Date   : 2026/08/01
*/

using UnityEngine;

namespace SchwesteriumLibrary.Utility
{
    public class MathUtility
    {
        public const float INVERSE_PI = 0.318309877f;
        public const float PI2 = 6.283185306f;

        /// <summary>
        /// 整数が偶数かどうかを判定する
        /// </summary>
        /// <param name="value"></param>
        public static bool IsEven(int value) => (value & 1) == 0;

        //===各波については、https://www.desmos.com/calculator/7kdyrhniw4 ここからグラフが見れます====

        /// <summary>
        /// 正弦波
        /// </summary>
        /// <param name="a">振幅</param>
        /// <param name="f">周波数</param>
        /// <param name="t">時間</param>
        /// <param name="phi">初期位相</param>
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
        public static float PyramidalWave(float a, float f, float t, float phi)
        {
            return 2f * a * INVERSE_PI * Mathf.Asin(Mathf.Sin(f * t - phi));
        }

        /// <summary>
        /// のこぎり波
        /// </summary>
        /// <param name="a">振幅</param>
        /// <param name="f">周波数</param>
        /// <param name="t">時間</param>
        public static float SawtoothWave(float a, float f, float t)
        {
            return 2f * a * (f * t - Mathf.Floor(f * t)) - a;
        }

        //==============================================================================
    }
}