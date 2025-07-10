using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Reward_Video_Loading : MonoBehaviour
{
    bool check;
    public int rewardnumber;
    public Image Bar;
    public float Loading_Time = 8;
    public static Reward_Video_Loading ins;
    public GameObject MessagePanel;
    public Text Notification;
    public static bool ShowMRECAfterVideo = false;
    private void OnEnable()
    {
        ins = this;
        check = false;
        Loading_Time = 8;
        Bar.fillAmount = 0f;
        Time.timeScale = 1;
        if (AdmobAdsManager.Instance)
        {
            if (AdmobAdsManager.Instance.IsRewardedAdReady())
            {
                Loading_Time = .5f;
                if (Notification) { Notification.text = "CONGRATULATIONS YOUR REWARDED COINS HAS BEEN ADDED \n "; }
            }
            else
            {
                if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork || Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
                {
                    load_rew();
                    if (AdmobAdsManager.Instance.Ads_Googel_Max == true)
                    {
                        Loading_Time = 0.1f;
                    }
                    else
                    {
                        Loading_Time = 8;
                    }
                }
                else
                {
                    Loading_Time = .5f;
                    if (Notification) { Notification.text = "INTERNET CONNECTION FAILED \n PLEASE CHECK YOUR INTERNET CONNECTION"; }
                }
            }
        }
    }
    void Update()
    {
        if (!check && Bar.fillAmount < 1)
            Bar.fillAmount += 1 / Loading_Time * Time.unscaledDeltaTime;

        // else { Bar.fillAmount += 1 / Loading_Time * Time.deltaTime; }   

        if (!check && Bar.fillAmount >= 1)
        {


            if (AdmobAdsManager.Instance)
            {
                if (AdmobAdsManager.Instance.IsRewardedAdReady())
                {
                    show_rew();
                }
                else
                {
                    if (Notification) { Notification.text = "Reward Video Not AVAILABLE  \n PLEASE TRY AGAIN "; }
                    if (MessagePanel) { MessagePanel.SetActive(true); }
                }

            }
            else
            {
                if (Notification) { Notification.text = "INTERNET CONNECTION FAILED \n PLEASE CHECK YOUR INTERNET CONNECTION"; }
                if (MessagePanel) { MessagePanel.SetActive(true); }
            }

            this.gameObject.SetActive(false);
            check = true;
        }
    }
    public void Chk_Coins()
    {
        AdmobAdsManager.Instance.Btn_Reward_Done();

        Debug.Log("Reward is given");

        // R
        if (Notification) { Notification.text = "CONGRATULATIONS YOUR REWARDED COINS HAS BEEN ADDED "; }
        if (MessagePanel)
        {
            MessagePanel.SetActive(true);
        }


        switch (rewardnumber)
        {
            case 0:
                //  ASH_GamePlayUIManager.Instance.revive();
                if (Notification) { Notification.text = "Health is refilled start your mission"; }
                if (MessagePanel)
                {
                    MessagePanel.SetActive(true);
                }
                break;


        }
    }

    void load_rew()
    {
        if (AdmobAdsManager.Instance.Ads_Googel_Max == true)
        {
            // ADsMax
        }
        else
        {
            AdmobAdsManager.Instance.LoadRewardedVideo();
        }
    }
    void show_rew()
    {
        if (AdmobAdsManager.Instance.Ads_Googel_Max == true)
        {
            //MaxAdsManager.Instance.Btn_LS_Rew(Chk_Coins);
        }
        else
        {
            AdmobAdsManager.Instance.ShowRewardedVideo(Chk_Coins);
        }
    }
}