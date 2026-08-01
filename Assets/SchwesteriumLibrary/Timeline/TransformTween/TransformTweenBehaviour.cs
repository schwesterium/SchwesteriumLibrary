/*
Author : schwesterium
Date   : 2026/08/01
*/


using UnityEngine;
using UnityEngine.Playables;

namespace SchwesteriumLibrary.Timeline
{
    public class TransformTweenBehaviour : PlayableBehaviour
    {
        public Transform StartLocation = null;
        public Transform EndLocation = null;

        public AnimationCurve EasingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

        public Vector3 StartPosition = Vector3.zero;

        public Vector3 StartCoord = Vector3.zero;
        public Vector3 EndCoord = Vector3.zero;

        public bool UseCoordinate = false;

        public override void PrepareFrame(Playable playable, FrameData info)
        {
            if (UseCoordinate) { return; }

            if (StartLocation)
            {
                StartPosition = StartLocation.position;
            }
        }

        public float EvaluateEasingCurve(float time)
        {
            if (!IsEasingCurveNormalised())
            {
                Debug.LogError("Easing Curveを正規化してください！ (0 ,0) ~ (1, 1)");
                return 0f;
            }

            return EasingCurve.Evaluate(time);
        }

        //EasingCurveが0~1に正規化されているか
        private bool IsEasingCurveNormalised()
        {
            //起点が0に近いか
            if (!Mathf.Approximately(EasingCurve[0].time, 0f)) { return false; }

            if (!Mathf.Approximately(EasingCurve[0].value, 0f)) { return false; }

            //終点が1に近いか
            if (!Mathf.Approximately(EasingCurve[EasingCurve.length - 1].time, 1f)) { return false; }

            return Mathf.Approximately(EasingCurve[EasingCurve.length - 1].value, 1f);
        }
    }

}