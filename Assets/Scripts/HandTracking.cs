using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class HandTracking1 : MonoBehaviour

{
    MRTKPoseEstimator mrtk_pe;
    JointExporter jointExporter;

    void Start()
    {
        mrtk_pe = new MRTKPoseEstimator(Handedness.Right);
        jointExporter = new JointExporter("test");
        mrtk_pe.updatePose();
        string joint = mrtk_pe.toTxt();
        jointExporter.appendJoint(joint);
        Debug.Log("Finished");
    }

    void Update()
    {
        //mrtk_pe.updatePose();
        //Debug.Log(mrtk_pe.toTxt());
    }

}
