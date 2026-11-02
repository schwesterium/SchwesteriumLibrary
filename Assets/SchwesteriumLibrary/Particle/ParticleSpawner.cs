/*
Author : tosa yuto
Date   : 2026/10/28
*/

using SchwesteriumLibrary.Collections;
using SchwesteriumLibrary.LibSystem;
using UnityEngine;

namespace SchwesteriumLibrary.Particle
{
    public class ParticleSpawner : SingletonMonoBehaviour<ParticleSpawner>
    {
        [SerializeField]
        private SerializableDictionary<string, ParticleSystem> _particleDict = null;

        protected override void OnAwake()
        {
            _particleDict.TryCreateDictionary();
        }

        public void Play(string name, Vector3 position, Quaternion rotation = default, Transform parent = null)
        {
            var particle = Create(name, position, ref rotation, parent);
            if( particle == null){ return; }

            particle.Play();
        }

        public void Play(string name, float normalizedValue, Vector3 position, Quaternion rotation = default, Transform parent = null)
        {
            var particle = Create(name, position, ref rotation, parent);
            if (particle == null) { return; }

            if (particle.TryGetComponent<ParticleColorByValue>(out var particleColorByValue))
            {
                particleColorByValue.Apply(normalizedValue);
            }

            particle.Play();
        }

        private ParticleSystem Create(in string name, in Vector3 position, ref Quaternion rotation, Transform parent = null)
        {
            if (!_particleDict.ContainsKey(name))
            {
                Debug.LogWarning($"particle {name} は存在しない");
                return null;
            }

            if (rotation == default) { rotation = Quaternion.identity; }

            return  Instantiate(_particleDict.GetElement(name), position, rotation, parent);
        }
    }
}