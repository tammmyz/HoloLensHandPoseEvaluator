using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FrameCount : MonoBehaviour
{
    private int absFrameCount;
    // Start is called before the first frame update
    void Start()
    {
        absFrameCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<TMP_Text>().text = absFrameCount.ToString();
        absFrameCount++;
    }
}
