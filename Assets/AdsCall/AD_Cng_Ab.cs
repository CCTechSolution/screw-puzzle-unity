using UnityEngine;
using GoogleMobileAds.Api;

public class AD_Cng_Ab : MonoBehaviour
{
    public bool AB;

    void OnEnable()
    {
        if (PlayerPrefs.GetInt("Shop") == 1)
        {
            // ADs Off
        }
        else
        {
            AB = true;
            Invoke(nameof(call_show), 0.1269f);
        }
    }

    void OnDisable()
    {
        call_hide();
    }
    void call_show()
    {
        if (AdmobAdsManager.Instance.Skip_Banner_B_Adp)
        {
            return;
        }
        if (AdmobAdsManager.Instance)
            AdmobAdsManager.Instance.Btn_Show_Bottom_Ab();
    }
    void call_hide()
    {
        if (AdmobAdsManager.Instance.Skip_Banner_B_Adp)
        {
            return;
        }
        AB = false;
        if(AdmobAdsManager.Instance)
        AdmobAdsManager.Instance.Btn_Hide_Bottom_Ab();
    }
}