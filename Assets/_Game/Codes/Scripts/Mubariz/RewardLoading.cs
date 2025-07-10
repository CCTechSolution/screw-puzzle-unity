using System;
using UnityEditor;
using UnityEngine;

namespace NultBolts
{
    public class RewardLoading : MonoBehaviour
    {
        public GameObject rewardLoadingPanel;

        public static RewardLoading Instance;

        Action curernt_Action;

        private void Awake()
        {
            Instance = this;
        }

        public void StartLoading(System.Action action)
        {
            rewardLoadingPanel.SetActive(true);
            curernt_Action = null;
            curernt_Action = action;
            if (AdmobAdsManager.Instance)
            {
                AdmobAdsManager.Instance.LoadRewardedVideo();
            }
            Invoke(nameof(ActionToDo),6f);
        }

        void ActionToDo()
        {
            rewardLoadingPanel?.SetActive(false);
           

            if (AdmobAdsManager.Instance)
            {
                AdmobAdsManager.Instance.ShowRewardedVideo(CurrentAction);
                if (AdmobAdsManager.Instance.ID_Test)
                {
                    CurrentAction();
                }
            }
        }

        void CurrentAction()
        {
            Invoke(nameof(NowDoAction), 0.35f);
        }

        void NowDoAction()
        {
            curernt_Action.Invoke();
            AdmobAdsManager.Instance.Btn_Reward_Done();
        }
    }
}
