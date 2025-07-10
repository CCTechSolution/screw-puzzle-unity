using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NultBolts
{
    public class NB_GameplayMenu : MonoBehaviour
    {
        

        [Space(15)]
        [Header("PANELS")]
        [SerializeField] GameObject gamePlayUI;
        [SerializeField] GameObject levelPanel;
        [SerializeField] GameObject victoryPanel;
        [SerializeField] GameObject settingPanel;
        [SerializeField] GameObject pausePanel;
        [SerializeField] GameObject pausePanelModes;
        [SerializeField] GameObject failPanel;




        [Space(15)]
        [Header("BUTTONS")]


        [Space(5)]
        [Header("Setting")]
        [SerializeField] private Button btnSetting;
        [SerializeField] private Button btnSettingBack;
        [SerializeField] private Button btnRateUs;

        [Space(5)]
        [Header("Fail")]
        [SerializeField] private Button btnRestart;
        [SerializeField] private Button btnHome2;
        [SerializeField] private Button btnAddTimer;

        [Space(5)]
        [Header("Pause")]
        [SerializeField] private Button btnPause;
        [SerializeField] private Button btnResume;
        [SerializeField] private Button btnHome;
        [SerializeField] private Button btnRestart2;

        [SerializeField] private GameObject btnSkip;


        [Space(15)]
        [Header("URLs")]
        public string rateUsLink;

        public static event Action OnLevelStart;
        public static event Action OnRewardTimeClicked;
        public static event Action OnGamePause;
        public static event Action OnGameResume;

        [SerializeField] private Text levelText;

        public static NB_GameplayMenu Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void Start()
        {
            //Fail
            btnRestart.onClick.AddListener(OnReset);
            btnHome2.onClick.AddListener(OnHome);
            btnAddTimer.onClick.AddListener(AddTime);


            //Setting
            btnSetting.onClick.AddListener(LevelToSetting);
            btnSettingBack.onClick.AddListener(OnSettingBack);

            //Pause
            btnPause.onClick.AddListener(OnPause);
            btnResume.onClick.AddListener(OnResume);
            btnHome.onClick.AddListener(OnHome);
            btnRestart2.onClick.AddListener(OnReset);

            //RateUs
            btnRateUs.onClick.AddListener(OnRateUs);
            
            NultBoltsManager.Instance.actionLoadLevel += UpdateTimer;
            NB_LevelController.OnLevelInstantiate += UpdateTimer;
            NB_Timer.OnTimerComplete += TimeEnded;
        }


        private void OnDisable()
        {
            //NultBoltsManager.Instance.actionLoadLevel -= ShowLevelTitle;
            NultBoltsManager.Instance.actionLoadLevel -= UpdateTimer;
            NB_LevelController.OnLevelInstantiate -= UpdateTimer;
            NB_Timer.OnTimerComplete -= TimeEnded;
        }

        //when timer ends or becomes zero
        private void TimeEnded()
        {
            failPanel.SetActive(true);
        }

        void ShowLevelTitle()
        {
            levelText.text = $"LEVEL {DataManager.indexLevel_NB + 1}";
        }


        void UpdateTimer()
        {
            Debug.Log("Update Timer");
            OnLevelStart?.Invoke();
        }

        public void CheckSkipLevel()
        {
            if (NultBoltsManager.Instance.currentLevelIndex >= 4)
            {
                btnSkip.SetActive(true);
            }
            else
            {
                btnSkip.SetActive(false);
            }
        }
        

        public void TimeOne()
        {
            Time.timeScale = 1f;
        }

        private void OnReset()
        {
            pausePanel.SetActive(false);
            failPanel.SetActive(false);    
            NultBoltsManager.Instance.ResetLevel();
        } 
        

        private void AddTime()
        {
            OnRewardTimeClicked?.Invoke();
        }


        //SETTINGS
        private void LevelToSetting()
        {
            NB_Timer.Instance.PauseTime();
            settingPanel.SetActive(true);
        }
        void OnSettingBack()
        {
            NB_Timer.Instance.ResumeTime();
            settingPanel.SetActive(false);
        }

        //PAUSE
        void OnPause()
        {
            if (ModeHandler.Instance.isModeLevel)
            {
                pausePanelModes.SetActive(true);
            }
            else
            {
                NB_Timer.Instance.PauseTime();
                pausePanel.SetActive(true);
                OnGamePause?.Invoke();
            }
            
        }
        void OnResume()
        {
            NB_Timer.Instance.ResumeTime();
            pausePanel.SetActive(false);
            OnGameResume?.Invoke();
        }
        
        void OnHome()
        {
            NB_Timer.Instance.PauseTime();
            failPanel.SetActive(false);
            pausePanel.SetActive(false);
            levelPanel.SetActive(true);
            NB_Timer.Instance.DontConsiderTimer();
        }
        

        void OnRateUs()
        {
            Application.OpenURL(rateUsLink);
        }

        public void Quit()
        {
            Application.Quit();
        }
        
    }
}
