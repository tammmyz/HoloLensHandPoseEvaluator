using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JointExporter
{
    private List<Vector3[]> joints; // keep now for debugging

    [SerializeField]
    private string exportPath;
    private StreamWriter writer;
    private string filepath;

    public JointExporter(string namingPrefix)
    {
        Debug.Log("Initialized jointExporter");
#if UNITY_EDITOR
        exportPath = "Assets/Data/";
#else
        exportPath = Path.Combine(Application.persistentDataPath, "ModelCompData/");
        if (!Directory.Exists(exportPath)) {
            Directory.CreateDirectory(exportPath);
        }
#endif
        filepath = getFilepath(exportPath, namingPrefix);
        Debug.Log(filepath);
        writer = new StreamWriter(filepath, true);
    }

    public void appendJoint(string joint)
    {
        writer.Write(joint);
        writer.Flush();
    }

    public List<Vector3[]> getJoints()
    {
        return joints;
    }

    private string getFilepath(string exportPath, string prefix)
    {
        var currentDate = System.DateTime.Now.ToString("_yyyyMMdd_HHmmss");
        return exportPath + prefix + currentDate + ".json";
    }

    public void Destroy()
    {
        writer.Dispose();
    }
}
