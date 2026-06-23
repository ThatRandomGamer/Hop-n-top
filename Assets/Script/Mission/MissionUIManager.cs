using UnityEngine;

public class MissionUIManager : MonoBehaviour
{
    [Header ("Mission UI Elements")]
    [SerializeField] private GameObject missionUI;
    [SerializeField] private GameObject mainMentUI;


    public void ShowMissionUI()
    {
        missionUI.SetActive(true);
        mainMentUI.SetActive(false);
    }

    public void HideMissionUI()
    {
        missionUI.SetActive(false);
        mainMentUI.SetActive(true);
    }



}
