using UnityEngine;

public class MissionUIManager : MonoBehaviour
{
    [Header("Mission UI Elements")]
    [SerializeField] private GameObject missionUI;
    [SerializeField] private GameObject mainMenuUI;

    [Header("Tab Contents")]
    [SerializeField] private GameObject missionContent;
    [SerializeField] private GameObject achievementContent;

    [Header("Slot Prefab")]
    [SerializeField] private MissionSlotUI slotPrefab;

    private void Start()
    {
        ShowMissionTab();
    }

    public void ShowMissionUI()
    {
        missionUI.SetActive(true);
        mainMenuUI.SetActive(false);

        RefreshUI();
    }

    public void HideMissionUI()
    {
        missionUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void ShowMissionTab()
    {
        missionContent.SetActive(true);
        achievementContent.SetActive(false);

        RefreshMissionUI();

        Debug.Log("Mission Tab Opened");
    }

    public void ShowAchievementTab()
    {
        missionContent.SetActive(false);
        achievementContent.SetActive(true);

        RefreshAchievementUI();

        Debug.Log("Achievement Tab Opened");
    }

    public void RefreshUI()
    {
        RefreshMissionUI();
        RefreshAchievementUI();
    }

    void RefreshMissionUI()
    {
        // Clear old mission slots
        foreach (Transform child in missionContent.transform)
        {
            Destroy(child.gameObject);
        }

        var missions =
            MissionManager.Instance.saveData.activeMissions;

        for (int i = 0; i < missions.Count; i++)
        {
            MissionProgress mission = missions[i];

            MissionDefinition definition =
                MissionManager.Instance.GetMissionDefinition(
                    mission.missionId);

            MissionSlotUI slot =
                Instantiate(
                    slotPrefab,
                    missionContent.transform);

            slot.Setup(
                definition.title,
                mission.progress,
                definition.target,
                mission.completed,
                i,
                true);
        }
    }

    void RefreshAchievementUI()
    {
        // Clear old achievement slots
        foreach (Transform child in achievementContent.transform)
        {
            Destroy(child.gameObject);
        }

        var achievements =
            MissionManager.Instance.saveData.achievements;

        for (int i = 0; i < achievements.Count; i++)
        {
            AchievementProgress achievement =
                achievements[i];

            AchievementDefinition definition =
                MissionManager.Instance.GetAchievementDefinition(
                    achievement.achievementId);

            MissionSlotUI slot =
                Instantiate(
                    slotPrefab,
                    achievementContent.transform);

            slot.Setup(
                definition.title,
                achievement.progress,
                definition.target,
                achievement.completed &&
                !achievement.claimed,
                i,
                false);
        }
    }
}