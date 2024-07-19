using HandTracker;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class HandRecording : MonoBehaviour
{
    public int reps = 1;
    private int i = 0;
    private MRTKHandTracker mrtk_ht = null;
    private JointExporter jointExporter;
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
                joint = mrtk_ht.jointToJSON(i / reps, tInf);
                jointExporter.appendJoint(joint);
                Debug.Log(joint);
            }
            i++;
        }
    }

    public void setRightHand()
    {
        if (mrtk_ht == null)
        {
            Debug.Log("Set right hand");
            mrtk_ht = new MRTKHandTracker(Handedness.Right);
            var handedness = mrtk_ht.attributeToJSON("handedness", "Right", "\n");
            jointExporter.appendAttribute(handedness);
        }
    }

    public void setLeftHand()
    {
        if (mrtk_ht == null)
        {
            Debug.Log("Set left hand");
            mrtk_ht = new MRTKHandTracker(Handedness.Left);
            var handedness = mrtk_ht.attributeToJSON("handedness", "Left", "\n");
            jointExporter.appendAttribute(handedness);
        }
    }

    public void setReady()
    {
        if (mrtk_ht != null && !ready)
        {
            Debug.Log("User ready");
            ready = true;
            var time = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var startTime = mrtk_ht.attributeToJSON("startTime", time);
            jointExporter.appendAttribute(startTime);
        }
    }

    public void initiateStop()
    {
        if (ready && !stop)
        {
            Debug.Log("User initiated stop");
            stop = true;
        }
    }

    private void OnDestroy()
    {
        jointExporter.Dispose();
    }
}
