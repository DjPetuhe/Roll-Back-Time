using System.IO;
using UnityEngine;

public class SaveLoadManager
{
    private static readonly string s_saveLocation = Application.persistentDataPath + "/Save";
    private static readonly string s_totalDirectory = $"{s_saveLocation}/Save.json";

    public static Level LoadLevelFromJson()
    {
        string json = File.ReadAllText(s_totalDirectory);
        return JsonUtility.FromJson<Level>(json);
    }

    public static void SaveLevelToJson(Level level)
    {
        Directory.CreateDirectory(s_saveLocation);
        string json = JsonUtility.ToJson(level, true);
        File.WriteAllText(s_totalDirectory, json);
    }

    public static bool SaveFileExists() => File.Exists(s_totalDirectory);

    public static void DeletePreviousSave() => File.Delete(s_totalDirectory);
}