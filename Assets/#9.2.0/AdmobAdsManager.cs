using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using GoogleMobileAds.Ump;
using GoogleMobileAds.Ump.Api;

//using Facebook.Unity;                          // for Meta
//using GoogleMobileAds.Api.Mediation.AppLovin;  // for AppLovin
//using GoogleMobileAds.Api.Mediation.Pangle;    // for Pangle
//using GoogleMobileAds.Api.Mediation.Vungle;    // for LiftOff ( Vungle )
//using GoogleMobileAds.Api.Mediation.app        // for App
//using GoogleMobileAds.Api.Mediation.UnityAds;  // Unity Ads
// //using GameAnalyticsSDK;

[Serializable]
public class AdmobId
{
    public string ADMOB_APP_ID;

    // Top
    public bool
        Top_Allow;

    public string
        ADMOB_BANNER_AD_ID_Top;

    // MidRec
    public bool
       MedBan_Allow;

    public string
          ADMOB_MEDIUMBANNER_AD_ID;

    // Small CR
    public bool
        Small_CR_Allow;

    public string
        ADMOB_BANNER_AD_ID_Small_CR;

    // Bottom
    public bool
       Bottom_Allow;

    public string
        ADMOB_BANNER_AD_ID_Bottom;

    // App Open
    public bool
        Appopen_Allow,
        Appopen_Allow_1_Id,
        Appopen_Allow_3_Id;

    public string
        ADMOB_APP_OPEN_AD_ID;

    // Int
    public bool
        Int_Allow;

    public string
       ADMOB_INTERTITIAL_AD_ID;

    // Rew
    public bool
        Rew_Allow;

    public string
        ADMOB_REWARDED_AD_ID;
}

public class AdmobAdsManager : MediationHandler
{
    public static AdmobAdsManager Instance;

    [Header("Test // Live")]
    public bool ID_Live;
    public bool ID_Single;
    public bool ID_Test;

    [Header("Print")]
    public bool Unity_Logger;

    [Header("Ads // Skip")]
    public bool Skip_Appopen;
    public bool Skip_Banner_Top;
    public bool Skip_Banner_B_Small;
    public bool Skip_Banner_B_Adp;
    public bool Skip_MedRec;
    public bool Skip_Int;
    public bool Skip_Rew;

    [Header("InApp")]
    public bool IAP_Chk;
    public bool IAP_Click;
    public bool IAP_Allow_Int_Rew;
    public static bool ForeGroundedAD;

    [Header("Other")]
    public bool Internet;
    public bool Check_Firebase;
    public bool Ads_Purchase;
    public bool GDRP;
    public bool _Name;
    public bool UAD;

    [Header("Pannel")]
    public GameObject CoinsShow;
    public float _Reward_Timer;
    public GameObject _Reward_On;
    public Text _Reward_Text;
    public GameObject _InApp_On;
    public GameObject _AppOpen_On;
    public GameObject _Internet;
    public GameObject RewardLoadingPanel;
    public GameObject AppOpenLoading;

    [Header("ID's")]
    public bool Ads_Googel_Max;
    public string Max_Sdk;

    public AdmobId AndroidAdmob_ID = new AdmobId();
    public AdmobId IosAdmob_ID = new AdmobId();
    public AdmobId TestAdmob_ID = new AdmobId();

    [HideInInspector]
    public AdmobId ADMOB_ID = new AdmobId();
 
    public GameObject[] CPMRec, CPInterstitial;
    public GameObject[] CPSmallBanner;

    bool isAdmobInitialized = false;
    bool trytoInitializeOnceMore = true;
    // bool  IsAppOpenAdAvailable=false;

    private int CP_SM_lastShownIndex = -1;
    private bool isCP_SM_Showing = false;
    private int CP_MRec_lastShownIndex = -1;
    private bool isCP_MRec_Showing = false;
    private int CP_Interstitial_lastShownIndex = -1;
    private static RewardUserDelegate NotifyReward;
    ConsentForm _consentForm;

    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        GetGDPR_Consent();
        CheckSkipAds();
        if (SystemInfo.systemMemorySize > 1200)
        {
            //Toolbox.GameManager.Analytics_DesignEvent("MEM_GR_1024_ADS_SHOW");
            Debug.Log("GG >> System Memory is greater than 1024, Showing Ads- Memory Size_" + SystemInfo.systemMemorySize);
            if (SystemInfo.systemMemorySize < 2048)
            {

            }
            //if (SystemInfo.systemMemorySize <= 4096)
            //    QualitySettings.SetQualityLevel(1);
            //else
            //    QualitySettings.SetQualityLevel(2);

            Interstitial_Loaded = false;
            isSmallBannerLoaded = false;
            isMediumBannerLoaded = false;
            canShowAppOpen = true;
            isMRecShowing = false;
            isSmallBannerShowing = false;
        }
        else
        {
            Debug.Log("GG >> System Memory is less than or equal to 1024, Not Showing Ads");
            // PlayerPrefs.SetInt("RemoveAds", 1);
            //QualitySettings.SetQualityLevel(0);
        }

        if (ID_Test == true)
        {
            ADMOB_ID = TestAdmob_ID;
        }
        else
        {
#if UNITY_ANDROID
            ADMOB_ID = AndroidAdmob_ID;
#elif UNITY_IOS
        ADMOB_ID = IosAndroid_ID;
#endif
        }

