using UnityEngine;

namespace NultBolts
{
    public class InterstitialAndReward : MonoBehaviour
    {
        public void LoadInterstitial()
        {
            AdmobAdsManager.Instance.LoadInterstitial();
        }
        public void ShowInterstitial()
        {
            AdmobAdsManager.Instance.ShowInterstitial();
        }
        public void LoadRewardedVideo()
        {
            AdmobAdsManager.Instance.LoadRewardedVideo();
        }
    }
}
