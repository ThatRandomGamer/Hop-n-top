using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
using System;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;

    private BannerView bannerView;
    private InterstitialAd interstitialAd;
    private RewardedAd rewardedAd;

    [Header("Scene Names")]
    [SerializeField] private string mainMenuSceneName = "TitleScreen";

#if UNITY_ANDROID

    private string bannerAdUnitId = "ca-app-pub-3940256099942544/6300978111";
    private string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";

#elif UNITY_IPHONE

    private string bannerAdUnitId = "ca-app-pub-3940256099942544/2934735716";
    private string interstitialAdUnitId = "ca-app-pub-3940256099942544/4411468910";
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";

#else

    private string bannerAdUnitId = "unused";
    private string interstitialAdUnitId = "unused";
    private string rewardedAdUnitId = "unused";

#endif

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {

        MobileAds.Initialize((InitializationStatus status) =>
        {
            LoadBannerAd();
            LoadInterstitialAd();
            LoadRewardedAd();
        });
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuSceneName)
        {
            // Force reload banner every time menu opens
            LoadBannerAd();
        }
        else
        {
            HideBannerAd();
        }
    }

    // =====================================================
    // BANNER
    // =====================================================

    public void LoadBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
            bannerView = null;
        }

        bannerView = new BannerView(
            bannerAdUnitId,
            AdSize.Banner,
            AdPosition.Bottom
        );

        bannerView.OnBannerAdLoaded += () =>
        {
            if (SceneManager.GetActiveScene().name == mainMenuSceneName)
            {
                bannerView.Show();
            }
        };

        bannerView.OnBannerAdLoadFailed += (LoadAdError error) =>
        {
            Debug.LogError("[ADMOB] BANNER FAILED: " + error);
        };

        AdRequest request = new AdRequest();

        bannerView.LoadAd(request);
    }

    public void ShowBannerAd()
    {
        if (bannerView == null)
        {
            return;
        }

        bannerView.Show();
    }

    public void HideBannerAd()
    {
        if (bannerView == null)
        {
            return;
        }

        bannerView.Hide();
    }

    // =====================================================
    // INTERSTITIAL
    // =====================================================

    public void LoadInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
            interstitialAd = null;
        }

        AdRequest request = new AdRequest();

        InterstitialAd.Load(
            interstitialAdUnitId,
            request,
            (InterstitialAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    return;
                }

                interstitialAd = ad;
            });
    }

    public void ShowInterstitialAd(string sceneName)
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                SceneManager.LoadScene(sceneName);

                LoadInterstitialAd();
            };

            interstitialAd.Show();

        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    // =====================================================
    // REWARDED
    // =====================================================

    public void LoadRewardedAd()
    {
        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        AdRequest request = new AdRequest();

        RewardedAd.Load(
            rewardedAdUnitId,
            request,
            (RewardedAd ad, LoadAdError error) =>
            {
                if (error != null || ad == null)
                {
                    return;
                }

                rewardedAd = ad;

            });
    }

    public void ShowRewardedAd(Action rewardAction)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                LoadRewardedAd();
            };

            rewardedAd.Show((Reward reward) =>
            {
                rewardAction?.Invoke();
            });
        }
        else
        {
            Debug.LogWarning("[ADMOB] Rewarded Not Ready");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        bannerView?.Destroy();
        interstitialAd?.Destroy();
        rewardedAd?.Destroy();
    }
}