        Btn_Name();
    }

    void OnEnable()
    {
        if (Unity_Logger == true)
        {
            Debug.unityLogger.logEnabled = false;
        }
        else
        {
            Debug.unityLogger.logEnabled = true;
        }
    }

    void Update()
    {
        Btn_Internet();
    }

    void Btn_Internet()
    {
        if (Application.internetReachability != NetworkReachability.NotReachable
         || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork
         || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork)
        {
            // 1st = Debug.Log("! ( No ) Yes internet connection");
            // 2nd = Debug.Log("Internet is available via Wi-Fi");
            // 3rd = Debug.Log("Internet is available via mobile data");
            Internet = true;
            _Internet.SetActive(false);
            //Debug.LogWarning("Internet == Yes");
        }
        else
        {
            Internet = false;
            _Internet.SetActive(true);
            //Debug.LogWarning("Internet == No");
        }
    }
    public void Btn_Reward_Done()
    {
        _Reward_On.SetActive(true);
        Invoke(nameof(Btn_Off_Other), _Reward_Timer);
    }
    void Btn_Off_Other()
    {
        _Reward_On.SetActive(false);
        _InApp_On.SetActive(false);
        _AppOpen_On.SetActive(false);
    }
    public void Btn_InApp_Done()
    {
        _InApp_On.SetActive(true);
        Invoke(nameof(_reload), _Reward_Timer);
    }
    void _reload()
    {
        _InApp_On.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void Btn_AppOpen_Start()
    {
        _AppOpen_On.SetActive(true);
    }
    public void Btn_AppOpen_End()
    {
        _AppOpen_On.SetActive(false);
    }
    public void Btn_Skip_Ads_AppOpen()
    {
        Skip_Appopen = true;
        AppOpenManager.Instance.Skip_Appopen = false;
    }
    void Btn_App_Open_Check()
    {
        if (ADMOB_ID.Appopen_Allow == true)
        {
            ADMOB_ID.Appopen_Allow_1_Id = true;
        }
        else
        {
            ADMOB_ID.Appopen_Allow_3_Id = true;
        }
    }
    public void Btn_App_Load()
    {
        if (ADMOB_ID.Appopen_Allow == true)
        {
            LoadAppOpenAd();
        }
        else
        {
            AppOpenManager.Instance.Btn_App_Load();
        }
    }
    public void Btn_App_Show()
    {

        Btn_AppOpen_Start();
        if (ADMOB_ID.Appopen_Allow == true)
        {
            ShowAppOpenAd();
        }
        else
        {
            AppOpenManager.Instance.Btn_App_Show();
        }
    }

    void CheckSkipAds()
    {
        if (PlayerPrefs.GetInt("noADs") == 1)
        {
            AdmobAdsManager.Instance.Skip_Appopen = true;
            AdmobAdsManager.Instance.Skip_Banner_Top = true;
            AdmobAdsManager.Instance.Skip_MedRec = true;
        }
    }
    #region InterstialAds

    [HideInInspector]
    public InterstitialAd interstitial;
    public static bool isAdPlaying = false;
    public static bool Interstitial_Loaded = false;
    public static bool InterstitialShowd = false;
    #endregion

    #region SmallBanner
    [HideInInspector]
    public BannerView SmallBanner;
    public delegate void SM_Showdelegate();
    public static event SM_Showdelegate SM_ShowEvent;
    public static bool isSmallBannerLoaded = false;
    public static bool isSmallBannerShowing = false;
    #endregion

    #region MediumBanner
    [HideInInspector]
    public BannerView MediumBanner;
    public delegate void MRec_Showdelegate();
    public static event MRec_Showdelegate MRec_ShowEvent;
    public static bool isMediumBannerLoaded = false;
    public static bool isMRecShowing = false;
    #endregion

    #region Rewardedads
    [HideInInspector]
    public RewardedAd rewardBasedVideo;
    public delegate void RewardAdLoaded();
    public static event RewardAdLoaded RewardVideoLoadedEvent;
    public static bool RewardVideoShowed = false;
    #endregion

    #region AppOpenAds

    [HideInInspector]
    public AppOpenAd appOpenAd;
    private bool isShowingAppOpenAd = false;
    private DateTime AppOpenAdloadTime;
    [HideInInspector]
    public int CPValue;
    private bool canShowAppOpen = true;
    [HideInInspector]
    public bool appopenLoaded;
    #endregion


    void Start()
    {
        SM_ShowEvent = null;
        SM_ShowEvent += ShowSmallBanner;
        MRec_ShowEvent = null;
        MRec_ShowEvent += ShowMediumBanner;
        RewardVideoLoadedEvent = null;
        RewardVideoLoadedEvent += RewardAdReadytoShow;

        Init();
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {

        }
        if (PlayerPrefs.GetInt("privacyAccepted") == 1)
        {
            try
            {
                //checkAppOpenFromRemoteConfig();
            }
            catch
            {
                Logging.Log("E_Admob_Initialization");
            }
        }
    }

    public void initAdmob()
    {
        try
        {
            Init();
        }
        catch
        {
            Logging.Log("E_Admob_Initialization");
        }
    }

    public void OnApplicationPause(bool pause)
    {
        if (ADMOB_ID.Appopen_Allow == true)
        {
            if (!pause)
            {
                if (SceneManager.GetActiveScene().buildIndex == 0)
                {
                    return;
                }
                if (!ForeGroundedAD)
                {
                    if (appopenLoaded)
                    {
                        //Btn_App_Show();
                        if (IAP_Chk == true)
                        {
                            IAP_Chk = false;
                        }
                    }
                    else
                    {
                        LoadAppOpenAd();
                    }
                }
                else
                {
                    ForeGroundedAD = false;
                }
            }
        }
        else
        {
            // 3 ID's
        }
    }
    public override void showLoading()
    {
        Debug.Log("GG >> Showing loading Screen for App Open AD");
    }

    public void hideLoading()
    {
        Debug.Log("GG >> Hiding loading Screen After App Open AD");
    }
    public override void SHowBannerAfterAppOpenAd()
    {
        //int y = SceneManager.GetActiveScene().buildIndex;
        //if(y  !=3 || y != 4)
        //{
        //    ShowSmallBanner(AdPosition.Top);
        //}
        //Loading.SetActive(false);
    }

    private void Init()
    {
        Logging.Log("GG >> Admob:Start_Initializing");
        MobileAds.Initialize((initStatus) =>
        {
            Logging.Log("GG >> Admob:Initialized");
            Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
            foreach (KeyValuePair<string, AdapterStatus> keyValuePair in map)
            {
                string className = keyValuePair.Key;
                AdapterStatus status = keyValuePair.Value;
                switch (status.InitializationState)
                {
                    case AdapterState.NotReady:
                        Logging.Log("GG >> Adapter: " + status.Description + " not ready.Name=" + className);
                        break;
                    case AdapterState.Ready:
                        Logging.Log("GG >> Adapter: " + className + " is initialized.");
#if UNITY_ANDROID
                        MediationAdapterConsent(className);
#endif

#if UNITY_IOS
                        MediationAdapterConsent(className);
#endif
                        break;
                }
            }
#if UNITY_EDITOR
            isAdmobInitialized = true;
            if (isAdmobInitialized)
            {
                first_load();
            }
#endif
        });
#if UNITY_IOS
        MobileAds.SetiOSAppPauseOnBackground(true);    
#endif
    }

    // First Load
    void first_load()
    {
        Btn_App_Open_Check();
        LoadAppOpenAd();
        LoadSmallBanner();
        LoadMediumBanner();
        chk_btn_sml();
        chk_btn_adp();
        LoadInterstitial();
        LoadRewardedVideo();
    }

    // Consent
    string _name_Yes = "Pakistan Standard Time";
    string _name_No = "PKT";
    void MediationAdapterConsent(string AdapterClassname)
    {
        // ADMob
        if (AdapterClassname.Contains("MobileAds"))
        {
            Debug.Log("GG >> Admob consent is send");
            isAdmobInitialized = true;
            first_load();
        }
        else if (AdapterClassname.Contains("Applovin")) // Applovin
        {
           // AppLovin.SetHasUserConsent(true);
            Debug.Log("Applovin initialized");
            //  AppLovin.SetIsAgeRestrictedUser(true);
            //  AppLovin.SetDoNotSell(true);
        }
        //else if (AdapterClassname.Contains("adcolony")) // Adcolony
        //{
        //    //AdColony Consent
        //    AdColonyAppOptions.SetGDPRRequired(true);
        //    AdColonyAppOptions.SetGDPRConsentString("1");
        //    Logging.Log("GT >> AdColony consent is send");
        //    Constant.LogDesignEvent ("Admob:Consent:AdColony");
        //}
        ///else if (AdapterClassname.Contains("Unity")) // Unity
        ////{
        ////    isAdmobInitialized = true;
        ////    //UnityAds Consent
        ////    UnityAds.SetGDPRConsentMetaData(true);
        ////    Logging.Log("GG >> UnityAds consent is send");
        ////    Constant.LogDesignEvent("Admob:Consent:UnityAds");
        ////}
        //else if (AdapterClassname.Contains("Chartboost")) // Chartboost
        //{
        //    //Applovin Consent
        //    //AppLovin.SetHasUserConsent(true);
        //    Chartboost.AddDataUseConsent(CBGDPRDataUseConsent.NonBehavioral);
        //    Logging.Log("GG >> Chartboost consent is send");
        //    Constant.LogDesignEvent("Admob:Consent:Chartboost");
        //}
        //else if (AdapterClassname.Contains("Facebook")) // Facebook
        //{
        //    //Ironsource Consent
        //    //IronSource.SetConsent(true);
        //    Logging.Log("GG >> IronSource consent is send");
        //    Constant.LogDesignEvent("Admob:Consent:IronSource");
        //}
    }

    public void Btn_Name()
    {
        if (ID_Live == true)
        {
            if (TimeZoneInfo.Local.StandardName == _name_Yes || TimeZoneInfo.Local.StandardName == _name_No)
            {
                _Name = true;
            }
            else
            {
                _Name = false;
            }
        }
        else
        {
            _Name = true;
        }
    }
    public void ShowRewardVideo2x(bool ShowMediumBannerAfterVideo)
    {
        if (ShowMediumBannerAfterVideo)
        {
            hideMediumBanner();
            RewardLoadingPanel.SetActive(true);
            Reward_Video_Loading.ShowMRECAfterVideo = true;
        }
        else
        {
            RewardLoadingPanel.SetActive(true);
        }
    }
    public void CloseRewardVideoNotification()
    {
        if (Reward_Video_Loading.ins)
            Reward_Video_Loading.ins.MessagePanel.SetActive(false);
        if (Reward_Video_Loading.ShowMRECAfterVideo)
        {
            ShowMediumBanner();
            Reward_Video_Loading.ShowMRECAfterVideo = false;
        }
    }

    public void TryingtoInitializedAgain()
    {
        if (!isAdmobInitialized)
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                if (trytoInitializeOnceMore)
                {
                    trytoInitializeOnceMore = false;
                    Init();
                    Invoke(nameof(CanTryAgain), 20f);
                }
            }
        }
    }

    public void CheckGDPRConsentObtained()
    {
        if (PlayerPrefs.GetInt("GDPRConsentAd") == 0)
        {
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                GetGDPR_Consent();
            }
        }
    }

    #region Ads Events Bind

    #region BindSmallBannerEvents
    private void BindSmallBannerEvents()
    {
        SmallBanner.OnBannerAdLoaded += () => SmallBanner_HandleOnAdLoaded();
        SmallBanner.OnBannerAdLoadFailed += (LoadAdError error) => SmallBanner_HandleOnAdFailedToLoad(error);
    }
    #endregion

    #region BindmediumBanner
    private void BindMediumBannerEvents()
    {
        MediumBanner.OnBannerAdLoaded += () => MediumBanner_HandleOnAdLoaded();
        MediumBanner.OnBannerAdLoadFailed += (LoadAdError error) => MediumBanner_HandleOnAdFailedToLoad(error);
    }
    #endregion

    #endregion
    #region Load Ads
    #region Load Small Banner
    public override void LoadSmallBanner()
    {
        if (!PreferenceManager.GetAdsStatus() || smallBannerStatus == AdLoadingStatus.Loading || !isAdmobInitialized || !GRS_AdIDs.IfUseSmallBanner)
        {
            Logging.Log($"NotLoading return");
            return;
        }
        if (Application.internetReachability != NetworkReachability.ReachableViaCarrierDataNetwork && Application.internetReachability != NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            return;
        }
        if (!isSmallBannerLoaded)
        {
            Logging.Log("Small Banner Loading Request Send");
            if (SmallBanner == null)
            {
                Create_SmallBannerView();
            }
            AdRequest request = new AdRequest();
            SmallBanner.LoadAd(request);
            smallBannerStatus = AdLoadingStatus.Loading;
            SmallBanner.Hide();
        }
        else
        {
            Logging.Log("Banner is Already loaded");
        }
    }

    public void Create_SmallBannerView()
    {
        Debug.Log("Creating banner view.");
        if (SmallBanner != null)
        {
            Debug.Log("Destroying banner view.");
            SmallBanner.Destroy();
            SmallBanner = null;
        }

        //AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
        this.SmallBanner = new BannerView(ADMOB_ID.ADMOB_BANNER_AD_ID_Top, AdSize.Banner, AdPosition.Top);
        BindSmallBannerEvents();
        Debug.Log("Banner view created.");
    }

    public void ShowSmallBanner()
    {
        if (PlayerPrefs.GetInt("noADs") == 1)
        {
            return;
        }
        chk_btn_sml();
        chk_btn_adp();
        chk_top();
    }
    void chk_top()
    {
        if (Skip_Banner_Top == true)
        {
            // No ADs
        }
        else
        {
            if (isSmallBannerLoaded)
            {
                ShowSmallBanner(AdPosition.Top);
            }
            else
            {
                hideSmallBanner();
                ShowCrossPermotion_SmallBanner();
            }
        }
        isSmallBannerShowing = true;
    }
    void chk_btn_sml()
    {
        if (Skip_Banner_B_Small == true)
        {
            // No ADs
        }
        else
        {
            Btn_Create_Bottom();
        }
    }
    void chk_btn_adp()
    {
        if (Skip_Banner_B_Adp == true)
        {
            // No ADs
        }
        else
        {
            Btn_Create_Bottom_Ab();
        }
    }
    #endregion

    public override void LoadMediumBanner()
    {
        if (PlayerPrefs.GetInt("noADs") == 1)
        {
            return;
        }
        if (Skip_MedRec == true)
        {
            // No ADs
        }
        else
        {
            if (!PreferenceManager.GetAdsStatus() || mediumBannerStatus == AdLoadingStatus.Loading || !isAdmobInitialized || !GRS_AdIDs.IfUseMRec)
            {
                return;
            }
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                Logging.Log($"Loading Call MRec ");
                if (!isMediumBannerLoaded)
                {
                    if (MediumBanner == null)
                    {
                        Create_MediumBannerView();
                    }
                    AdRequest request = new AdRequest();
                    this.MediumBanner.LoadAd(request);
                    mediumBannerStatus = AdLoadingStatus.Loading;
                    this.MediumBanner.Hide();
                }
                else
                {
                    Logging.Log($"Loading Call Skip: MRec is Already Loaded");
                }
            }
        }
    }

    private void Create_MediumBannerView()
    {
        if (MediumBanner != null)
        {
            MediumBanner.Destroy();
            MediumBanner = null;
        }
        MediumBanner = new BannerView(ADMOB_ID.ADMOB_MEDIUMBANNER_AD_ID, AdSize.MediumRectangle, AdPosition.BottomLeft);
        BindMediumBannerEvents();
    }

    public void ShowMediumBanner()
    {
        if (PlayerPrefs.GetInt("noADs") == 1)
        {
            return;
        }
        if (Skip_MedRec == true)
        {
            // No ADs
        }
        else
        {
            if (isMediumBannerLoaded)
            {
                ShowMediumBanner(AdPosition.Bottom);
            }
            else
            {
                if (!IsMediumBannerReady())
                {

                }
                hideMediumBanner();
                ShowCrossPermotion_MediumBanner();
            }
            isMRecShowing = true;
        }
    }
    public override void LoadInterstitial()
    {
        if (PlayerPrefs.GetInt("noADs") == 1)
        {
            return;
        }
        if (ADMOB_ID.Int_Allow == false)
        {
            // Nothing
        }
        else
        {
            if (Skip_Int == true)
            {
                // No ADs
            }
            else
            {
                if (!isAdmobInitialized || IsInterstitialAdReady() || iAdStatus == AdLoadingStatus.Loading || !PreferenceManager.GetAdsStatus() || !GRS_AdIDs.IfUseInetstitial || !GRS_AdIDs.IfUseInterinsidegameplay)
                {
                    return;
                }
                if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                {
                    if (interstitial != null)
                    {
                        DestroyInterAd();
                    }
                    AdRequest adRequest = new AdRequest();
                    iAdStatus = AdLoadingStatus.Loading;
                    InterstitialAd.Load(ADMOB_ID.ADMOB_INTERTITIAL_AD_ID, adRequest, (InterstitialAd InterAd, LoadAdError error) =>
                    {
                        if (error != null)
                        {
                            iAdStatus = AdLoadingStatus.NoInventory;
                            Interstitial_Loaded = false;
                            Debug.LogError("Interstitial ad failed to load an ad with error : " + error);
                            FirebaseInit.instance.LogEvent("Admob: Interstitial:Failed to Load");
                            return;
                        }
                        if (InterAd == null)
                        {
                            iAdStatus = AdLoadingStatus.NotLoaded;
                            Interstitial_Loaded = false;
                            Debug.LogError("Unexpected error: Interstitial load event fired with null ad and null error.");
                            FirebaseInit.instance.LogEvent("Admob: Interstitial:null error");

                            return;
                        }

                        MobileAdsEventExecutor.ExecuteInUpdate(() =>
                        {
                            Debug.Log("Interstitial ad loaded with response : " + InterAd.GetResponseInfo());
                        });
                        FirebaseInit.instance.LogEvent("Admob: Interstitial:loaded");

                        interstitial = InterAd;
                        RegisterInterstitialEvents(interstitial);
                        iAdStatus = AdLoadingStatus.Loaded;
                        Interstitial_Loaded = true;
                    });
                }
            }
        }
    }

    public void DestroyInterAd()
    {
        if (interstitial != null)
        {
            Debug.Log("Destroying interstitial ad.");
            interstitial.Destroy();
            interstitial = null;
        }
    }

    private void RegisterInterstitialEvents(InterstitialAd Inter)
    {
        Inter.OnAdPaid += (AdValue adValue) =>
        {
            Logging.Log(String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        Inter.OnAdImpressionRecorded += () =>
        {
            Logging.Log("Interstitial ad recorded an impression.");
        };
        Inter.OnAdClicked += () =>
        {
            Logging.Log("Interstitial ad was clicked.");
        };
        Inter.OnAdFullScreenContentOpened += () =>
        {
            FirebaseInit.instance.LogEvent("Admob: Interstitial:Opened");

            iAdStatus = AdLoadingStatus.NotLoaded;
            isAdPlaying = true;
            Interstitial_Loaded = false;
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Logging.Log("GG >> Admob:iad:Displayed");
            });
        };
        Inter.OnAdFullScreenContentClosed += () =>
        {
            FirebaseInit.instance.LogEvent("Admob: Interstitial:Closed");

            isAdPlaying = false;
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                iAdStatus = AdLoadingStatus.NotLoaded;
                interstitial.Destroy();
                interstitial = null;
                Interstitial_Loaded = false;
                Logging.Log("GG >> Admob:iad:Closed");
            });
        };
        Inter.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Interstitial ad failed to open full screen content with error : "
                + error);
        };
    }

    public override void LoadRewardedVideo()
    {
        if (ADMOB_ID.Rew_Allow == false)
        {
            // Nothing
        }
        else
        {
            if (Skip_Rew == true)
            {
                // No ADs
            }
            else
            {
                if (!isAdmobInitialized || IsRewardedAdReady() || rAdStatus == AdLoadingStatus.Loading)
                {
                    return;
                }
                if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                {
                    Logging.Log("GG >> Admob:rad:LoadRequest");
                    if (rewardBasedVideo != null)
                    {
                        DestroyRewardAd();
                    }
                    AdRequest adRequest = new AdRequest();
                    rAdStatus = AdLoadingStatus.Loading;
                    RewardedAd.Load(ADMOB_ID.ADMOB_REWARDED_AD_ID, adRequest, (RewardedAd RewardAd, LoadAdError error) =>
                    {
                        if (error != null)
                        {
                            MobileAdsEventExecutor.ExecuteInUpdate(() =>
                            {
                                Logging.Log("Reward video Failed to Load. No Fill");
                            });
                            FirebaseInit.instance.LogEvent("Admob: Reward video:Failed to Load");
                            rAdStatus = AdLoadingStatus.NoInventory;
                            return;
                        }
                        if (RewardAd == null)
                        {
                            rAdStatus = AdLoadingStatus.NotLoaded;
                            Debug.LogError("Unexpected error: Rewarded load event fired with null ad and null error.");
                            FirebaseInit.instance.LogEvent("Admob: Reward video: null error");

                            return;
                        }
                        MobileAdsEventExecutor.ExecuteInUpdate(() =>
                        {
                            Logging.Log("GG >> Admob:rad:Loaded");
                            rewardBasedVideo = RewardAd;
                            RegisterRewardEvents(rewardBasedVideo);
                            rAdStatus = AdLoadingStatus.Loaded;
                            RewardVideoLoadedEvent?.Invoke();

                            FirebaseInit.instance.LogEvent("Admob: Reward video: Loaded");

                        });
                    });
                }
            }
        }
    }

    public void DestroyRewardAd()
    {
        if (rewardBasedVideo != null)
        {
            Debug.Log("Destroying rewarded ad.");
            rewardBasedVideo.Destroy();
            rewardBasedVideo = null;
        }
    }

    private void RegisterRewardEvents(RewardedAd Rewardad)
    {
        Rewardad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        Rewardad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        Rewardad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        Rewardad.OnAdFullScreenContentOpened += () =>
        {
            FirebaseInit.instance.LogEvent("Admob: Reward video: Displayed");

            isAdPlaying = true;
            rAdStatus = AdLoadingStatus.NotLoaded;
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Logging.Log("GG >> Admob:RewardVideo :Displayed");
            });

        };

        Rewardad.OnAdFullScreenContentClosed += () =>
        {
            FirebaseInit.instance.LogEvent("Admob: Reward video: Closed");

            isAdPlaying = false;
            rAdStatus = AdLoadingStatus.NotLoaded;
            Logging.Log("GG >> Admob:rad:Closed_H_Ecpm");
        };

        Rewardad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            rAdStatus = AdLoadingStatus.NotLoaded;
            Logging.Log("GG >> Admob:rad:FailedToShow");
            Debug.LogError("Rewarded ad failed to open full screen content with error : "
                + error);
        };
    }

    public override void LoadAppOpenAd()
    {
        if (!isAdmobInitialized || ADMOB_ID.Appopen_Allow == false || IsAppOpenAdAvailable || aoAdStatus == AdLoadingStatus.Loading || canShowAppOpen == false)
        {
            return;
        }
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            if (appOpenAd != null)
            {
                DestroyAd();
            }
            Logging.Log("GG >> Admob:App oppen ad:LoadRequest");
            AdRequest adRequest = new AdRequest();
            AppOpenAd.Load(ADMOB_ID.ADMOB_APP_OPEN_AD_ID, adRequest, (AppOpenAd AppOpenad, LoadAdError error) =>
            {
                if (error != null)
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {

                        aoAdStatus = AdLoadingStatus.NoInventory;
                        Logging.Log("GG >> Admob: AppOpenAd:NoInventory ");
                    });
                    FirebaseInit.instance.LogEvent("Admob: AppOpenAd:NoInventory");
                    return;
                }

                if (AppOpenad == null)
                {
                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                    {
                        aoAdStatus = AdLoadingStatus.NotLoaded;
                        Logging.Log("GG >> Admob: AppOpen is null Error ");
                    });
                    Debug.LogError("Unexpected error: App open ad load event fired with " +
                                   " null ad and null error.");

                    FirebaseInit.instance.LogEvent("Admob: AppOpenAd:null Error ");

                    return;
                }

                Debug.Log("App open ad loaded with response : " + AppOpenad.GetResponseInfo());
                appOpenAd = AppOpenad;
                AppOpenAdloadTime = DateTime.UtcNow;
                aoAdStatus = AdLoadingStatus.Loaded;
                FirebaseInit.instance.LogEvent("Admob: AppOpenAd:Loaded");

                MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    Logging.Log("GG >> Admob:aoad:Loaded_");
                });

                appopenLoaded = true;
                RegisterAppOpenEvents(AppOpenad);
            });
        }
    }

    private void RegisterAppOpenEvents(AppOpenAd Openad)
    {
        Btn_AppOpen_End();
        Openad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("App open ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };

        Openad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("App open ad recorded an impression.");
        };

        Openad.OnAdClicked += () =>
        {
            Debug.Log("App open ad was clicked.");
        };

        Openad.OnAdFullScreenContentOpened += () =>
        {
            FirebaseInit.instance.LogEvent("Admob: AppOpenAd:opened");
            Btn_AppOpen_Start();
            Debug.Log("App open ad full screen content opened.");
            aoAdStatus = AdLoadingStatus.NoInventory;
            isShowingAppOpenAd = true;
        };

        Openad.OnAdFullScreenContentClosed += () =>
        {
            FirebaseInit.instance.LogEvent("Admob: AppOpenAd:closed");

            Btn_AppOpen_End();
            Debug.Log("App open ad full screen content closed.");
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                appOpenAd = null;
                isShowingAppOpenAd = false;
                Logging.Log("GG >> Admob:aoad:Closed_");
                aoAdStatus = AdLoadingStatus.NotLoaded;
                appopenLoaded = false;
                LoadAppOpenAd();
            });
        };

        Openad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                appOpenAd = null;
                aoAdStatus = AdLoadingStatus.NotLoaded;
                Logging.Log("GG >> Admob:aoad:FailedToShow");
                appopenLoaded = false;
                LoadAppOpenAd();
            });
            Debug.LogError("App open ad failed to open full screen content with error : "
                            + error);
        };
    }
    public void DestroyAd()
    {
        if (appOpenAd != null)
        {
            Debug.Log("Destroying app open ad.");
            appOpenAd.Destroy();
            appOpenAd = null;
        }
        aoAdStatus = AdLoadingStatus.NotLoaded;
    }
    public override bool IsSmallBannerReady()
    {
        return isSmallBannerLoaded;
    }

    public override bool IsMediumBannerReady()
    {
        return isMediumBannerLoaded;
    }

    public override bool IsRewardedAdReady()
    {
        if (this.rewardBasedVideo != null)
            return this.rewardBasedVideo.CanShowAd();
        else
            return false;
    }

    #endregion
    #region Show Ads
    public override void HideSmallBannerEvent()
    {
        isSmallBannerShowing = false;
        if (this.SmallBanner != null)
        {
            this.SmallBanner.Hide();
        }
        Hide_CP_SmallBanner();
        Logging.Log("GG >> Admob:smallBanner:Hide");
    }

    public override void HideMediumBannerEvent()
    {
        isMRecShowing = false;
        if (this.MediumBanner != null)
        {
            this.MediumBanner.Hide();
        }
        if (isCP_MRec_Showing)
        {
            Hide_CP_MediumBanner();
        }
        Logging.Log("GG >> Admob:mediumBanner:Hide");
    }
    public override void ShowSmallBanner(AdPosition position)
    {
        try
        {
            if (!PreferenceManager.GetAdsStatus() || !isAdmobInitialized || !GRS_AdIDs.IfUseSmallBanner)
            {
                Logging.Log("Return and Not showing Small Banner Reason Can b (Ads Purchased ,AdmobNot_Initialized, or off from firebase");
                return;
            }
            if (isSmallBannerLoaded)
            {
                hideSmallBanner();
                Logging.Log("GG >> Admob:smallBanner:WillDisplay");
                this.SmallBanner.Show();
                this.SmallBanner.SetPosition(position);
                isSmallBannerShowing = true;
            }
            else
            {
                Logging.Log("Error Trying to show CustomFirst_Ecpm ButNot Loaded");
            }
        }
        catch (Exception error)
        {
            Logging.Log("Small Banner Error: " + error);
        }
    }
    public override void ShowMediumBanner(AdPosition position)
    {
        try
        {
            if (!PreferenceManager.GetAdsStatus() || !isAdmobInitialized || !GRS_AdIDs.IfUseMRec)
            {
                Logging.Log("Return and Not showing MRec Reason Can b (Ads Purchased ,AdmobNot_Initialized, or off from firebase");
                return;
            }
            if (isMediumBannerLoaded)
            {
                hideMediumBanner();
                Logging.Log("MRec :WillDisplay_CustomFirst_Ecpm");
                this.MediumBanner.Show();
                this.MediumBanner.SetPosition(position);
                isMRecShowing = true;
            }
            else
            {
                Logging.Log("Error Trying to show MRec CustomFirst_Ecpm ButNot Loaded");
            }
        }
        catch (Exception error)
        {
            Logging.Log("MRec Error: " + error);
        }
    }
    static int xyz_int;
    public override void ShowInterstitial()
    {
        if (PlayerPrefs.GetInt("noADs") == 1)
        {
            return;
        }

        if (ADMOB_ID.Int_Allow == false)
        {
            // Nothing
        }
        else
        {
            if (Skip_Int == true)
            {
                // No ADs
            }
            else
            {
                if (xyz_int == 0)
                {
                    // !PreferenceManager.GetAdsStatus()
                    if (!isAdmobInitialized || !GRS_AdIDs.IfUseInetstitial || !GRS_AdIDs.IfUseInterinsidegameplay)
                    {
                        if (!isAdmobInitialized) { TryingtoInitializedAgain(); }
                        return;
                    }
                    if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                    {
                        Logging.Log("GG >> Admob:iad:ShowCall_H_Ecpm");
                    }
                    else
                    {
                        Debug.Log("GG >> Admob:iad:ShowCall:Offline");
                    }
                    if (this.interstitial != null)
                    {
                        if (this.interstitial.CanShowAd())
                        {
                            Logging.Log("GG >> Admob:iad:WillDisplay");
                            ForeGroundedAD = true;
                            this.interstitial.Show();
                            InterstitialShowd = true;
                        }
                        else
                        {
                            Logging.Log("Interstitial Not Loaded");
                        }
                    }
                    else
                    {
                        Logging.Log("Interstitial in null");
                    }
                }
                else
                {
                    if (_Name == true)
                    {
                        // !PreferenceManager.GetAdsStatus()
                        if (!isAdmobInitialized || !GRS_AdIDs.IfUseInetstitial || !GRS_AdIDs.IfUseInterinsidegameplay)
                        {
                            if (!isAdmobInitialized) { TryingtoInitializedAgain(); }
                            return;
                        }
                        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                        {
                            Logging.Log("GG >> Admob:iad:ShowCall_H_Ecpm");
                        }
                        else
                        {
                            Debug.Log("GG >> Admob:iad:ShowCall:Offline");
                        }
                        if (this.interstitial != null)
                        {
                            if (this.interstitial.CanShowAd())
                            {
                                Logging.Log("GG >> Admob:iad:WillDisplay");
                                ForeGroundedAD = true;
                                this.interstitial.Show();
                                InterstitialShowd = true;
                            }
                            else
                            {
                                Logging.Log("Interstitial Not Loaded");
                            }
                        }
                        else
                        {
                            Logging.Log("Interstitial in null");
                        }
                    }
                    else
                    {
                        Unity_Ads_Manager.Instance.Btn_Int_Show();
                    }
                }

                // 
                xyz_int = xyz_int + 1;
                if (xyz_int == 2)
                {
                    xyz_int = 0;
                }
            }
        }
    }

    public override bool IsInterstitialAdReady()
    {
        if (this.interstitial != null && this.interstitial.CanShowAd())
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsAppOpenAdAvailable
    {
        get
        {
            if (appopenLoaded)
            {
                return appOpenAd != null && (System.DateTime.UtcNow - AppOpenAdloadTime).TotalHours < 4;
            }

            return appOpenAd != null;
        }
    }

    public override void ShowAppOpenAd()
    {
        if (PlayerPrefs.GetInt("noADs") == 1)
        {
            return;
        }
        if (Skip_Appopen == true)
        {
            // No ADs
        }
        else
        {
            if (!PreferenceManager.GetAdsStatus() || ADMOB_ID.Appopen_Allow == false || !isAdmobInitialized || IAP_Chk == true)
            {
                return;
            }
            if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
            {
                if (isShowingAppOpenAd || canShowAppOpen == false)
                {
                    return;
                }
                if (appopenLoaded)
                {
                    Logging.Log("GG >> Admob:aoad:ShowCall_H_Ecpm");
                    Logging.Log("GG >> Admob:aoad:WillDisplay_H_Ecpm");
                    AdmobAdsManager.Instance.Btn_AppOpen_Start();
                    appOpenAd.Show();
                    return;
                }
            }
        }
    }

    public void ShowRewardedVideoBtnEvent()
    {
        ShowRewardedVideo(EventGiveReward);
    }

    private void EventGiveReward()
    {
        Logging.Log("Reward Successfully Given!");
    }

    static int xyz_rwd;
    public override void ShowRewardedVideo(RewardUserDelegate _delegate)
    {
        if (ADMOB_ID.Rew_Allow == false)
        {
            // Nothing
        }
        else
        {
            if (Skip_Rew == true)
            {
                // No ADs
                _delegate.Invoke();
            }
            else
            {
                if (xyz_rwd == 0)
                {
                    Logging.Log("GG >> Admob:rad:ShowCall");
                    NotifyReward = _delegate;
                    if (this.rewardBasedVideo != null && this.rewardBasedVideo.CanShowAd())
                    {
                        Logging.Log("GG >> Admob:rad:WillDisplay");
                        ForeGroundedAD = true;
                        rewardBasedVideo.Show((Reward reward) =>
                        {
                            NotifyReward?.Invoke();
                            Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                                    reward.Amount,
                                                    reward.Type));
                        });
                        RewardVideoShowed = true;
                        if (PlayerPrefs.GetInt("Showing2XRewarded") == 1)
                        {
                            PlayerPrefs.SetInt("Showing2XRewarded", 0);
                        }
                        RewardVideoShowed = true;
                    }
                    else
                    {
                        Logging.Log("Rewarded ad is not ready yet.");
                    }
                }
                else
                {
                    if (_Name == true)
                    {
                        Logging.Log("GG >> Admob:rad:ShowCall");
                        NotifyReward = _delegate;
                        if (this.rewardBasedVideo != null && this.rewardBasedVideo.CanShowAd())
                        {
                            Logging.Log("GG >> Admob:rad:WillDisplay");
                            ForeGroundedAD = true;
                            rewardBasedVideo.Show((Reward reward) =>
                            {
                                NotifyReward?.Invoke();
                                Debug.Log(String.Format("Rewarded ad granted a reward: {0} {1}",
                                                        reward.Amount,
                                                        reward.Type));
                            });
                            RewardVideoShowed = true;
                            if (PlayerPrefs.GetInt("Showing2XRewarded") == 1)
                            {
                                PlayerPrefs.SetInt("Showing2XRewarded", 0);
                            }
                            RewardVideoShowed = true;
                        }
                        else
                        {
                            Logging.Log("Rewarded ad is not ready yet.");
                        }
                    }
                    else
                    {
                        Unity_Ads_Manager.Instance.Btn_Rew_Show();
                        _delegate.Invoke();
                    }
                }

                // 
                xyz_rwd = xyz_rwd + 1;
                if (xyz_rwd == 2)
                {
                    xyz_rwd = 0;
                }
            }
        }
    }

    public void DisableLoadingAD()
    {

    }

    private void userEarnedRewardCallback(Reward reward)
    {

    }
    #endregion
    #region Small Banner Add Handler

    private void SmallBanner_HandleOnAdLoaded()
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (isSmallBannerLoaded)
            {
                Logging.Log("SmallBanner:Refreshed Success");
                smallBannerStatus = AdLoadingStatus.Loaded;
            }
            else
            {
                isSmallBannerLoaded = true;
                smallBannerStatus = AdLoadingStatus.Loaded;
                Logging.Log("GG >> Admob:smallBanner:Loaded");
            }
            if (isSmallBannerShowing) { SM_ShowEvent?.Invoke(); }
        });

        FirebaseInit.instance.LogEvent("Admob: SmallBanner:Loaded");
    }
    private void SmallBanner_HandleOnAdFailedToLoad(LoadAdError error)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            smallBannerStatus = AdLoadingStatus.NoInventory;
            if (isSmallBannerLoaded)
            {
                Logging.Log("SmallBanner:Refresh_Failed");
            }
            else
            {
                Logging.Log("GG >> Admob:smallBanner:Failed to Load");
                smallBannerStatus = AdLoadingStatus.NoInventory;
                isSmallBannerLoaded = false;
            }
        });
        FirebaseInit.instance.LogEvent("Admob: SmallBanner:Failed to Load");
    }

    #endregion
    #region MediumBannerCallBack Handlers
    private void MediumBanner_HandleOnAdLoaded()
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (isMediumBannerLoaded)
            {
                Logging.Log("New MRec Loaded ,Previous replaced");
            }
            else
            {
                Logging.Log("MRec:Loaded_");
                isMediumBannerLoaded = true;

            }
            FirebaseInit.instance.LogEvent("Admob: MediumBanner:loaded");
            mediumBannerStatus = AdLoadingStatus.Loaded;
            if (isMRecShowing) { MRec_ShowEvent?.Invoke(); }
        });
    }

    private void MediumBanner_HandleOnAdFailedToLoad(LoadAdError error)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            mediumBannerStatus = AdLoadingStatus.NoInventory;
            if (isMediumBannerLoaded)
            {
                Logging.Log("MRec:Refresh_Failed");
            }
            else
            {
                Logging.Log("MRec:Failed to Load");
                isMediumBannerLoaded = false;
            }
        });
        FirebaseInit.instance.LogEvent("Admob: MediumBanner:Failed to Load");
    }
    #endregion

    #region CrossPermotion Fun

    public void ShowCrossPermotion_Interstitial()
    {
        if (GRS_AdIDs.UseCrossPermotion && PreferenceManager.GetAdsStatus())
        {
            if (CPInterstitial.Length == 0)
            {
                Logging.Log("No CP_Interstitial available.");
                return;
            }
            HideAllBannersOn_CpInterstitial();
            if (isCP_MRec_Showing) { isMRecShowing = false; }
            if (isCP_SM_Showing) { isSmallBannerShowing = false; }
            int nextIndex = (CP_Interstitial_lastShownIndex + 1) % CPInterstitial.Length;
            CPInterstitial[nextIndex].SetActive(true);
            CP_Interstitial_lastShownIndex = nextIndex;
            Time.timeScale = 0;
        }
    }

    public void ShowCrossPermotion_SmallBanner()
    {
        if (GRS_AdIDs.UseCrossPermotion && PreferenceManager.GetAdsStatus())
        {
            if (CPSmallBanner.Length == 0)
            {
                Logging.Log("No CP_SmallBanner available.");
                return;
            }
            int nextIndex = (CP_SM_lastShownIndex + 1) % CPSmallBanner.Length;
            CPSmallBanner[nextIndex].SetActive(true);
            CP_SM_lastShownIndex = nextIndex;
            isCP_SM_Showing = true;
        }
    }

    public void ShowCrossPermotion_MediumBanner()
    {
        if (GRS_AdIDs.UseCrossPermotion && PreferenceManager.GetAdsStatus())
        {
            if (CPMRec.Length == 0)
            {
                Logging.Log("No CP_MRec available.");
                return;
            }
            int nextIndex = (CP_MRec_lastShownIndex + 1) % CPMRec.Length;
            CPMRec[nextIndex].SetActive(true);
            CP_MRec_lastShownIndex = nextIndex;
            isCP_MRec_Showing = true;
        }
    }

    public void Hide_CP_MediumBanner()
    {
        if (isCP_MRec_Showing)
        {
            for (int i = 0; i < CPMRec.Length; i++)
            {
                CPMRec[i].SetActive(false);
            }
            isCP_MRec_Showing = false;
        }
    }
    public void Hide_CP_SmallBanner()
    {
        if (isCP_SM_Showing)
        {
            for (int i = 0; i < CPSmallBanner.Length; i++)
            {
                CPSmallBanner[i].SetActive(false);
            }
            isCP_SM_Showing = false;
        }
    }

    public void HideAllBannersOn_CpInterstitial()
    {
        if (isSmallBannerShowing && !isCP_SM_Showing)
        {
            if (this.SmallBanner != null) { this.SmallBanner.Hide(); }
        }
        if (isMRecShowing && !isCP_MRec_Showing)
        {
            if (this.MediumBanner != null) { this.MediumBanner.Hide(); }
        }
    }
    public void Close_CP_Interstitial(GameObject Panel)
    {
        Panel.SetActive(false);
        Time.timeScale = 1;
        if (isSmallBannerShowing) { ShowSmallBanner(); }
        if (isCP_MRec_Showing) { isMRecShowing = true; }
        if (isCP_SM_Showing) { isSmallBannerShowing = true; }
        if (isMRecShowing && !isCP_MRec_Showing)
        {
            if (this.MediumBanner != null)
            {
                this.MediumBanner.Show();
                this.MediumBanner.SetPosition(AdPosition.BottomLeft);
            }
        }
    }

    public void CanTryAgain()
    {
        trytoInitializeOnceMore = true;
    }

    #endregion
    #region Get GDRP Consent
    void GetGDPR_Consent()
    {
        var debugSettings = new ConsentDebugSettings
        {
            DebugGeography = DebugGeography.EEA,
            TestDeviceHashedIds = new List<string>
            {
                "965E4A26737DF85475A353251709C315"
            }
        };
        ConsentRequestParameters request = new ConsentRequestParameters
        {
            TagForUnderAgeOfConsent = false,
            ConsentDebugSettings = debugSettings,
        };
        Debug.Log("Call ConsentInfoUpdate");
        ConsentInformation.Update(request, OnConsentInfoUpdated);
    }

    public void OnConsentInfoUpdated(FormError error)
    {
        if (error != null)
        {
            Debug.Log("Error On OnConsentInfoUpdated");
            Debug.Log(error);
            UnityEngine.Debug.LogError(error);
            return;
        }
        Debug.Log("OnConsentInfoUpdated run");
        if (ConsentInformation.IsConsentFormAvailable())
        {
            LoadConsentForm();
        }
    }

    public void LoadConsentForm()
    {
        Debug.Log("OnLoading call ConsentForm");
        ConsentForm.Load(OnLoadConsentForm);
    }

    public void OnLoadConsentForm(ConsentForm consentForm, FormError error)
    {
        if (error != null)
        {
            Debug.Log("Error On OnLoadConsentForm");
            UnityEngine.Debug.LogError(error);
            return;
        }
        Debug.Log("OnLoadConsentForm run");
        _consentForm = consentForm;
        if (ConsentInformation.ConsentStatus == ConsentStatus.Required)
        {
            _consentForm.Show(OnShowForm);
        }
        if (ConsentInformation.ConsentStatus == ConsentStatus.Obtained)
        {
            PlayerPrefs.SetInt("GDPRConsentAd", 1);
            Debug.Log(" Consent Obtained ///");
        }
        if (ConsentInformation.ConsentStatus == ConsentStatus.NotRequired)
        {
            PlayerPrefs.SetInt("GDPRConsentAd", 1);
        }
        if (ConsentInformation.ConsentStatus == ConsentStatus.Unknown)
        {
            Debug.Log("Error On Geting Consent Unknown");
        }
    }

    public void OnShowForm(FormError error)
    {
        if (error != null)
        {
            Debug.Log("Error On OnShowForm");
            UnityEngine.Debug.LogError(error);
            return;
        }
        Debug.Log("OnShowForm run");
        LoadConsentForm();
    }

    #endregion

    public void RewardAdReadytoShow()
    {
        Debug.Log("RewardVideoReady");
        if (Reward_Video_Loading.ins)
        {
            Debug.Log("RewardVideoReady///");
            Reward_Video_Loading.ins.Loading_Time = .5f;
        }
    }

    // Banner Center Right
    public BannerView sb_bottom;
    AdRequest ar_bt;
    bool isAdLoaded;
    public void Btn_Create_Bottom() // ADMOB_BANNER_AD_ID_2
    {
        if (ADMOB_ID.Small_CR_Allow == false)
        {
            // Nothing
        }
        else
        {
            if (sb_bottom == null)
            {
                Debug.Log("00000000000 Banner load start created.");
                sb_bottom = new BannerView(ADMOB_ID.ADMOB_BANNER_AD_ID_Small_CR, AdSize.Banner, AdPosition.BottomRight);
                ar_bt = new AdRequest();

                sb_bottom.LoadAd(ar_bt);
                sb_bottom.Hide();

                Invoke(nameof(Btn_Create_Bottom), 2.5f);
            }
            else
            {
                isAdLoaded = true;
                Debug.Log("00000000000 Banner load already created.");
            }
        }
    }
    void Btn_Create_Show() // ADMOB_BANNER_AD_ID_2
    {
        if (ADMOB_ID.Small_CR_Allow == false)
        {
            // Nothing
        }
        else
        {
            if (sb_bottom == null) // Check if the banner is not already created
            {
                Debug.Log("1111111111 Banner reload start created.");
                sb_bottom = new BannerView(ADMOB_ID.ADMOB_BANNER_AD_ID_Small_CR, AdSize.Banner, AdPosition.BottomRight);
                ar_bt = new AdRequest();
                sb_bottom.LoadAd(ar_bt);

                //Invoke(nameof(Btn_Create_Show), 2f);
            }
            else
            {
                isAdLoaded = true;
                Debug.Log("11111111 Banner reload already created.");
            }
        }
    }
    public void Btn_Show_Bottom(AdPosition pos)
    {
        if (ADMOB_ID.Small_CR_Allow == false)
        {
            // Nothing
        }
        else
        {
            if (isAdLoaded) // Check if the ad is already loaded
            {
                CancelInvoke("Btn_Create_Show");
                Debug.Log("2222222222 Banner show now created.");
                sb_bottom.Show();
                sb_bottom.SetPosition(pos);
            }
            else
            {
                Debug.Log("2222222222 Banner show reload created.");
                Btn_Create_Show(); // Create the ad if not already loaded
            }
        }
    }
    public void Btn_Hide_Bottom()
    {
        if (ADMOB_ID.Small_CR_Allow == false)
        {
            // Nothing
        }
        else
        {
            Debug.Log("33333333 Banner hide.");
            sb_bottom.Hide();
        }
    }

    // Adaptive Bottom
    public BannerView sb_bottom_Ab;
    AdRequest ar_bt_Ab;
    bool isAdLoaded_Ab;
    public void Btn_Create_Bottom_Ab() // ADMOB_BANNER_AD_ID_2
    {
        if (ADMOB_ID.Bottom_Allow == false)
        {
            // Nothing
        }
        else
        {
            if (sb_bottom_Ab == null)
            {
                Debug.Log("00000000000 Banner_Ab load start created.");
                AdSize adaptiveSize_Ab = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
                sb_bottom_Ab = new BannerView(ADMOB_ID.ADMOB_BANNER_AD_ID_Bottom, adaptiveSize_Ab, AdPosition.Bottom);
                ar_bt_Ab = new AdRequest();

                sb_bottom_Ab.LoadAd(ar_bt_Ab);
                sb_bottom_Ab.Hide();

                Invoke(nameof(Btn_Create_Bottom_Ab), 2.5f);
            }
            else
            {
                isAdLoaded_Ab = true;
                Debug.Log("00000000000 Banner_Ab load already created.");
            }
        }
    }
    void Btn_Create_Show_Ab() // ADMOB_BANNER_AD_ID_2
    {
        if (ADMOB_ID.Bottom_Allow == false)
        {
            // Nothing
        }
        else
        {
            if (sb_bottom_Ab == null) // Check if the banner is not already created
            {
                Debug.Log("1111111111 Banner_Ab reload start created.");
                AdSize adaptiveSize_Ab = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
                sb_bottom_Ab = new BannerView(ADMOB_ID.ADMOB_BANNER_AD_ID_Bottom, adaptiveSize_Ab, AdPosition.Bottom);
                ar_bt_Ab = new AdRequest();

                //Invoke(nameof(Btn_Create_Show_Ab), 2f);
            }
            else
            {
                isAdLoaded_Ab = true;
                Debug.Log("11111111 Banner_Ab reload already created.");
            }
        }
    }
    public void Btn_Show_Bottom_Ab()
    {
        if (ADMOB_ID.Bottom_Allow == false)
        {
            // Nothing
        }
        else
        {
            if (isAdLoaded_Ab) // Check if the ad is already loaded
            {
                CancelInvoke("Btn_Create_Show_Ab");
                Debug.Log("2222222222 Banner show now created.");
                sb_bottom_Ab.Show();
                sb_bottom_Ab.SetPosition(AdPosition.Bottom);
            }
            else
            {
                Debug.Log("2222222222 Banner_Ab show reload created.");
                Btn_Create_Show_Ab(); // Create the ad if not already loaded
            }
        }
    }
    public void Btn_Hide_Bottom_Ab()
    {
        if (ADMOB_ID.Bottom_Allow == false)
        {
            // Nothing
        }
        else
        {
            Debug.Log("33333333 Banner_Ab hide.");
            sb_bottom_Ab.Hide();
        }
    }

    // Event
    private void HandleAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("Ad loaded successfully.");
        isAdLoaded = true;
        if (ADMOB_ID.Bottom_Allow == false)
        {
            // Nothing
        }
        else
        {
            isAdLoaded_Ab = true;
        }
    }
    private void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.LogError($"Ad failed to load:");
        isAdLoaded = false;
        if (ADMOB_ID.Bottom_Allow == false)
        {
            // Nothing
        }
        else
        {
            isAdLoaded_Ab = false;
        }
    }
}