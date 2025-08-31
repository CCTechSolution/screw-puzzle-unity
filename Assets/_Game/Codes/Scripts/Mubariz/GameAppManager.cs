using System;
using NultBolts;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;

[Serializable]
public class InAppItem
{
    public string iapItem_Name;
    public ProductType producttype;
}

public class GameAppManager : MonoBehaviour, IStoreListener
{
    static GameAppManager instance_;
    public static GameAppManager Instance
    {
        get
        {
            if (!instance_)
            {
                instance_ = GameObject.FindObjectOfType<GameAppManager>();
                Debug.Log("[GameAppManager] Instance created");
            }
            return instance_;
        }
    }

    public InAppItem[] iapitems = null;
    public static event EventHandler consumable_events;
    private static IStoreController m_StoreController;
    private static IExtensionProvider m_StoreExtensionProvider;

    public static bool check_Unlockall = false;
    public static string remove_AdsString = "remove_adds";
    public static string UnlockAll = "unlockall";

    private static string kProductNameAppleSubscription = "com.unity3d.subscription.new";
    private static string kProductNameGooglePlaySubscription = "com.unity3d.subscription.original";

    public InGameWallPaper ingameWallpaper;

    void Awake()
    {
        Debug.Log("[GameAppManager] Awake called");
        //DontDestroyOnLoad(Instance);
    }

    void Start()
    {
        Debug.Log("[GameAppManager] Start called");

        if (m_StoreController == null)
        {
            Debug.Log("[GameAppManager] StoreController is null → initializing purchasing");
            InitializePurchasing();
        }
        else
        {
            Debug.Log("[GameAppManager] StoreController already initialized");
        }
    }

    // ========== PURCHASE WRAPPERS ==========
    public void BuyWallpapers() { Debug.Log("[IAP] BuyWallpapers called"); Buy_Product(0); }
    public void UnlockExcitedMode() { Debug.Log("[IAP] UnlockExcitedMode called"); Buy_Product(1); }
    public void UnlockHappyMode() { Debug.Log("[IAP] UnlockHappyMode called"); Buy_Product(2); }
    public void UnlockSadMode() { Debug.Log("[IAP] UnlockSadMode called"); Buy_Product(3); }
    public void RemoverTimerFromGame() { Debug.Log("[IAP] RemoverTimerFromGame called"); Buy_Product(4); }
    public void UnlockAllLevels() { Debug.Log("[IAP] UnlockAllLevels called"); Buy_Product(5); }
    public void UnlockEverything() { Debug.Log("[IAP] UnlockEverything called"); Buy_Product(6); }

    // ========== INITIALIZATION ==========
    public void InitializePurchasing()
    {
        Debug.Log("[GameAppManager] InitializePurchasing called");

        if (IsInitialized())
        {
            Debug.Log("[GameAppManager] Already initialized → skipping");
            return;
        }

        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        builder.AddProduct(remove_AdsString, ProductType.NonConsumable);

        Debug.Log("[GameAppManager] Adding IAP items...");
        for (int i = 0; i < iapitems.Length; i++)
        {
            Debug.Log($"[GameAppManager] Adding product: {iapitems[i].iapItem_Name}, type: {iapitems[i].producttype}");
            builder.AddProduct(iapitems[i].iapItem_Name, iapitems[i].producttype);
        }

        //builder.Configure<IGooglePlayConfiguration>().SetDeferredPurchaseListener(OnDeferredPurchase);
        UnityPurchasing.Initialize(this, builder);

        Debug.Log("[GameAppManager] UnityPurchasing.Initialize called");
    }

    void OnDeferredPurchase(Product product)
    {
        Debug.Log($"[IAP] Purchase of {product.definition.id} is deferred");
    }

    public void OnPurchaseDeferred(Product product)
    {
        Debug.Log("[IAP] OnPurchaseDeferred called for " + product.definition.id);
    }

    public bool IsInitialized()
    {
        bool result = (m_StoreController != null && m_StoreExtensionProvider != null);
        Debug.Log($"[GameAppManager] IsInitialized = {result}");
        return result;
    }

    // ========== BUY LOGIC ==========
    void Buy_noAds()
    {
        Debug.Log("[IAP] Buy_noAds called");
        if (IsInitialized())
        {
            if (!CheckProductID_Status(remove_AdsString))
            {
                Debug.Log("[IAP] Buying noAds");
                BuyProductID(remove_AdsString);
            }
            else { Debug.Log("[IAP] noAds already owned");

              
            }
        }
        else Debug.Log("[IAP] Not initialized, cannot buy noAds");
    }

