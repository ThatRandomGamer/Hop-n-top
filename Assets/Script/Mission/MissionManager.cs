using System;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;

public class MissionManager : MonoBehaviour
{
    public static MissionManager Instance;

    MissionDatabase missionDB;
    AchievementDatabase achievementDB;

    public SaveData saveData;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        missionDB =
            MissionDatabaseLoader.LoadMissions();

        achievementDB =
            MissionDatabaseLoader.LoadAchievements();

        LoadGame();
    }

    void LoadGame()
    {
        saveData = SaveSystem.Load();

        if (saveData == null)
        {
            saveData = new SaveData();

            GenerateDailyMissions();

            InitializeAchievements();

            SaveGame();
        }

        CheckDailyReset();
    }

    void SaveGame()
    {
        SaveSystem.Save(saveData);
    }

    void InitializeAchievements()
    {
        foreach (var achievement
                 in achievementDB.achievements)
        {
            saveData.achievements.Add(
                new AchievementProgress
                {
                    achievementId = achievement.id
                });
        }
    }

    void CheckDailyReset()
    {
        string today =
            DateTime.UtcNow.Date.ToString();

        if (saveData.lastMissionResetDate != today)
        {
            GenerateDailyMissions();

            saveData.lastMissionResetDate = today;

            SaveGame();
        }
    }

    void GenerateDailyMissions()
    {
        saveData.activeMissions.Clear();

        AddRandomMission(MissionDifficulty.Easy);
        AddRandomMission(MissionDifficulty.Easy);
        AddRandomMission(MissionDifficulty.Medium);

        Debug.Log("=== ACTIVE MISSIONS ===");

        foreach (var mission in saveData.activeMissions)
        {
            Debug.Log($"Mission ID: {mission.missionId}");
        }
    }

    void AddRandomMission(MissionDifficulty difficulty)
    {
        List<MissionDefinition> candidates =
            new List<MissionDefinition>();

        foreach (var mission in missionDB.missions)
        {
            if (mission.difficulty != difficulty)
                continue;

            if (saveData.completedMissionIds
                .Contains(mission.id))
                continue;

            bool active = false;

            foreach (var current
                     in saveData.activeMissions)
            {
                if (current.missionId ==
                    mission.id)
                {
                    active = true;
                    break;
                }
            }

            if (!active)
                candidates.Add(mission);
        }

        if (candidates.Count == 0)
            return;

        var selected =
            candidates[
                UnityEngine.Random.Range(
                    0,
                    candidates.Count)];

        saveData.activeMissions.Add(
            new MissionProgress
            {
                missionId = selected.id
            });
    }

    public void AddTotalDistance(int amount)
    {
        saveData.totalDistance += amount;

        CheckAchievements();

        SaveGame();
    }

    public void AddTotalCoins(int amount)
    {
        saveData.totalCoins += amount;

        CheckAchievements();

        SaveGame();
    }

    public void AddUnlockedSkin()
    {
        saveData.unlockedSkins++;

        CheckAchievements();

        SaveGame();
    }

    public void AddUnlockedMap()
    {
        saveData.unlockedMaps++;

        CheckAchievements();

        SaveGame();
    }

    void CheckAchievements()
    {
        foreach (var progress
                 in saveData.achievements)
        {
            var definition =
                achievementDB.achievements
                .Find(x =>
                    x.id ==
                    progress.achievementId);

            switch (definition.type)
            {
                case TaskType.TotalDistance:
                    progress.progress =
                        saveData.totalDistance;
                    break;

                case TaskType.TotalCoins:
                    progress.progress =
                        saveData.totalCoins;
                    break;

                case TaskType.UnlockSkins:
                    progress.progress =
                        saveData.unlockedSkins;
                    break;

                case TaskType.UnlockMaps:
                    progress.progress =
                        saveData.unlockedMaps;
                    break;
            }

            progress.completed =
                progress.progress >=
                definition.target;
        }
    }

    public MissionDefinition GetMissionDefinition(int missionId)
    {
        return missionDB.missions.Find(
            x => x.id == missionId);
    }

    public AchievementDefinition GetAchievementDefinition(int achievementId)
    {
        return achievementDB.achievements.Find(
            x => x.id == achievementId);
    }

    public void UpdateRunDistance(int distance)
    {
        foreach (var mission in saveData.activeMissions)
        {
            MissionDefinition definition =
                GetMissionDefinition(mission.missionId);

            if (definition.type == TaskType.DistanceOneRun)
            {
                mission.progress = distance;

                if (distance >= definition.target)
                    mission.completed = true;
            }
        }
    }

    public void UpdateRunCoins(int coins)
    {
        foreach (var mission in saveData.activeMissions)
        {
            MissionDefinition definition =
                GetMissionDefinition(mission.missionId);

            if (definition.type == TaskType.CoinsOneRun)
            {
                mission.progress = coins;

                if (coins >= definition.target)
                    mission.completed = true;
            }
        }
    }

    public void ClaimMission(int index)
    {
        MissionProgress mission =
            saveData.activeMissions[index];

        if (!mission.completed)
            return;

        MissionDefinition definition =
            GetMissionDefinition(mission.missionId);

        Debug.Log("Claimed: " + definition.title);

        saveData.activeMissions.RemoveAt(index);

        AddRandomMission(definition.difficulty);

        SaveGame();
    }

    public void ClaimAchievement(int index)
    {
        AchievementProgress achievement =
            saveData.achievements[index];

        if (!achievement.completed)
            return;

        if (achievement.claimed)
            return;

        achievement.claimed = true;

        Debug.Log("Achievement Claimed");

        SaveGame();
    }

}