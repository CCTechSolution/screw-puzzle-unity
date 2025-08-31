using System;
using UnityEngine;
using UnityEngine.UI;

namespace NultBolts
{
    public class ModeHandler : MonoBehaviour
    {
        public static ModeHandler Instance;

        public bool isModeLevel;
        public int levelNumber;
        public NB_LevelController levelController;


        [Space(5)]
        [Header("Pause")]
        public Button btnHome;
        public Button btnRestart;
        public Button btnResume;
        public GameObject pausePanelMode;


        [Space(5)]
        [Header("Excited")]
        public Button excitedModeButton;
        bool excitedUnlocked;
        public GameObject lock1;
        public GameObject black1;


        [Space(3)]
        [Header("Happy")]
        public Button happyModeButton;
        bool happyUnlocked;
        public GameObject lock2;
        public GameObject black2;


        [Space(3)]
        [Header("Sad")]
        public Button sadModeButton;
        bool sadUnlocked;
        public GameObject lock3;
        public GameObject black3;

        [Space(3)]
        [Header("Other")]
        public GameObject levelTextGameobject;
        public GameObject modesPanel;
        public GameObject levelPanel;
        public Button btnModeOpen;
        public Button btnModesPanelBack;
        public Button btnNextComplete;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            UpdateState();
            excitedModeButton.onClick.AddListener(OnExcitedButtonClicked);
            happyModeButton.onClick.AddListener(OnHappyButtonClicked);
            sadModeButton.onClick.AddListener(OnSadButtonClicked);
            btnNextComplete.onClick.AddListener(CompleteNextLevel);
            btnModeOpen.onClick.AddListener(OnModesButtonClicked);
            btnModesPanelBack.onClick.AddListener(OnModePanelBack);
            btnHome.onClick.AddListener(ToHome);
            btnRestart.onClick.AddListener(Restart);
            btnResume.onClick.AddListener(Resume);
        }

        public void UpdateState()
        {
            if (PlayerPrefs.GetInt("Excited") == 1 || NultBoltsManager.Instance.latestLevel >= 20)
            {
                excitedUnlocked = true;
            }
            if (PlayerPrefs.GetInt("Happy") == 1 || NultBoltsManager.Instance.latestLevel >= 40)
            {
                happyUnlocked = true;
            }
            if (PlayerPrefs.GetInt("Sad") == 1 || NultBoltsManager.Instance.latestLevel >= 60)
            {
                sadUnlocked = true;
            }
            //1
            if (excitedUnlocked)
            {
                lock1.SetActive(false);
                black1.SetActive(false);
            }
            else
            {
                lock1.SetActive(true);
                black1.SetActive(true);
            }
            //2
            if (happyUnlocked)
            {
                lock2.SetActive(false);
                black2.SetActive(false);
            }
            else
            {
                lock2.SetActive(true);
                black2.SetActive(true);
            }
            //3
            if (sadUnlocked)
            {
                lock3.SetActive(false);
                black3.SetActive(false);
            }
            else
            {
                lock3.SetActive(true);
                black3.SetActive(true);
            }
        }

        void OnExcitedButtonClicked()
        {
            if (!excitedUnlocked)
            {
                GameAppManager.Instance.UnlockExcitedMode();
                return;
            }

            AnyModeClicked();

        }
        void OnHappyButtonClicked()
        {
            if (!happyUnlocked)
            {
                GameAppManager.Instance.UnlockHappyMode();
                return;
            }

            AnyModeClicked();
        }
        void OnSadButtonClicked()
        {
            if (!sadUnlocked)
            {
                GameAppManager.Instance.UnlockSadMode();
                return;
            }

            AnyModeClicked();
        }

        void AnyModeClicked()
        {
            RandomLevel();
            isModeLevel = true;
            modesPanel.SetActive(false);
            levelPanel.SetActive(false);
            NB_Timer.Instance.DisableTimer();
            NB_Timer.Instance.DontConsiderTimer();
            levelController.LoadLevel(levelNumber);
        }

        public void CompleteNextLevel()
        {
            levelController.LoadLevel(levelNumber);
        }

        void OnModesButtonClicked()
        {
            isModeLevel = true;
            levelPanel.SetActive(false);
            levelTextGameobject.SetActive(false);
            modesPanel.SetActive(true);
        }
        void OnModePanelBack()
        {
            isModeLevel = false;
            modesPanel.SetActive(false);
            levelPanel.SetActive(true);
            levelTextGameobject.SetActive(true);
        }
        void RandomLevel()
        {
            levelNumber = UnityEngine.Random.Range(60, 100);
        }

        void ToHome()
        {
            pausePanelMode.SetActive(false);
            OnModePanelBack();
        }

        void Resume()
        {
            pausePanelMode.SetActive(false);
        }

        void Restart()
        {
            pausePanelMode.SetActive(false);
            levelController.LoadLevel(levelNumber);
        }
        
    }
}
