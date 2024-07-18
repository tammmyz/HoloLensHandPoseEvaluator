using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class HandTracking1 : MonoBehaviour

{
    MRTKPoseEstimator mrtk_pe;
    JointExporter jointExporter;
    int i = 0;

    void Start()
    {
        mrtk_pe = new MRTKPoseEstimator(Handedness.Right);
        jointExporter = new JointExporter("test");
        mrtk_pe.updatePose();
        var joint = mrtk_pe.toTxt(i,"\n");
        jointExporter.appendJoint(joint);
        Debug.Log(joint);
    }

    void Update()
    {
        // Not update every frame
        if (i % 200 == 0)
        {
            if (i / 1000 > 1)
            {
                mrtk_pe.updatePose();
                var joint = mrtk_pe.toTxt(i / 1000);
                jointExporter.appendJoint(joint);
                Debug.Log(joint);
            }
        }
        i++;
    }

    private void OnDestroy()
    {
        jointExporter.Dispose();
    }
}
