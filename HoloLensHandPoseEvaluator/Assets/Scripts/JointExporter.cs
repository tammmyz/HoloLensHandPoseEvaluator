using System.IO;
using UnityEngine;

// Class for writing the joints from each estimated hand pose to a JSON file
public class JointExporter
{
    // Directory to save the file to be written
    [SerializeField]
    private string exportPath;

    // StreamWriter object to write to path
    private StreamWriter writer;

    // Complete filepath of the file to be writeen
    private string filepath;

    // Constructor method
    // @param namingPrefix: naming prefix for the file
    public JointExporter(string namingPrefix)
    {
        Debug.Log("Initialized jointExporter");
        // Determine path to write file to based on testing in editor
        // vs actual use on the HoloLens
#if UNITY_EDITOR
        exportPath = "Assets/Data/";
#else
        exportPath = Path.Combine(Application.persistentDataPath, "ModelCompData/");
#endif
        if (!Directory.Exists(exportPath))
        {
            Directory.CreateDirectory(exportPath);
        }
        filepath = getFilepath(exportPath, namingPrefix);
        Debug.Log(filepath);
        writer = new StreamWriter(filepath, true);
        writer.Write("{");
        writer.Flush();
    }

    // Writes new entry to the end of the JSON file
    // @param entry: JSON formatted text to write to file
    public void appendToFile(string entry)
    {
        writer.Write(entry);
        writer.Flush();
    }

    // Generate filename based on naming prefix and append to desired
    // @param exportPath: directory to save the file
    // @param prefix: naming prefix for the file
    // @Returns Complete filepath for file
    private string getFilepath(string exportPath, string prefix)
    {
        var currentDate = System.DateTime.Now.ToString("_yyyyMMdd_HHmmss");
        return exportPath + prefix + currentDate + ".json";
    }

    public void Dispose()
    {
        writer.Write("\n}");
        writer.Close();
    }
}
