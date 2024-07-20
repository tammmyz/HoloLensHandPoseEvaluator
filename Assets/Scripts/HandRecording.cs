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
    public Boolean ready { get; private set; }
    public Boolean stop { get; private set; }

    void Start()
    {
        ready = false;
        stop = false;
        jointExporter = new JointExporter("test");
    }

    void Update()
    {
        if (!stop && mrtk_ht != null && ready) {
            string joint;
            // Update joints every <reps> number of iterations
            if (i % reps == 0)
            {
                var tInf = mrtk_ht.updatePose();
                joint = mrtk_ht.jointToJSON(i / reps, tInf);
                jointExporter.appendToFile(joint);
                Debug.Log(joint);
            }
            i++;
        }
    }

    // Set hand tracker to track right hand
    public void setRightHand()
    {
        if (mrtk_ht == null)
        {
            Debug.Log("Set right hand");
            mrtk_ht = new MRTKHandTracker(Handedness.Right);
            var handedness = mrtk_ht.attributeToJSON("handedness", "Right", "\n");
            jointExporter.appendToFile(handedness);
        }
    }

    // Set hand tracker to track left hand
    public void setLeftHand()
    {
        if (mrtk_ht == null)
        {
            Debug.Log("Set left hand");
            mrtk_ht = new MRTKHandTracker(Handedness.Left);
            var handedness = mrtk_ht.attributeToJSON("handedness", "Left", "\n");
            jointExporter.appendToFile(handedness);
        }
    }

    // Start recording hand joints
    public void setReady()
    {
        if (mrtk_ht != null && !ready)
        {
            Debug.Log("User ready");
            ready = true;
            var time = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            var startTime = mrtk_ht.attributeToJSON("startTime", time);
            jointExporter.appendToFile(startTime);
        }
    }

    // Trigger stop sequence
    public void initiateStop()
    {
        if (ready && !stop)
        {
            Debug.Log("User initiated stop");
            stop = true;
            jointExporter.writeFile();
        }
    }
}