    public void Buy_unlockall()
    {
        Debug.Log("[IAP] Buy_unlockall called");
        if (IsInitialized())
        {
            if (!CheckProductID_Status(UnlockAll))
            {
                Debug.Log("[IAP] Buying UnlockAll");
                BuyProductID(UnlockAll);
            }
            else Debug.Log("[IAP] UnlockAll already owned");
        }
        else Debug.Log("[IAP] Not initialized, cannot buy UnlockAll");
    }

    public void Buy_Product(int iapID)
    {
        Debug.Log($"[IAP] Buy_Product called with ID {iapID}");

        if (iapID >= iapitems.Length)
        {
            Debug.LogError("[IAP] Invalid IAP index: " + iapID);
            return;
        }

        if (IsInitialized())
        {
            var item = iapitems[iapID];
            Debug.Log($"[IAP] Attempting to buy {item.iapItem_Name}");

            if (item.producttype == ProductType.NonConsumable)
            {
                //if (!CheckProductID_Status(item.iapItem_Name))
                {
                    BuyProductID(item.iapItem_Name);
                }
                //else Debug.Log($"[IAP] {item.iapItem_Name} already owned");
            }
            else
            {
                BuyProductID(item.iapItem_Name);
            }
        }
        else
        {
            Debug.Log("[IAP] Cannot buy, not initialized");
        }
    }

    public bool CheckProductID_Status(string productId)
    {
        Debug.Log("[IAP] Checking product status: " + productId);

        Product product = m_StoreController?.products.WithID(productId);
        bool owned = product != null && product.hasReceipt;
        Debug.Log($"[IAP] Product {productId} owned? {owned}");
        return owned;
    }

    void BuyProductID(string productId)
    {
        Debug.Log("[IAP] BuyProductID called for " + productId);

        if (IsInitialized())
        {
            Product product = m_StoreController.products.WithID(productId);
            if (product != null && product.availableToPurchase)
            {
                Debug.Log($"[IAP] Purchasing product: {product.definition.id}");
                m_StoreController.InitiatePurchase(product);
            }
            else
            {
                Debug.Log($"[IAP] FAIL: Product {productId} not found or not available");
            }
        }
        else
        {
            Debug.Log("[IAP] FAIL: Not initialized");
        }
    }

    // ========== IStoreListener ==========
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        Debug.Log("[IAP] OnInitialized SUCCESS");

