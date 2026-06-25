using UnityEngine;

public static class MissionDatabaseLoader
{
    public static MissionDatabase LoadMissions()
    {
        TextAsset file =
            Resources.Load<TextAsset>("missions");

        return JsonUtility.FromJson<MissionDatabase>(
            file.text
        );
    }

    public static AchievementDatabase LoadAchievements()
    {
        TextAsset file =
            Resources.Load<TextAsset>("achievements");

        return JsonUtility.FromJson<AchievementDatabase>(
            file.text
        );
    }
}