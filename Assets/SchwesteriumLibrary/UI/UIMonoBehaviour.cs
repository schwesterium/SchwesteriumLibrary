/*
Author : schwesterium
Date   : 2026/11/02
*/

using UnityEngine;

namespace SchwesteriumLibrary.UI
{
    public abstract class UIMonoBehaviour : MonoBehaviour
    {
        protected RectTransform rectTransform = null;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            OnAwake();
        }

        protected virtual void OnAwake() { }
    }

}