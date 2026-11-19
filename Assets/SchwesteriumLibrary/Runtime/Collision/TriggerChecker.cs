/*
Author : schwesterium
Date   : 2026/10/08
*/

using System;
using UnityEngine;

namespace SchwesteriumLibrary.Collision
{
    /// <summary>
    /// Triggerが特定のレイヤーのオブジェクトを検知したらイベントを発火する
    /// <br>Unityの仕様上、トリガー判定にはどちらかにRigidBodyを割り当てる必要がある</br>
    /// </summary>
    [RequireComponent(typeof(Collider))]
    public class TriggerChecker : MonoBehaviour
    {
        public event Action<Collider> OnEnter;
        public event Action<Collider> OnExit;

        [SerializeField]
        [Tooltip("判定を取るレイヤーを指定する")]
        private LayerMask _targetLayer = default;

        public Collider Collider { get; private set; } = null;

        private void Awake()
        {
            Collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            var layer = 1 << other.gameObject.layer;

            if ((layer & _targetLayer) != 0)
            {
                OnEnter?.Invoke(other);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            var layer = 1 << other.gameObject.layer;

            if ((layer & _targetLayer) != 0)
            {
                OnExit?.Invoke(other);
            }
        }

        private void OnDestroy()
        {
            OnEnter = null;
            OnExit = null;
        }
    }
}