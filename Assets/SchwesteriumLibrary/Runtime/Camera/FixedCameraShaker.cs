/*
Author : schwesterium
Date   : 2026/08/01
*/

using UnityEngine;

namespace SchwesteriumLibrary.Camera
{
    //固定カメラ用の振動スクリプト
    public class FixedCameraShaker : MonoBehaviour
    {
        private static bool _isShaking = false;
        private static float _shakeMagnitude = 0f;
        private static float _shakeTime = 0f;

        private bool _isChild = false;
        private Vector3 _startPosition = Vector3.zero;

#if UNITY_EDITOR
        //Domainのリロードをしない場合の変数リセット
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void DomainInit()
        {
            _isShaking = false;
            _shakeMagnitude = 0f;
            _shakeTime = 0f;
        }
#endif

        private void Awake()
        {
            _isChild = transform.parent == transform;

            _startPosition = transform.position;
        }

        /// <summary>
        /// カメラを揺らす
        /// </summary>
        /// <param name="duration">振動時間</param>
        /// <param name="magnitude">振動の強さ</param>
        public static void CameraShake(float duration, float magnitude)
        {
            _isShaking = true;

            _shakeMagnitude = magnitude;
            _shakeTime = duration;
        }

        private void LateUpdate()
        {
            if (!_isShaking) { return; }

            //振動時間中はカメラを振動させる
            if (_shakeTime > 0f)
            {
                float x = Random.Range(-1f, 1f) * _shakeMagnitude;
                float y = Random.Range(-1f, 1f) * _shakeMagnitude;

                var position = _isChild ? transform.localPosition : transform.position;
                position.x += x;
                position.y += y;

                if (_isChild)
                {
                    transform.localPosition = position;
                }
                else
                {
                    transform.position = position;
                }

                _shakeTime -= Time.deltaTime;
            }
            else
            {
                if (_isChild)
                {
                    transform.localPosition = _startPosition;
                }
                else
                {
                    transform.position = _startPosition;
                }

                _isShaking = false;
            }
        }
    }
}