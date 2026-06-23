using System.IO;
using UnityEngine;

public static class SaveSystem
{
    static string SavePath =>
        Application.persistentDataPath + "/missionSave.json";

    public static void Save(SaveData data)
    {
        string json =
            JsonUtility.ToJson(data, true);

        File.WriteAllText(SavePath, json);
    }

    public static SaveData Load()
    {
        if (!File.Exists(SavePath))
            return null;

        string json =
            File.ReadAllText(SavePath);

        return JsonUtility.FromJson<SaveData>(json);
    }
}