using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionSlotUI : MonoBehaviour
{
    [SerializeField] private TMP_Text taskText;
    [SerializeField] private Button claimButton;

    private bool isMission;
    private int index;

    public void Setup(
        string title,
        int progress,
        int target,
        bool completed,
        int taskIndex,
        bool mission)
    {
        index = taskIndex;
        isMission = mission;

        taskText.text =
            $"{title}\n{progress}/{target}";

        claimButton.interactable = completed;

        claimButton.onClick.RemoveAllListeners();
        claimButton.onClick.AddListener(ClaimReward);
    }

    void ClaimReward()
    {
        if (isMission)
        {
            MissionManager.Instance
                .ClaimMission(index);
        }
        else
        {
            MissionManager.Instance
                .ClaimAchievement(index);
        }

        FindFirstObjectByType<MissionUIManager>()
            ?.RefreshUI();
    }
}