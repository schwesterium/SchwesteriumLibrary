/*
Author : schwesterium
Date   : 2026/08/01
*/


using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace SchwesteriumLibrary.Timeline
{
    public class TransformTweenClip : PlayableAsset, ITimelineClipAsset
    {
        public ExposedReference<Transform> StartLocation;
        public ExposedReference<Transform> EndLocation;

        public bool UseCoordinate = false;

        public Vector3 StartCoord = Vector3.zero;
        public Vector3 EndCoord = Vector3.zero;

        public AnimationCurve EasingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public ClipCaps clipCaps => ClipCaps.Blending;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<TransformTweenBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();

            behaviour.UseCoordinate = UseCoordinate;

            if (UseCoordinate)
            {
                behaviour.StartCoord = StartCoord;
                behaviour.EndCoord = EndCoord;
            }
            else
            {
                behaviour.StartLocation = StartLocation.Resolve(graph.GetResolver());
                behaviour.EndLocation = EndLocation.Resolve(graph.GetResolver());
            }

            behaviour.EasingCurve = EasingCurve;

            return playable;
        }
    }
}