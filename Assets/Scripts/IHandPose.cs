using Microsoft.MixedReality.Toolkit.Utilities;
using UnityEngine;

// General base class for hand pose estimation models
// Inherited by MRTKPoseEstimator
public class IHandPose
{
    // Array of joint coordinates grouped by fingers
    public Vector3[] thumbPos;
    public Vector3[] indexPos;
    public Vector3[] middlePos;
    public Vector3[] ringPos;
    public Vector3[] pinkyPos;

    // Joint coordinate for the wrist
    public Vector3 wristPos;

    // Whether the specified hand is Right or Left
    public Handedness handedness;

    // Base method to update pose, to be specified in inherited methods
    public virtual void updatePose() { }

    // Method to format the pose estimation data as a JSON
    // @Returns String formatted with joint and inference data
    public string toTxt()
    {
        string export = "{\n";
        string indent = "  ";
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
        export += formatVector3List("thumb", thumbPos, indent);
        export += formatVector3List("index", indexPos, indent);
        export += formatVector3List("middle", middlePos, indent);
        export += formatVector3List("ring", ringPos, indent);
        export += formatVector3List("pinky", pinkyPos, indent);
        export += $"{indent}\"wrist\": [{wristPos.x}, {wristPos.y}, {wristPos.z}],\n";
        export += $"{indent}\"timestamp\": \"{timestamp}\"\n";
        export += "}\n";
        return export;
    }

    // Helper method for toTxt, formats a Vector3 array as JSON-formatted text
    // @param label: key value for vector, assuming "key": [vector] in JSON format
    // @param joints: vector array to be reformatted
    // @param indent: value used as indentation
    // @Returns String representing formatted array
    private string formatVector3List(string label, Vector3[] joints, string indent="")
    {
        string export = $"{indent}\"{label}\": [\n";
        Vector3 joint;
        for (int i = 0; i < joints.Length - 1; i++)
        {
            joint = joints[i];
            export += $"{indent}{indent}[{joint.x}, {joint.y}, {joint.z}],\n";
        }
        joint = joints[joints.Length-1];
        export += $"{indent}{indent}[{joint.x}, {joint.y}, {joint.z}]\n";
        export += $"{indent}],\n";
        return export;
    }
}