        m_StoreController = controller;
        m_StoreExtensionProvider = extensions;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.LogError("[IAP] OnInitializeFailed: " + error);
    }


    [SerializeField] GameObject LevelUnlockScreen;
    [SerializeField] GameObject BuyTimerScreen;
    [SerializeField] GameObject RemoveAdsScreen;
    [SerializeField] GameObject RemoveAdsScreen2;
    [SerializeField] GameObject Lock_Happy;
    [SerializeField] GameObject Lock_Sad;
    [SerializeField] GameObject Lock_Excited;


    [SerializeField] GameObject icon_jumbo;
    [SerializeField] GameObject icon_removeTime;
    [SerializeField] GameObject icon_unlockLevels;

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs args)
    {

        if (String.Equals(args.purchasedProduct.definition.id, remove_AdsString, StringComparison.Ordinal))
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));
        }

        //0
        else if (String.Equals(args.purchasedProduct.definition.id, GameAppManager.Instance.iapitems[0].iapItem_Name, StringComparison.Ordinal))//unlock_all
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));


            ingameWallpaper.UnlockAllWallPapers();
            Debug.Log("RemovedAds and UnlockWallpaper");

            FirebaseInit.instance.LogEvent("IAP Purchase : " + GameAppManager.Instance.iapitems[0].iapItem_Name);
        }

        //1 EXCITED MODE
        else if (String.Equals(args.purchasedProduct.definition.id, GameAppManager.Instance.iapitems[1].iapItem_Name, StringComparison.Ordinal))//unlock_player
        {
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            PlayerPrefs.SetInt("Excited", 1);
            Debug.Log("Excited InApp");

            FirebaseInit.instance.LogEvent("IAP Purchase : " + GameAppManager.Instance.iapitems[1].iapItem_Name);
            Lock_Excited.SetActive(false);
            ModeHandler.Instance.UpdateState();

        }

        //2 HAPPY MODE
        else if (String.Equals(args.purchasedProduct.definition.id, GameAppManager.Instance.iapitems[2].iapItem_Name, StringComparison.Ordinal))//unlock_levels
        {

            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            PlayerPrefs.SetInt("Happy", 1);
            Debug.Log("Happy InApp");
            Lock_Happy.SetActive(false);
            FirebaseInit.instance.LogEvent("IAP Purchase : " + GameAppManager.Instance.iapitems[2].iapItem_Name);
            ModeHandler.Instance.UpdateState();

        }

        //3 SAD MODE
        else if (String.Equals(args.purchasedProduct.definition.id, GameAppManager.Instance.iapitems[3].iapItem_Name, StringComparison.Ordinal))//cars
        {

            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            ModeHandler.Instance.UpdateState();

            PlayerPrefs.SetInt("Sad", 1);
            Debug.Log("Sad InApp");
            Lock_Sad.SetActive(false);

            FirebaseInit.instance.LogEvent("IAP Purchase : " + GameAppManager.Instance.iapitems[3].iapItem_Name);

        }

        //4 REMOVE TIMER
        else if (String.Equals(args.purchasedProduct.definition.id, GameAppManager.Instance.iapitems[4].iapItem_Name, StringComparison.Ordinal))//cars
        {
            icon_removeTime.SetActive(false);
            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            BuyTimerScreen.SetActive(false);

            PlayerPrefs.SetInt("RemoveTimer", 1);
            Debug.Log("Timer remove from the game");
            ModeHandler.Instance.UpdateState();

            FirebaseInit.instance.LogEvent("IAP Purchase : " + GameAppManager.Instance.iapitems[4].iapItem_Name);

        }

        //5  UNLOCK ALL LEVELS
        else if (String.Equals(args.purchasedProduct.definition.id, GameAppManager.Instance.iapitems[5].iapItem_Name, StringComparison.Ordinal))//cars
        {
            icon_unlockLevels.SetActive(false);

            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            LevelUnlockScreen.SetActive(false);

            PlayerPrefs.SetInt("AllLevels", 1);
            NB_LevelLoader.Instance.UnlockAllLevels();
            LevelUnlockScreen.SetActive(false);
            FirebaseInit.instance.LogEvent("IAP Purchase : " + GameAppManager.Instance.iapitems[5].iapItem_Name);

        }

        //6  All Jumbo
        else if (String.Equals(args.purchasedProduct.definition.id, GameAppManager.Instance.iapitems[6].iapItem_Name, StringComparison.Ordinal))//cars
        {
            icon_jumbo.SetActive(false);

            Debug.Log(string.Format("ProcessPurchase: PASS. Product: '{0}'", args.purchasedProduct.definition.id));

            PlayerPrefs.SetInt("noADS", 1);
            AdmobAdsManager.Instance.Skip_Appopen = true;
            AdmobAdsManager.Instance.Skip_MedRec = true;
            AdmobAdsManager.Instance.Skip_Banner_Top = true;
            PlayerPrefs.SetInt("AllLevels", 1);
            NB_LevelLoader.Instance.UnlockAllLevels();
            PlayerPrefs.SetInt("RemoveTimer", 1);
            Debug.Log("Timer remove from the game");
            PlayerPrefs.SetInt("Sad", 1);
            Debug.Log("Sad InApp");
            PlayerPrefs.SetInt("Happy", 1);
            Debug.Log("Happy InApp");
            PlayerPrefs.SetInt("Excited", 1);
            Debug.Log("Excited InApp");
            ingameWallpaper.UnlockAllWallPapers();
            Debug.Log("RemovedAds and UnlockWallpaper");
            RemoveAdsScreen.SetActive(false);
            RemoveAdsScreen2.SetActive(false);

            AdmobAdsManager.Instance.hideSmallBanner();
            AdmobAdsManager.Instance.hideMediumBanner();

            FirebaseInit.instance.LogEvent("IAP Purchase : " + GameAppManager.Instance.iapitems[6].iapItem_Name);

        }
        else
        {
            Debug.Log(string.Format("ProcessPurchase: FAIL. Unrecognized product: '{0}'", args.purchasedProduct.definition.id));
        }
        InAppPurchased();
        Invoke(nameof(LoadScene), 3f);

        //Data.SaveData();
        return PurchaseProcessingResult.Complete;
    }
    void InAppPurchased()
    {
        AdmobAdsManager.Instance.Btn_InApp_Done();
    }
    void LoadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"[IAP] OnPurchaseFailed: {product.definition.storeSpecificId}, reason: {failureReason}");
    }

    void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {
        Debug.LogError($"[IAP] Explicit OnPurchaseFailed: {product.definition.storeSpecificId}, reason: {failureReason}");
    }

    void IStoreListener.OnInitializeFailed(InitializationFailureReason error, string message)
    {
        Debug.LogError($"[IAP] Explicit OnInitializeFailed: {error}, message: {message}");
    }
}
