/*
Author : schwesterium
Date   : 2026/08/01
*/


using UnityEngine;
using UnityEngine.Playables;

namespace SchwesteriumLibrary.Timeline
{
    public class TransformTweenMixerBehaviour : PlayableBehaviour
    {
        private bool _firstFrameMoved = false;

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            Transform trackBinding = playerData as Transform;

            if (trackBinding == null) { return; }

            //バインドされたオブジェクトの初期位置
            Vector3 initPosition = trackBinding.position;

            int inputCount = playable.GetInputCount();

            float positionTotalWeight = 0f;

            Vector3 blendedPosition = new Vector3(0f, 0f, 0f);

            for (int i = 0; i < inputCount; i++)
            {
                ScriptPlayable<TransformTweenBehaviour> playableInput = (ScriptPlayable<TransformTweenBehaviour>)playable.GetInput(i);
                TransformTweenBehaviour behaviour = playableInput.GetBehaviour();

                if (behaviour.UseCoordinate)
                {
                    float inputWeight = playable.GetInputWeight(i);

                    float normalisedTime = (float)(playableInput.GetTime() / playableInput.GetDuration());
                    float progress = behaviour.EvaluateEasingCurve(normalisedTime);

                    positionTotalWeight += inputWeight;

                    blendedPosition += Vector3.Lerp(behaviour.StartCoord, behaviour.EndCoord, progress) * inputWeight;
                }
                else
                {
                    if (behaviour.EndLocation == null) { continue; }

                    float inputWeight = playable.GetInputWeight(i);

                    if (!_firstFrameMoved && !behaviour.StartLocation)
                    {
                        behaviour.StartPosition = initPosition;
                    }

                    float normalisedTime = (float)(playableInput.GetTime() / playableInput.GetDuration());
                    float progress = behaviour.EvaluateEasingCurve(normalisedTime);

                    positionTotalWeight += inputWeight;

                    blendedPosition += Vector3.Lerp(behaviour.StartPosition, behaviour.EndLocation.position, progress) * inputWeight;

                }
            }

            //ブレンドされた位置にする
            blendedPosition += initPosition * (1f - positionTotalWeight);
            trackBinding.position = blendedPosition;

            _firstFrameMoved = true;
        }

        public override void OnPlayableDestroy(Playable playable)
        {
            _firstFrameMoved = false;
        }
    }
}