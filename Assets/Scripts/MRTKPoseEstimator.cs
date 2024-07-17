using Microsoft.MixedReality.Toolkit.Utilities;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine;

public class MRTKPoseEstimator : IHandPose
{
    private TrackedHandJoint[] trackedThumb = new TrackedHandJoint[]
    {
        TrackedHandJoint.ThumbTip,
        TrackedHandJoint.ThumbDistalJoint,
        TrackedHandJoint.ThumbProximalJoint,
        TrackedHandJoint.ThumbMetacarpalJoint
    };

    private TrackedHandJoint[] trackedIndex = new TrackedHandJoint[]
    {
        TrackedHandJoint.IndexTip,
        TrackedHandJoint.IndexDistalJoint,
        TrackedHandJoint.IndexMiddleJoint,
        TrackedHandJoint.IndexKnuckle,
        TrackedHandJoint.IndexMetacarpal
    };

    private TrackedHandJoint[] trackedMiddle = new TrackedHandJoint[]
    {
        TrackedHandJoint.MiddleTip,
        TrackedHandJoint.MiddleDistalJoint,
        TrackedHandJoint.MiddleMiddleJoint,
        TrackedHandJoint.MiddleKnuckle,
        TrackedHandJoint.MiddleMetacarpal
    };

    private TrackedHandJoint[] trackedRing = new TrackedHandJoint[]
    {
        TrackedHandJoint.RingTip,
        TrackedHandJoint.RingDistalJoint,
        TrackedHandJoint.RingMiddleJoint,
        TrackedHandJoint.RingKnuckle,
        TrackedHandJoint.RingMetacarpal
    };

    private TrackedHandJoint[] trackedPinky = new TrackedHandJoint[]
    {
        TrackedHandJoint.PinkyTip,
        TrackedHandJoint.PinkyDistalJoint,
        TrackedHandJoint.PinkyMiddleJoint,
        TrackedHandJoint.PinkyKnuckle,
        TrackedHandJoint.PinkyMetacarpal
    };

    MixedRealityPose pose;

    public MRTKPoseEstimator(Handedness inHandedness)
    {
        Debug.Log("Initialized MRTKPoseEstimator");
        thumbPos = new Vector3[trackedThumb.Length];
        indexPos = new Vector3[trackedIndex.Length];
        middlePos = new Vector3[trackedMiddle.Length];
        ringPos = new Vector3[trackedRing.Length];
        pinkyPos = new Vector3[trackedPinky.Length];
        wristPos = new Vector3();
        handedness = inHandedness;
    }

    public override void updatePose()
    {
        storeJointPose(trackedThumb, thumbPos, handedness);
        storeJointPose(trackedIndex, indexPos, handedness);
        storeJointPose(trackedMiddle, middlePos, handedness);
        storeJointPose(trackedRing, ringPos, handedness);
        storeJointPose(trackedPinky, pinkyPos, handedness);
        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.Wrist, handedness, out pose))
        {
            wristPos = pose.Position;
        }
        //Debug.Log($"thumbPos0: {thumbPos[0]}\nindexPos1: {indexPos[1]}\nmiddlePos2: {middlePos[2]}\nringPos3: {ringPos[3]}\npinkyPos4: {pinkyPos[4]}\nwrist: {wristPos}");
    }

    private void storeJointPose(TrackedHandJoint[] jointGroup, Vector3[] handPart, Handedness handedness)
    {
        for (int i = 0; i < jointGroup.Length; i++)
        {
            if (HandJointUtils.TryGetJointPose(jointGroup[i], handedness, out pose))
            {
                handPart[i] = pose.Position;
            }
        }
    }

}
