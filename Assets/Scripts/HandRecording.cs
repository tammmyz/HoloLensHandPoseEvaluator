using HandTracker;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class HandRecording : MonoBehaviour
{
    public int reps = 200;
    private int i = 0;
    private MRTKHandTracker mrtk_ht = null;
    private JointExporter jointExporter;
    private Boolean first = true;
    private Boolean ready = false;
    private Boolean stop = false;

    void Start()
    {
        jointExporter = new JointExporter("test");
    }

    void Update()
    {
        if (!stop && mrtk_ht != null && ready) {
            string joint;
            // Not update every frame
            if (i % reps == 0)
            {
                var tInf = mrtk_ht.updatePose();
                joint = mrtk_ht.toJSON(i / reps, tInf);
                jointExporter.appendJoint(joint);
                Debug.Log(joint);
            }
            i++;
        }
    }

    public void setRightHand()
    {
        Debug.Log("Set right hand");
        mrtk_ht = new MRTKHandTracker(Handedness.Right);
        var handedness = mrtk_ht.toJSONHandedness();
        jointExporter.appendHandedness(handedness);
    }

    public void setLeftHand()
    {
        Debug.Log("Set left hand");
        mrtk_ht = new MRTKHandTracker(Handedness.Left);
        var handedness = mrtk_ht.toJSONHandedness();
        jointExporter.appendHandedness(handedness);
    }

    public void setReady()
    {
        Debug.Log("User ready");
        ready = true;
    }

    public void initiateStop()
    {
        Debug.Log("User initiated stop");
        stop = true;
    }

    private void OnDestroy()
    {
        jointExporter.Dispose();
    }
}
