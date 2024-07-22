using Microsoft.MixedReality.Toolkit.Utilities;
using System;
using System.Diagnostics;
using UnityEngine;

namespace HandTracker
{
    // General base class for hand pose estimation models
    // Inherited by MRTKHandTracker
    public class IHandTracker
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

        public Stopwatch sw = new Stopwatch();

        // Base method to update pose, to be specified in inherited methods
        // @Returns Model inference time in milliseconds
        public virtual float updatePose() { return 0; }

        private string indent = "  ";

        public string attributeToJSON(string key, string value, string lastLineEnd=",\n")
        {
            return $"{lastLineEnd}{indent}\"{key}\": \"{value}\"";
        }

        // Method to format the pose estimation data as a JSON
        // @Returns String formatted with joint and inference data
        public string jointToJSON(int i, float inferenceTime=-9999, string lastLineEnd=",\n")
        {
            string timestamp = System.DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssZ");
            string export = $"{lastLineEnd}{indent}\"{i}\": {{\n";
            export += indent + formatVector3List("thumb", thumbPos, indent + indent);
            export += indent + formatVector3List("index", indexPos, indent + indent);
            export += indent + formatVector3List("middle", middlePos, indent + indent);
            export += indent + formatVector3List("ring", ringPos, indent + indent);
            export += indent + formatVector3List("pinky", pinkyPos, indent + indent);
            export += $"{indent}{indent}\"wrist\": [{wristPos.x}, {wristPos.y}, {wristPos.z}],\n";
            if (inferenceTime >= 0) {
                export += $"{indent}{indent}\"inference_time\": {inferenceTime},\n";
            }
            export += $"{indent}{indent}\"timestamp\": \"{timestamp}\"\n";
            export += $"{indent}}}";
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
}