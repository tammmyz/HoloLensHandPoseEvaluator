using Microsoft.MixedReality.Toolkit.Utilities;
using System.Net.NetworkInformation;
using UnityEngine;

public class IHandPose
{
    public Vector3[] thumbPos;
    public Vector3[] indexPos;
    public Vector3[] middlePos;
    public Vector3[] ringPos;
    public Vector3[] pinkyPos;
    public Vector3 wristPos;
    public Handedness handedness;

    public virtual void updatePose() { }

    public string toTxt()
    {
        string export = "{\n";
        string prefix = "  ";
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
        export += formatVector3List("thumb", thumbPos, prefix);
        export += formatVector3List("index", indexPos, prefix);
        export += formatVector3List("middle", middlePos, prefix);
        export += formatVector3List("ring", ringPos, prefix);
        export += formatVector3List("pinky", pinkyPos, prefix);
        export += $"{prefix}\"wrist\": [{wristPos.x}, {wristPos.y}, {wristPos.z}],\n";
        export += $"{prefix}\"timestamp\": \"{timestamp}\"\n";
        export += "}\n";
        return export;
    }

    private string formatVector3List(string label, Vector3[] joints, string prefix="")
    {
        string export = $"{prefix}\"{label}\": [\n";
        Vector3 joint;
        for (int i = 0; i < joints.Length - 1; i++)
        {
            joint = joints[i];
            export += $"{prefix}{prefix}[{joint.x}, {joint.y}, {joint.z}],\n";
        }
        joint = joints[joints.Length-1];
        export += $"{prefix}{prefix}[{joint.x}, {joint.y}, {joint.z}]\n";
        export += $"{prefix}],\n";
        return export;
    }
}
