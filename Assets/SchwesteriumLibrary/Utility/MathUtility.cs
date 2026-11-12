/*
Author : schwesterium
Date   : 2026/08/01
*/

using System.Runtime.CompilerServices;
using UnityEngine;

namespace SchwesteriumLibrary.Utility
{
    public static class MathUtility
    {
        public const float PI2              = 6.28318530f;
        public const float HALF_PI          = 1.57079632f;
        public const float INV_PI           = 0.31830989f;
        public const float INV_SQRT_PI      = 0.56418958f;
        public const float INV_SQRT_PI2     = 0.39894228f;

        public const float INV2  = 0.5f;
        public const float INV3  = 0.33333333f;
        public const float INV4  = 0.25f;
        public const float INV5  = 0.2f;
        public const float INV6  = 0.16666667f;
        public const float INV7  = 0.14285714f;
        public const float INV8  = 0.125f;
        public const float INV9  = 0.11111111f;
        public const float INV10 = 0.1f;

        public const float INV_SQRT2 = 0.70710678f;
        public const float INV_SQRT3 = 0.57735027f;
        public const float INV_SQRT5 = 0.44721360f;

        //
        //  概要
        //      与えられた整数が偶数かどうかを判定する
        //  引数
        //      value:
        //  戻り値
        //      偶数であればtrue、奇数であればfalse
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEven(int value) => (value & 1) == 0;

        //
        //  概要
        //      正弦波の値を計算する
        //  引数
        //      a: 振幅, f: 周波数, t: 時間, phi: 初期位相
        //  戻り値
        //      時刻tにおける正弦波の値
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SineWave(float a, float f, float t, float phi)
        {
            return a * Mathf.Sin(PI2 * f * t - phi);
        }

        //
        //  概要
        //      矩形波の値を計算する
        //  引数
        //      a: 振幅, f: 周波数, t: 時間, phi: 初期位相
        //  戻り値
        //      時刻tにおける矩形波の値（±a）
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SquareWave(float a, float f, float t, float phi)
        {
            return a * Mathf.Sign(Mathf.Sin(PI2 * f * t - phi));
        }

        //
        //  概要
        //      三角波の値を計算する
        //  引数
        //      a: 振幅, f: 周波数, t: 時間, phi: 初期位相
        //  戻り値
        //      時刻tにおける三角波の値
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float PyramidalWave(float a, float f, float t, float phi)
        {
            return 2f * a * INV_PI * Mathf.Asin(Mathf.Sin(PI2 * f * t - phi));
        }

        //
        //  概要
        //      のこぎり波の値を計算する
        //  引数
        //      a: 振幅, f: 周波数, t: 時間
        //  戻り値
        //      時刻tにおけるのこぎり波の値
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SawtoothWave(float a, float f, float t)
        {
            return 2f * a * (f * t - Mathf.Floor(f * t)) - a;
        }

        //
        //  概要
        //      始点から終点へ向かう正規化された方向ベクトルを求める
        //  引数
        //      from: 始点, to: 終点
        //  戻り値
        //      正規化ベクトル
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 NormalizedDirection(Vector3 from, Vector3 to)
        {
            Vector3 result = default;
            result.x = to.x - from.x;
            result.y = to.y - from.y;
            result.z = to.z - from.z;
            result.Normalize();

            return result;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3 NormalizedDirection(in Vector3 from, in Vector3 to)
        {
            Vector3 result = default;
            result.x = to.x - from.x;
            result.y = to.y - from.y;
            result.z = to.z - from.z;
            result.Normalize();
            return result;
        }

        //
        //  概要
        //      2点間の距離の2乗を計算する
        //  引数
        //      a: 始点, b: 終点
        //  戻り値
        //      2点間の距離の2乗
        //
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrDistance(Vector3 a, Vector3 b)
        {
            float num = a.x - b.x;
            float num2 = a.y - b.y;
            float num3 = a.z - b.z;
            return num * num + num2 * num2 + num3 * num3;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float SqrDistance(in Vector3 a, in Vector3 b)
        {
            float num = a.x - b.x;
            float num2 = a.y - b.y;
            float num3 = a.z - b.z;
            return num * num + num2 * num2 + num3 * num3;
        }

        //
        //  概要
        //      目標位置の方向へ現在の向きを徐々に回転させつつ、回転後のforward方向へ前進する
        //  引数
        //      currentPosition: 現在のワールド位置, currentRotation: 現在のワールド回転, target: 目標位置, dt: deltaTime, moveSpeed: 移動速度, trunSpeed: 回転速度
        //  戻り値
        //      (位置、回転)
        //
        public static (Vector3, Quaternion) SteerTowards(Vector3 currentPosition, Quaternion currentRotation, Vector3 target, float dt, float moveSpeed, float trunSpeed)
        {
            float dx = target.x - currentPosition.x;
            float dy = target.y - currentPosition.y;
            float dz = target.z - currentPosition.z;

            //normalized + zero判定 => 2乗長のチェックだけにする
            if (dx * dx + dy * dy + dz * dz < 1E-10f) { return (currentPosition, currentRotation); }

            Vector3 resultPos = default;
            Quaternion resultRot = default;

            //LookRotationは内部で正規化するので、dirは正規化しなくてok
            Quaternion look = Quaternion.LookRotation(new Vector3(dx, dy, dz));
            Quaternion rot = Quaternion.Slerp(currentRotation, look, trunSpeed * dt);
            resultRot = rot;

            //transform.forward = rot * Vector3.forward を展開
            //参照 https://jp.mathworks.com/help/aeroblks/quaternionrotation.html#mw_3e561cb2-035f-473a-8d3a-0b064824515c
            float fx = 2f * (rot.x * rot.z + rot.w * rot.y);
            float fy = 2f * (rot.y * rot.z - rot.w * rot.x);
            float fz = 1f - 2f * (rot.x * rot.x + rot.y * rot.y);

            float step = moveSpeed * dt;
            resultPos = new Vector3(currentPosition.x + fx * step, currentPosition.y + fy * step, currentPosition.z + fz * step);

            return (resultPos, resultRot);
        }
    }
}