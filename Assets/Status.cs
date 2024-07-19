using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusUpdater : MonoBehaviour
{
    private TMP_Text tmpText;
    private string statusText;
    private string selectedHand = "None";
    private string recordingStatus = "Not recording";
    private string modelSelected = "MRTK (default)";

    [SerializeField]
    public HandRecording handRecSess;

    void Start()
    {
        tmpText = GetComponent<TMP_Text>();
        updateStatusText();
    }
    
    public void updateRightHand()
    {
        selectedHand = "Right";
        updateStatusText();
    }

    public void updateLefttHand()
    {
        selectedHand = "Left";
        updateStatusText();
    }

    public void updateRecording()
    {
        if (handRecSess.ready)
        {
            recordingStatus = "Recording";
            updateStatusText();
        }
    }

    public void updateNotRecording()
    {
        if (handRecSess.stop)
        {
            recordingStatus = "Not recording";
            updateStatusText();
        }
    }

    private void updateStatusText()
    {
        statusText = $"Selected hand: {selectedHand}\tRecording status: {recordingStatus}\tModels Selected: {modelSelected}";
        tmpText.text = statusText;
    }
}
