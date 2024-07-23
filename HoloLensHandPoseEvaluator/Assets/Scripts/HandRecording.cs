using HandTracker;
using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class HandRecording : MonoBehaviour
{
    private int i = 0;
    private MRTKHandTracker mrtk_ht = null;
    private JointExporter jointExporter;
    private string handedness;
    public Boolean ready { get; private set; }
    public Boolean stop { get; private set; }
    public Boolean videoRecording { get; private set; }

    void Start()
    {
        ready = false;
        stop = false;
        videoRecording = false;
        jointExporter = new JointExporter("test");
    }

    void Update()
    {
        if (!stop && mrtk_ht != null && ready) {
            string joint;
            // Not update every frame
            var tInf = mrtk_ht.updatePose();
            joint = mrtk_ht.jointToJSON(i, tInf);
            jointExporter.appendToFile(joint);
            Debug.Log(joint);
            if (videoRecording)
            {
                var startRecordingFlag = mrtk_ht.attributeToJSON("startRecordingFrame", i.ToString());
                jointExporter.appendToFile(startRecordingFlag);
                videoRecording = false;
            }
        }
        i++;
    }

    // Set hand tracker to track right hand
    public void setRightHand()
    {
        if (mrtk_ht == null)
        {
            Debug.Log("Set right hand");
            mrtk_ht = new MRTKHandTracker(Handedness.Right);
            handedness = mrtk_ht.attributeToJSON("handedness", "Right", "\n");
        }
    }

    // Set hand tracker to track left hand
    public void setLeftHand()
    {
        if (mrtk_ht == null)
        {
            Debug.Log("Set left hand");
            mrtk_ht = new MRTKHandTracker(Handedness.Left);
            handedness = mrtk_ht.attributeToJSON("handedness", "Left", "\n");
        }
    }

    // Start recording hand joints
    public void setReady()
    {
        if (mrtk_ht != null && !ready)
        {
            Debug.Log("User ready");
            ready = true;
            stop = false;
            jointExporter.setFile();
            jointExporter.appendToFile(handedness);
            string time = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string startTime = mrtk_ht.attributeToJSON("startTime", time);
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
            ready = false;
            jointExporter.writeFile();
        }
    }

    public void setVideoRecording()
    {
        if (!videoRecording)
        {
            videoRecording = true;
        }
    }
    private void OnDestroy()
    {
        jointExporter.Dispose();
    }
}
