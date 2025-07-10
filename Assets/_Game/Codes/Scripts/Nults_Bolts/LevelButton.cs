using System;
using UnityEngine;
using UnityEngine.UI;

namespace NultBolts
{
    public class LevelButton : MonoBehaviour
    {
        public bool isUnlocked;
        public int levelToLoad;
        public Text levelText;
        public GameObject green;
        public Button btn_Level_select;
        public NB_LevelController levelController;
        public GameObject currentPanel;


        public static event Action OnLevelSelected;
        private void Start()
        {
            levelText.text = levelToLoad.ToString();
            Invoke(nameof(CheckOnStartAndEnable), 0.1f);
        }

        private void OnEnable()
        {
            Invoke(nameof(CheckOnStartAndEnable), 0.1f);
        }

        void CheckOnStartAndEnable()
        {
            if (isUnlocked)
            {
                green.SetActive(true);
                btn_Level_select.onClick.AddListener(LoadLevel);
            }
            else
            {
                if (levelToLoad <= NultBoltsManager.Instance.latestLevel)
                {
                    green.SetActive(true);
                    btn_Level_select.onClick.AddListener(LoadLevel);
                }
                else
                {
                    green.SetActive(false);
                }
            }
        }

        public void LoadLevel()
        {
            NultBoltsManager.Instance.ChangeLevelIndex(levelToLoad);
            levelController.LoadLevel(levelToLoad);
            NultBoltsManager.Instance.actionLoadLevel?.Invoke();
            OnLevelSelected?.Invoke();
            currentPanel.SetActive(false);
            if (levelToLoad > 3)
            {
                NB_Timer.Instance.ConsiderTimer();
                NB_Timer.Instance.EnableTimer();
            }
            else
            {
                NB_Timer.Instance.DontConsiderTimer();
                NB_Timer.Instance.DisableTimer();
            }
            NB_GameplayMenu.Instance.CheckSkipLevel();
            NB_Timer.Instance.NB_GameplayMenu_OnLevelStart();
        }
    }
}
