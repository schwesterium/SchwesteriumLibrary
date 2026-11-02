/*
Author : schwesterium
Date   : 2026/10/29
*/

using SchwesteriumLibrary.Collections;
using UnityEngine;

namespace SchwesteriumLibrary.Audio
{
    [CreateAssetMenu(fileName = "AudioClipsData", menuName = "SchwesteriumLibrary/AudioManager/AudioClipsData")]
    public class AudioClipsData : ScriptableObject
    {
        public SerializableDictionary<string, AudioClip> ClipsDict = new();
    }
}