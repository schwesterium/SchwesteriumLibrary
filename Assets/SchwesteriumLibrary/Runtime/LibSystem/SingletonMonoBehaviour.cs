/*
Author : schwesterium
Date   : 2026/11/02
*/

using UnityEngine;

namespace SchwesteriumLibrary.LibSystem
{
    /// <summary>
    /// MonoBehaviourを継承したシングルトン
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        public static T Instance { get; private set; } = null;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = (T)this;

            OnAwake();
        }

        private void OnDestroy()
        {
            if(Instance == this) { Instance = null; }

            OnDestoryed();
        }

        protected virtual void OnAwake() { }
        protected virtual void OnDestoryed() { }
    }
}