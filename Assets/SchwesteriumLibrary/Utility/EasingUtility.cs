/*
Author : schwesterium
Date   : 2026/08/01
*/


using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SchwesteriumLibrary.Utility
{
    //https://easings.net/ja#
    public static class EasingUtility
    {
        public enum EaseType
        {
            Linear,
            InSine, OutSine, InOutSine,
            InQuad, OutQuad, InOutQuad,
            InCubic, OutCubic, InOutCubic,
            InQuart, OutQuart, InOutQuart,
            InQuint, OutQuint, InOutQuint,
            InExpo, OutExpo, InOutExpo,
            InCirc, OutCirc, InOutCirc,
            InBack, OutBack, InOutBack,
            InElastic, OutElastic, InOutElastic,
            InBounce, OutBounce, InOutBounce,
        }

        public static float Evaluate(float x, EaseType type) => type switch
        {
            EaseType.Linear => x,

            EaseType.InSine => EaseInSine(x),
            EaseType.OutSine => EaseOutSine(x),
            EaseType.InOutSine => EaseInOutSine(x),

            EaseType.InQuad => EaseInQuad(x),
            EaseType.OutQuad => EaseOutQuad(x),
            EaseType.InOutQuad => EaseInOutQuad(x),

            EaseType.InCubic => EaseInCubic(x),
            EaseType.OutCubic => EaseOutCubic(x),
            EaseType.InOutCubic => EaseInOutCubic(x),

            EaseType.InQuart => EaseInQuart(x),
            EaseType.OutQuart => EaseOutQuart(x),
            EaseType.InOutQuart => EaseInOutQuart(x),

            EaseType.InQuint => EaseInQuint(x),
            EaseType.OutQuint => EaseOutQuint(x),
            EaseType.InOutQuint => EaseInOutQuint(x),

            EaseType.InExpo => EaseInExpo(x),
            EaseType.OutExpo => EaseOutExpo(x),
            EaseType.InOutExpo => EaseInOutExpo(x),

            EaseType.InCirc => EaseInCirc(x),
            EaseType.OutCirc => EaseOutCirc(x),
            EaseType.InOutCirc => EaseInOutCirc(x),

            EaseType.InBack => EaseInBack(x),
            EaseType.OutBack => EaseOutBack(x),
            EaseType.InOutBack => EaseInOutBack(x),

            EaseType.InElastic => EaseInElastic(x),
            EaseType.OutElastic => EaseOutElastic(x),
            EaseType.InOutElastic => EaseInOutElastic(x),

            EaseType.InBounce => EaseInBounce(x),
            EaseType.OutBounce => EaseOutBounce(x),
            EaseType.InOutBounce => EaseInOutBounce(x),

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
        };

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInSine(float x) => 1f - Mathf.Cos(x * Mathf.PI * 0.5f);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutSine(float x) => 1f - Mathf.Sin(x * Mathf.PI * 0.5f);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutSine(float x) => -(Mathf.Cos(Mathf.PI * x) - 1f) * 0.5f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInQuad(float x) => x * x;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutQuad(float x) => 1f - (1f - x) * (1f - x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutQuad(float x) => x < 0.5f ? 2f * x * x : 1f - (-2f * x + 2f) * (-2f * x + 2f) * 0.5f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInCubic(float x) => x * x * x;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutCubic(float x) => 1 - (1 - x) * (1 - x) * (1 - x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutCubic(float x) => x < 0.5f ? 4f * x * x * x : 1f - (-2f * x + 2f) * (-2f * x + 2f) * (-2f * x + 2f) * 0.5f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInQuart(float x) => x * x * x * x;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutQuart(float x) => 1 - (1 - x) * (1 - x) * (1 - x) * (1 - x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutQuart(float x) => x < 0.5f ? 8f * x * x * x * x : 1f - (-2f * x + 2f) * (-2f * x + 2f) * (-2f * x + 2f) * (-2f * x + 2f) * 0.5f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInQuint(float x) => x * x * x * x * x;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutQuint(float x) => 1 - (1 - x) * (1 - x) * (1 - x) * (1 - x) * (1 - x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutQuint(float x) => x < 0.5f ? 16f * x * x * x * x * x : 1 - (-2f * x + 2f) * (-2f * x + 2f) * (-2f * x + 2f) * (-2f * x + 2f) * (-2f * x + 2f) * 0.5f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInExpo(float x) => x == 0 ? 0 : Mathf.Pow(2, 10f * x - 10f);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutExpo(float x) => x == 1 ? 1 : 1 - Mathf.Pow(2f, -10f * x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutExpo(float x)
        {
            if (x == 0) { return 0; }
            if (x == 1) { return 1; }
            if (x < 0.5) { return Mathf.Pow(2f, 20f * x - 10f) * 0.5f; }

            return (2 - Mathf.Pow(2f, -20f * x + 10f)) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInCirc(float x) => 1f - Mathf.Sqrt(1f - x * x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutCirc(float x) => Mathf.Sqrt(1f - (x - 1f) * (x - 1f));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutCirc(float x) => x < 0.5f 
            ? (1f - Mathf.Sqrt(1f - (2f * x) * (2f * x))) * 0.5f : (Mathf.Sqrt(1f - (-2f * x + 2f) * (-2f * x + 2f)) + 1f) * 0.5f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInBack(float x)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;

            return c3 * x * x * x - c1 * x * x;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutBack(float x)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1f;

            return 1f + c3 * (x - 1f) * (x - 1f) * (x - 1f) + c1 * (x - 1f) * (x - 1f);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutBack(float x)
        {
            const float c1 = 1.70158f;
            const float c2 = c1 * 1.525f;

            return x < 0.5f
              ? ((2f * x) * (2f * x) * ((c2 + 1f) * 2f * x - c2)) * 0.5f
              : ((2f * x - 2f) * (2f * x - 2f) * ((c2 + 1f) * (x * 2f - 2f) + c2) + 2f) * 0.5f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInElastic(float x)
        {
            const float c4 = MathUtility.PI2 * 0.33333f;

            if (x == 0) { return 0f; }
            if (x == 1) { return 1f; }

            return -Mathf.Pow(2f, 10f * x - 10f) * Mathf.Sin((x * 10f - 10.75f) * c4);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutElastic(float x)
        {
            const float c4 = MathUtility.PI2 * 0.33333f;

            if (x == 0) { return 0f; }
            if (x == 1) { return 1f; }

            return Mathf.Pow(2f, -10f * x) * Mathf.Sin((x * 10f - 0.75f) * c4) + 1f;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutElastic(float x)
        {
            const float c5 = MathUtility.PI2 * 0.22222f;

            if (x == 0) { return 0f; }
            if (x == 1) { return 1f; }
            if (x < 0.5f) { return -(Mathf.Pow(2f, 20f * x - 10f) * Mathf.Sin((20f * x - 11.125f) * c5)) * 0.5f; }

            return (Mathf.Pow(2f, -20f * x + 10f) * Mathf.Sin((20f * x - 11.125f) * c5)) * 0.5f + 1f;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInBounce(float x) => 1 - EaseOutBounce(1 - x);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseOutBounce(float x)
        {
            const float n1 = 7.5625f;
            const float d1 = 2.75f;

            if (x < 1f / d1)
            {
                return n1 * x * x;
            }
            else if (x < 2f / d1)
            {
                x -= 1.5f / d1;
                return n1 * x * x + 0.75f;
            }
            else if (x < 2.5f / d1)
            {
                x -= 2.25f / d1;
                return n1 * x * x + 0.9375f;
            }
            else
            {
                x -= 2.625f / d1;
                return n1 * x * x + 0.984375f;
            }
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float EaseInOutBounce(float x)
        {
            return x < 0.5f ? (1f - EaseOutBounce(1f - 2f * x)) * 0.5f : (1f + EaseOutBounce(2f * x - 1f)) * 0.5f;
        }
    }
}