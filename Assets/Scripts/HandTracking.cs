using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class HandTracking1 : MonoBehaviour

{
    public int reps = 200;
    private int i = 0;
    private MRTKPoseEstimator mrtk_pe;
    private JointExporter jointExporter;

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
        if (i % reps == 0)
        {
            if (i / reps > 1)
            {
                mrtk_pe.updatePose();
                var joint = mrtk_pe.toTxt(i / reps);
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
