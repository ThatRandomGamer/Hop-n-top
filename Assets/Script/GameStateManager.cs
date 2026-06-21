using System.IO;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public PlayerController playerScript;
    public ChunkGenerator chunkGenerator;

    [Header("Debug & Testing")]
    [Tooltip("Check this to wipe the save file every time you hit Play.")]
    public bool forceFreshStartInEditor = true;

    [Tooltip("Prevents the game from saving when you hit the Stop button in the Editor.")]
    public bool disableAutoSaveInEditor = true;
    private string saveFilePath;

    void Awake()
    {
        saveFilePath = Application.persistentDataPath + "/gamestate.json";

#if UNITY_EDITOR
        //  DEBUG FEATURE: Delete the save file if we want a fresh run
        if (forceFreshStartInEditor && File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Debug: Wiped old save data for a fresh session.");
        }
#endif

        // Auto-load if a save exists when the app opens!
        LoadGameState();
    }
    void OnApplicationPause(bool isPaused)
    {
        if (isPaused)
        {
            SaveGameState();
        }
    }

    void OnApplicationQuit()
    {
#if UNITY_EDITOR
        // DEBUG FEATURE: Stop the Editor's Stop button from creating a new save
        if (disableAutoSaveInEditor)
        {
            Debug.Log("Debug: Skipped saving game state on quit.");
            return;
        }
#endif
        SaveGameState();
    }
    public void SaveGameState()
    {
        Save data = new Save();
        data.playerX = playerScript.transform.position.x;
        data.playerY = playerScript.transform.position.y;
        data.playerZ = playerScript.transform.position.z;
        data.currentRunDistance = playerScript.Distance;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(saveFilePath, json);
        Debug.Log("Game State frozen and saved!");
    }
    public void LoadGameState()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            Save data = JsonUtility.FromJson<Save>(json);

            //playerScript.transform.position = new Vector3(data.playerX, data.playerY, data.playerZ);


            File.Delete(saveFilePath);

            Debug.Log("Game State Loaded! Chunks rebuilt at Y: " + data.playerY);
        }
    }
}