using UnityEngine;

public class RewardedAds : MonoBehaviour
{
    private const string LAST_DAILY_AD = "LastDailyAd";

    // Daily Ad

    public void ShowDailyAd()
    {
        if (!CanClaimDailyReward())
        {
            Debug.Log("Daily reward already claimed.");
            return;
        }

        AdManager.Instance.ShowRewardedAd(GetDailyReward);
    }

    private bool CanClaimDailyReward()
    {
        if (!PlayerPrefs.HasKey(LAST_DAILY_AD))
            return true;

        System.DateTime lastClaim =
            System.DateTime.Parse(PlayerPrefs.GetString(LAST_DAILY_AD));

        return System.DateTime.UtcNow >= lastClaim.AddDays(1);
    }

    private void GetDailyReward()
    {
        PlayerPrefs.SetString(
            LAST_DAILY_AD,
            System.DateTime.UtcNow.ToString("o")
        );

        PlayerPrefs.Save();

        Debug.Log("Daily Reward Granted");

        // Add 100 coins here
    }

}
