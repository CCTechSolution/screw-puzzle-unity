using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class Unity_Ads_Manager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    public static Unity_Ads_Manager Instance;
    public bool Test_Ads;
    public bool Skip_Ads;
    public bool ForeGroundedAD;
    bool Test;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);

#if UNITY_IOS
            _Game = _IOS;
#elif UNITY_ANDROID
        _Game = _Android;
#elif UNITY_EDITOR
            _Game = _Android;
#endif

        InitializeAds();
        // _Ban_ID();
        _Int_ID();
        _Rew_ID();
    }

    void OnEnable()
    {
        Invoke(nameof(Btn_Int_Load), 5f);
        Invoke(nameof(Btn_Rew_Load), 5f);
    }
    void Start()
    {

    }
    void OnDestroy()
    {

    }

    #region Initialize
    string _Game;
    string _Android = "5860187";
    string _IOS = "5860186";

    void InitializeAds()
    {
        // Initialize
        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(_Game, Test, this);
        }
    }
    public void OnInitializationComplete()
    {
        // Complete
        Debug.Log("Unity Ads initialization complete");
    }
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        // Fail
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
    #endregion

    //    #region Banner
    //    [Header("----- Banner -----")]
    //    // public BannerPosition _bannerPosition = BannerPosition.TOP_CENTER;
    //    public string _andBID = "Banner_Android";
    //    public string _iosBID = "Banner_iOS";
    //    string _banID;

    //    void _Ban_ID()
    //    {
    //#if UNITY_IOS
    //        _banID = _iosBID;
    //#elif UNITY_ANDROID
    //        _banID = _andBID;
    //#elif UNITY_EDITOR
    //        _banID = _andBID;
    //#endif
    //        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
    //        LoadBanner();
    //    }
    //    public void Btn_Banner_Show()
    //    {
    //        ShowBannerAd();
    //    }
    //    public void Btn_Banner_Load()
    //    {
    //        LoadBanner();
    //    }
    //    void LoadBanner()
    //    {
    //        // Load
    //        BannerLoadOptions options = new BannerLoadOptions
    //        {
    //            loadCallback = OnBannerLoaded,
    //            errorCallback = OnBannerError
    //        };
    //        Advertisement.Banner.Load(_banID, options);
    //    }
    //    void OnBannerLoaded()
    //    {
    //        // Loaded
    //        Debug.Log("Banner loaded");
    //        ShowBannerAd();
    //        // HideBannerAd();
    //    }
    //    void OnBannerError(string message)
    //    {
    //        // Error
    //        Debug.Log($"Banner Error: {message}");
    //    }
    //    void ShowBannerAd()
    //    {
    //        // Show
    //        BannerOptions options = new BannerOptions
    //        {
    //            clickCallback = OnBannerClicked,
    //            hideCallback = OnBannerHidden,
    //            showCallback = OnBannerShown
    //        };
    //        Advertisement.Banner.Show(_banID, options);
    //    }
    //    void HideBannerAd()
    //    {
    //        // Hidden
    //        Advertisement.Banner.Hide();
    //    }
    //    void OnBannerClicked()
    //    {
    //        // Clicked
    //    }
    //    void OnBannerShown()
    //    {
    //        // Shown
    //    }
    //    void OnBannerHidden()
    //    {
    //        // Hidden
    //    }
    //    #endregion

    #region Interstitial
    [Header("----- Interstitial -----")]
    [SerializeField] string _andIID = "Interstitial_Android";
    [SerializeField] string _iosIID = "Interstitial_iOS";
    string _intID;

    void _Int_ID()
    {
#if UNITY_IOS
        _intID = _iosIID;
#elif UNITY_ANDROID
        _intID = _andIID;
#elif UNITY_EDITOR
        _intID = _andIID;
#endif

        Btn_Int_Load();
    }
    public void Btn_Int_Load()
    {
        LoadInt();
    }
    public void Btn_Int_Show()
    {
        if (Skip_Ads == true)
        {
            // No ADs
        }
        else
        {
            Btn_On();
            ShowInt();
        }
    }
    void LoadInt()
    {
        // Load Call
        Debug.Log("Loading Ad: " + _intID);
        Advertisement.Load(_intID, this);
    }
    void ShowInt()
    {
        FirebaseInit.instance.LogEvent("Unity Ads: Interstitial: Show");

        // Show Call
        Debug.Log("Showing Ad: " + _intID);
        Advertisement.Show(_intID, this);
        Invoke(nameof(Btn_Off), 0.369f);
        Invoke(nameof(Btn_Int_Load), 1f);
    }
    #endregion

    #region Reward
    [Header("----- Rewarded -----")]
    [SerializeField] string _andRID = "Rewarded_Android";
    //[SerializeField] string _iosRID = "Rewarded_iOS";
    string _rewID;
    bool _rad;
    void _Rew_ID()
    {
#if UNITY_IOS
        _rewID = _iosRID;
#elif UNITY_ANDROID
        _rewID = _andRID;
#elif UNITY_EDITOR
        _rewID = _andRID;
#endif

        Btn_Rew_Load();
    }
    public void Btn_Rew_Load()
    {
        _rad = true;
        LoadRew();
    }
    public void Btn_Rew_Show()
    {
        if (Skip_Ads == true)
        {
            // No ADs
        }
        else
        {
            Btn_On();
            _rad = true;
            ShowRew();
        }
    }
    void LoadRew()
    {
        // Load Call
        Debug.Log("Loading Ad: " + _rewID);
        Advertisement.Load(_rewID, this);
    }
    void ShowRew()
    {
        FirebaseInit.instance.LogEvent("Unity Ads: Rewarded: Show");

        // Show Call
        Debug.Log("Showing Ad: " + _rewID);
        Advertisement.Show(_rewID, this);
        Invoke(nameof(Btn_Off), 0.369f);
        Invoke(nameof(Btn_Rew_Load), 1f);
    }
    #endregion

    #region Call Back
    public void OnUnityAdsAdLoaded(string _ID)
    {
        // Loaded 
        Debug.Log("Loading Ad: " + _rewID);
        if (_ID.Equals(_rewID))
        {
            // ShowRew();
        }
    }
    public void OnUnityAdsFailedToLoad(string _ID, UnityAdsLoadError error, string message)
    {
        // Fail Load
        Debug.Log($"Error loading Ad Unit: {_ID}: {error.ToString()} - {message}");
    }
    public void OnUnityAdsShowFailure(string _ID, UnityAdsShowError error, string message)
    {
        // Fail Show
        Debug.Log($"Error showing Ad Unit {_ID}: {error.ToString()} - {message}");
    }
    public void OnUnityAdsShowStart(string _ID)
    {
        // Start
    }
    public void OnUnityAdsShowClick(string _ID)
    {
        // Click
    }
    public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(_rewID) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED) && _rad == true)
        {
            // Complete
            Debug.Log("Unity Ads Rewarded Ad Completed");
        }
    }
    #endregion

    #region ForeGroundedAD
    void Btn_On()
    {
       AdmobAdsManager.ForeGroundedAD = true;
    }
    void Btn_Off()
    {
      AdmobAdsManager.ForeGroundedAD = false;
    }
    #endregion
}