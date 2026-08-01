/*
Author : schwesterium
Date   : 2026/08/01
*/


using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SchwesteriumLibrary.Timeline
{
    [TrackColor(0.458f, 0.568f, 0.470f)]
    [TrackClipType(typeof(TransformTweenClip))]
    [TrackBindingType(typeof(Transform))]
    public class TransformTweenTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            return ScriptPlayable<TransformTweenMixerBehaviour>.Create(graph, inputCount);
        }

        public override void GatherProperties(PlayableDirector director, IPropertyCollector driver)
        {
#if UNITY_EDITOR
            var transform = director.GetGenericBinding(this) as Transform;
            if (transform == null) { return; }

            //決められたプロパティ名を入れる
            driver.AddFromName<Transform>(transform.gameObject, "m_LocalPosition.x");
            driver.AddFromName<Transform>(transform.gameObject, "m_LocalPosition.y");
            driver.AddFromName<Transform>(transform.gameObject, "m_LocalPosition.z");

#endif
            base.GatherProperties(director, driver);
        }

    }
}