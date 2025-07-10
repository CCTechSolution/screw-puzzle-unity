using UnityEngine;
using UnityEngine.Events;

public class CallInterstialAd : MonoBehaviour
{
    [SerializeField] float loadAdAfter = 5.8f;
    public UnityEvent ToDosAfterAD;
   public void CallInterstitialMax()
   {
       Debug.Log("Call Interstitial Ad");
    }

    private void OnEnable()
    {
        LoadInterstitialAd();
        Invoke(nameof(ShowInterstitial), loadAdAfter);
        Invoke(nameof(DisableGameobject), 5.9f);
        Invoke(nameof(MainWork), 6f);

    }


    void LoadInterstitialAd()
    {
        AdmobAdsManager.Instance.LoadInterstitial();
    }
    void ShowInterstitial()
    {
        AdmobAdsManager.Instance.ShowInterstitial();
           
    }
    void DisableGameobject()
    {
        gameObject.SetActive(false);
    }
    void MainWork()
    {
        ToDosAfterAD?.Invoke();
    }
}
