using System;
using System.Collections.Generic;

[Serializable]
public class MissionDefinition
{
    public int id;
    public string title;
    public TaskType type;
    public MissionDifficulty difficulty;

    public int target;
    public int reward;
}

[Serializable]
public class MissionDatabase
{
    public List<MissionDefinition> missions;
}

[Serializable]
public class AchievementDefinition
{
    public int id;
    public string title;

    public TaskType type;

    public int target;
    public int reward;
}

[Serializable]
public class AchievementDatabase
{
    public List<AchievementDefinition> achievements;
}

[Serializable]
public class MissionProgress
{
    public int missionId;

    public int progress;

    public bool completed;
    public bool claimed;
}

[Serializable]
public class AchievementProgress
{
    public int achievementId;

    public int progress;

    public bool completed;
    public bool claimed;
}

[Serializable]
public class SaveData
{
    public int totalDistance;
    public int totalCoins;

    public int unlockedSkins;
    public int unlockedMaps;

    public string lastMissionResetDate;

    public List<int> completedMissionIds =
        new List<int>();

    public List<MissionProgress> activeMissions =
        new List<MissionProgress>();

    public List<AchievementProgress> achievements =
        new List<AchievementProgress>();
}