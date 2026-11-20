/*
Author : tosa yuto
Date   : 2026/10/29
*/

using System;
using UnityEngine;

namespace SchwesteriumLibrary.Particle
{
    /// <summary>
    /// Gradientで設定した色をもとに、0~1の範囲でパーティクルの色を変えてくれる
    /// </summary>
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleColorByValue : MonoBehaviour
    {
        [SerializeField]
        private Gradient _gradient = new Gradient();

        [SerializeField]
        private ParticleSystem _particle = null;

        public void Apply(float normalizedValue)
        {
            var main = _particle.main;
            main.startColor = _gradient.Evaluate(normalizedValue);
        }
    }
}