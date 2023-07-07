using UnityEngine;

namespace DredPack.Help
{
    public static class InverseCurve
    {
        public static AnimationCurve Get(AnimationCurve curve)
        {
            AnimationCurve mirrored = new AnimationCurve();

            for (int i = 0; i < curve.keys.Length; i++)
            {
                Keyframe originalKeyframe = curve.keys[i];
                Keyframe mirroredKeyframe = new Keyframe(1f - originalKeyframe.time, originalKeyframe.value, -originalKeyframe.inTangent, -originalKeyframe.outTangent);
                mirrored.AddKey(mirroredKeyframe);
            }

            return mirrored;
        }
    }
}