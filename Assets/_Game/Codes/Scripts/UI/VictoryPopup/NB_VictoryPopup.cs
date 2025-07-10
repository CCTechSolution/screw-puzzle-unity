using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NultBolts
{
    public class NB_VictoryPopup : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] Button btn_Next;
        [SerializeField] GameObject inappPanel;
        int currenProgress;
        public int showPanelAfterLevels;
        private void Start()
        {
            btn_Next.onClick.AddListener(Next);
        }

        void Next()
        {
            NultBoltsManager.Instance.NextLevel();
            gameObject.SetActive(false);
            NB_GameplayMenu.Instance.CheckSkipLevel();

        }

        private void OnEnable()
        {
            if (DataManager.indexLevel_NB <= 3)
            {
                return;
            }
            else
            {
                if (currenProgress >= showPanelAfterLevels)
                {
                    inappPanel.SetActive(true);
                    currenProgress = 0;
                }
            }
            currenProgress ++;
           
        }
    }
}
