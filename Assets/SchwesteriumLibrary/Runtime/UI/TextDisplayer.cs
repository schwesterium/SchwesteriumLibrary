/*
Author : schwesterium
Date   : 2026/08/01
*/


using TMPro;
using UnityEngine;

namespace SchwesteriumLibrary.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public abstract class TextDisplayer<T> : MonoBehaviour
    {
        protected TextMeshProUGUI _text = null;

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();

            OnStart();
        }

        public virtual void OnStart() { }

        public virtual void SetText(T text) => _text.SetText(text.ToString());
    }
}