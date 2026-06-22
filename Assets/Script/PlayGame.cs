using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayGame : MonoBehaviour
{
    [SerializeField] public string LevelName;
    public void Play()
    {
        //COMMENTED OUT CODE UNTIL AD MANAGER GETS
        //REINTEGRATED AFTER THAT YOU CAN SETUP PROPERLY

        // Hide Banner Before Game Starts
        // AdManager.Instance.HideBannerAd();

        // Show Interstitial Ad
        // After closing ad, "Bird" scene will load
        //  AdManager.Instance.ShowInterstitialAd(LevelName);

        AdManager.Instance.ShowInterstitialAd(LevelName);
    }
}