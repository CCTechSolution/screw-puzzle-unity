using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class new_AD_Call : MonoBehaviour
{
    public int Total_Coins;
    string Coin_Call;
    int Coin;
    public Text[] All_Coin;
    public bool AdoptiveBanner;
    public bool MedRec;
    public bool Interstitial;
    public bool Rew_AD;

    [Header(" . . . Loading . . . ")]
    public bool Load_Ban;
    public bool Load_Med_Rec;
    public bool Load_Rew;
    public bool Load_Int;

    [Header("Reward")]
    public GameObject Reward;
    public bool Game_Pannel;

    [Header(" . . . GamePlay . . . ")]
    public bool Fight;
    public bool Shoot_Transform;

    void OnEnable()
    {
        //Data.LoadData();

        tsk();
        // no
        if (AdoptiveBanner == true)
        {
            bann();
        }
        if (MedRec == true)
        {
            if (AdmobAdsManager.Instance.Skip_MedRec)
            {
                return;
            }
            mrban();
        }
        if (Interstitial == true)
        {
            AdmobAdsManager.Instance.ShowInterstitial();
        }
        if (Fight == true)
        {
            if (AdmobAdsManager.Instance.Skip_MedRec)
            {
                return;
            }
            bann();
            AdmobAdsManager.Instance.hideMediumBanner();


        }
        if (Shoot_Transform == true)
        {
            if (AdmobAdsManager.Instance.Skip_MedRec)
            {
                return;
            }
            bann();
            AdmobAdsManager.Instance.hideMediumBanner();
        }
        loAd();
    }
    void OnDisable()
    {
        if (Interstitial == true)
        {
            // no
        }
        if (MedRec == true)
        {
            if (AdmobAdsManager.Instance)
                AdmobAdsManager.Instance.hideMediumBanner();
        }

        if (AdoptiveBanner == true)
        {
            Debug.Log("Hide Adaptive Banner");
            if (AdmobAdsManager.Instance)
                AdmobAdsManager.Instance.hideSmallBanner();
        }

        //Data.SaveData();
    }

    public void InT_Now()
    {
        AdmobAdsManager.Instance.ShowInterstitial();
    }
    public void MRec_Now()
    {
        mrban();
    }
    void loAd()
    {
        if (Load_Ban == true)
        {
            if (AdmobAdsManager.Instance.Skip_MedRec)
            {
                return;
            }
            bann();
        }
        if (Load_Med_Rec == true)
        {
            // no
        }
        if (Load_Int == true)
        {
            // no
        }
        if (Load_Rew == true)
        {
            // no
        }
    }
    public void Btn_Call_Coin(string val)
    {
        Coin_Call = val;
    }
    public void Get_Coins_Usama()
    {
        if (Game_Pannel == true)
        {
            AdmobAdsManager.Instance.hideMediumBanner();
        }

        // no
        // Reward.SetActive(true);
        Invoke("waitAD_now", 0.1f);
    }
    void waitAD_now()
    {
        //Reward.SetActive(false);
        AdmobAdsManager.Instance.ShowRewardedVideo(Chk_Coins);
    }

    int coin_x;
    int coin_y;

    void Chk_Coins()
    {

    }
    void tsk()
    {
        Total_Coins = PlayerPrefs.GetInt("TotalCoins");

        foreach (Text item in All_Coin)
        {
            if (item != null)
                item.GetComponent<Text>().text = ("$ " + Total_Coins).ToString();
        }
        Invoke("tsk", 1f);
    }


    void bann()
    {
        if (AdmobAdsManager.Instance)
        {
            if (!AdmobAdsManager.Instance.IsSmallBannerReady())
            {
                load_bann();
            }
            else
            {
                AdmobAdsManager.Instance.ShowSmallBanner();
            }

        }
    }


    void load_bann()
    {
        if (!AdmobAdsManager.Instance.IsSmallBannerReady())
        {
            AdmobAdsManager.Instance.LoadSmallBanner();
            Invoke(nameof(trry), 3f);
        }
        else
        {
            AdmobAdsManager.Instance.ShowSmallBanner();
        }
    }
    void trry()
    {
        bann();
    }

    void mrban()
    {
        if (AdmobAdsManager.Instance)
            if (AdmobAdsManager.Instance.Internet == true)
            {
                if (AdmobAdsManager.Instance.Skip_MedRec)
                {
                    return;
                }
                AdmobAdsManager.Instance.ShowMediumBanner();
            }
    }
    void load_mrban()
    {
        if (AdmobAdsManager.Instance.Internet == true)
        {
            //no  if (!AdmobAdsManager.Instance.IsMediumBannerReady())
            {
                // no
            }
        }
    }
    public void Btn_Int_Call()
    {
        if (AdmobAdsManager.Instance.Skip_Int)
        {
            return;
        }
        if (AdmobAdsManager.Instance)
            AdmobAdsManager.Instance.ShowInterstitial();
    }
}