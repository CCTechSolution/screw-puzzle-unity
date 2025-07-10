using System;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

namespace NultBolts
{
    public class NB_Timer : MonoBehaviour
    {
        public static NB_Timer Instance;

        [Space(5)]
        [Header("INAPP")]

        public bool isTimerRemovedFromTheGame;
        public GameObject timerUIParent;
        public GameObject timerInappButton;
        public GameObject timerInappButton2;


        [Space(5)]
        [Header("Timers for Level")]
        public Button adTenSeconds;

        public float[] timerForGames;

        public GameObject timerUIGameobject;
        public Text timeText;
        public float timer;
        float maxTimer;
        bool canStart;
        bool canConsiderTimer;
        bool isPaused;
        public Image timeFiller;
        public GameObject rewardLoadingScreen;
        public GameObject failPanel;

        bool canFailAppear;

        public static event Action OnTimerComplete;

        private void Awake()
        {
            Instance = this;
        }
        private void OnEnable()
        {
            Invoke(nameof(NB_GameplayMenu_OnLevelStart), 0.5f);
        }

        private void Start()
        {
            CheckInapp();
            canStart = false;
            NB_LevelController.OnLevelInstantiate += NB_GameplayMenu_OnLevelStart;
            NB_GameplayMenu.OnLevelStart += NB_GameplayMenu_OnLevelStart;
            NB_GameplayMenu.OnGamePause += PauseTime;
            NB_GameplayMenu.OnGameResume += ResumeTime;
            NB_GameplayMenu.OnRewardTimeClicked += AddTimer;
            adTenSeconds.onClick.AddListener(Ad10SecondsDuringGameplay);
        }

        private void OnDestroy()
        {
            NB_LevelController.OnLevelInstantiate -= NB_GameplayMenu_OnLevelStart;
            NB_GameplayMenu.OnLevelStart -= NB_GameplayMenu_OnLevelStart;
            NB_GameplayMenu.OnGamePause -= PauseTime;
            NB_GameplayMenu.OnGameResume += ResumeTime;
            NB_GameplayMenu.OnRewardTimeClicked += AddTimer;
        }


        public  void NB_GameplayMenu_OnLevelStart()
        {

            if (canConsiderTimer && DataManager.indexLevel_NB > 2)
            {
                UpdateTimer();
            }
            else
            {
                timerUIGameobject.SetActive(false);
            }
        }

        void UpdateTimer()
        {
            timerUIGameobject.SetActive(true);
            timer = 0f;

            canStart = true;
            isPaused = false;
            SetTimerForLevel();
            timer = maxTimer;
            timeFiller.fillAmount = maxTimer;
        }

        private void Update()
        {
            if (canConsiderTimer)
            {
                if (!canStart)
                    return;

                if (!isPaused)
                {
                    if (timer > 0f)
                    {
                        timer -= Time.deltaTime;

                        // 🔁 Update fill amount (0 to 1)
                        timeFiller.fillAmount = timer / maxTimer;
                    }

                    if (timer <= 0f)
                    {
                        timer = 0f;
                        timeFiller.fillAmount = 0f;
                        canFailAppear = true;



                        if (canFailAppear)
                        {
                            OnTimerComplete?.Invoke();
                            canFailAppear = false;
                        }

                    }
                }
            }


            // Update time text
            int displayTime = Mathf.CeilToInt(timer);
            timeText.text = displayTime.ToString();
        }

        public void DisableTimer()
        {
            timerUIGameobject.SetActive(false);
        }
        public void EnableTimer()
        {
            timerUIGameobject.SetActive(true);
        }


        void SetTimerForLevel()
        {
            //Debug.Log(DataManager.indexLevel_NB + 1);
            maxTimer = timerForGames[DataManager.indexLevel_NB];
        }

        public void AddTimer()
        {
            PauseTime();
            rewardLoadingScreen.SetActive(true);
            if (AdmobAdsManager.Instance)
            {
                AdmobAdsManager.Instance.LoadRewardedVideo();
            }
            Invoke(nameof(After6sec), 6f);
        }

        void After6sec()
        {
            rewardLoadingScreen.SetActive(false);
            if (AdmobAdsManager.Instance)
            {
                AdmobAdsManager.Instance.ShowRewardedVideo(Add10Seconds);
            }
        }

        void Ad10SecondsDuringGameplay()
        {
            RewardLoading.Instance.StartLoading(Add10Seconds);
        }

        void Add10Seconds()
        {
            failPanel.SetActive(false);
            ResumeTime();
            timer += 10f;
            AdmobAdsManager.Instance.Btn_Reward_Done();
        }



        [ContextMenu("Check Level in Console")]
        public void CheckLevelNumber()
        {
            Debug.Log("Current level number is : " + DataManager.indexLevel_NB + 1);
        }
        [ContextMenu("Pause Time")]
        public void PauseTime()
        {
            isPaused = true;
        }
        [ContextMenu("Resume Time")]
        public void ResumeTime()
        {
            isPaused = false;
        }

        public void ConsiderTimer()
        {
            if (isTimerRemovedFromTheGame)
            {
                canConsiderTimer = false;
            }
            else
            {
                canConsiderTimer = true;
            }
        }

        public void DontConsiderTimer()
        {
            canConsiderTimer = false;
        }

        public void RemoveTimerFromGame()
        {
            PlayerPrefs.SetInt("RemoveTimer", 1);
            isTimerRemovedFromTheGame = true;
            timerUIParent.SetActive(false);
            timerInappButton.SetActive(false);
            timerInappButton2.SetActive(false);
        }

        void CheckInapp()
        {
            if (PlayerPrefs.GetInt("RemoveTimer") == 1)
            {
                isTimerRemovedFromTheGame = true;
                timerUIParent.SetActive(false);
                timerInappButton.SetActive(false);
                timerInappButton2.SetActive(false);
            }
        }
    }


}
