using Microsoft.MixedReality.Toolkit.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHandPose
{
    public Vector3[] thumbPos;
    public Vector3[] indexPos;
    public Vector3[] middlePos;
    public Vector3[] ringPos;
    public Vector3[] pinkyPos;
    public Vector3 wristPos;
    public Handedness handedness;

    public virtual void updatePose() { }

    public void toJSON()
    {

    }
}
