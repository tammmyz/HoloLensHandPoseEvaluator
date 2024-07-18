using HandTracker;
using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

public class HandRecording : MonoBehaviour
{
    public int reps = 200;
    private int i = 0;
    private MRTKHandTracker mrtk_ht;
    private JointExporter jointExporter;

    void Start()
    {
        mrtk_ht = new MRTKHandTracker(Handedness.Right);
        jointExporter = new JointExporter("test");
        mrtk_ht.updatePose();
        var joint = mrtk_ht.toTxt(i,"\n");
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
                mrtk_ht.updatePose();
                var joint = mrtk_ht.toTxt(i / reps);
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
