using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class HandTracking1 : MonoBehaviour

{
    MRTKPoseEstimator mrtk_pe;

    void Start()
    {
        mrtk_pe = new MRTKPoseEstimator(Handedness.Right);
    }

    void Update()
    {
        mrtk_pe.updatePose();
        Debug.Log(mrtk_pe.toTxt());
    }

}
