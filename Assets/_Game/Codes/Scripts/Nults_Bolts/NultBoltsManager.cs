using System;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NultBolts
{
    public class NultBoltsManager : SingletonMonoBehaviour<NultBoltsManager>
    {
        public int rewardCoinsOnLevelComplete;

        [Header("Setup")]
        [SerializeField] NB_LevelController levelController;

        [Header("Popups")]
        [SerializeField] GameObject levelVictory;
        [SerializeField] GameObject modeVictory;

        [Header("Buttons")]
        [SerializeField] Button skipButton;

        public InGameWallPaper InGameWallPaper;
        public SpriteRenderer bgSprite;

        public Text LevelText;

        public TypeManager.GameState gameState;

        public Sprite[] sprites;

        public Action actionLoadLevel;

        public bool CanVibrate;

        /// <summary>
        /// ///////////////////////
        /// </summary>
        public int currentLevelIndex;
        public int latestLevel;
        const string PlayerPrefsKey_CurrentLevel = "CurrentLevel";
        const string PlayerPrefsKey_LatestLevel = "LatestLevel";


        private void Start()
        {
            latestLevel = PlayerPrefs.GetInt(PlayerPrefsKey_LatestLevel, 0);

            Application.targetFrameRate = 60;
            gameState = TypeManager.GameState.Ready;

            skipButton.onClick.AddListener(SkipLevel);

            NB_LevelController.OnLevelInstantiate += UpdateLevelText;

            LoadLevel();

            currentLevelIndex = PlayerPrefs.GetInt(PlayerPrefsKey_CurrentLevel, 1);
            latestLevel = PlayerPrefs.GetInt(PlayerPrefsKey_LatestLevel, 0);

        }

        private void OnDestroy()
        {
            NB_LevelController.OnLevelInstantiate -= UpdateLevelText;
        }
        void LoadLevel()
        {
            levelController.LoadLevel(DataManager.indexLevel_NB + 1);
            actionLoadLevel?.Invoke();
        }

        public void ResetLevel()
        {
            levelController.LoadLevel(currentLevelIndex);
        }

        public void SkipThisLevel()
        {
            Debug.Log("working");
            ChangeGameState(TypeManager.GameState.Win);
            //NextLevel();
            AdmobAdsManager.Instance.Btn_Reward_Done();
        }

        void SkipLevel()
        {
            if (AdmobAdsManager.Instance)
            {
                RewardLoading.Instance.StartLoading(SkipThisLevel);
            }
        }

        public void NextLevel()
        {
            if (currentLevelIndex <= 3)
            {
                NB_Timer.Instance.DontConsiderTimer();
                NB_Timer.Instance.DisableTimer();
            }
            else
            {
                NB_Timer.Instance.ConsiderTimer();
                NB_Timer.Instance.DisableTimer();
            }
            currentLevelIndex++;

            if (currentLevelIndex > latestLevel)
            {
                latestLevel = currentLevelIndex;
                PlayerPrefs.SetInt(PlayerPrefsKey_LatestLevel, latestLevel);
            }

            // Always save current
            PlayerPrefs.SetInt(PlayerPrefsKey_CurrentLevel, currentLevelIndex);
            PlayerPrefs.Save();

            levelController.LoadLevel(currentLevelIndex);


        }

        public void ChangeGameState(TypeManager.GameState _state)
        {
            if (_state != gameState)
            {
                SetGameState(_state);
            }
        }

        void SetGameState(TypeManager.GameState _state)
        {
            gameState = _state;
            switch (gameState)
            {
                case TypeManager.GameState.Ready:
                    Debug.Log("Game ready");
                    NB_Timer.Instance.ResumeTime();
                    break;
                case TypeManager.GameState.Playing:
                    Debug.Log("Game Playing");
                    NB_Timer.Instance.ResumeTime();
                    break;
                case TypeManager.GameState.Win:
                    DataManager.indexLevel_NB += 1;
                    if (DataManager.levelUnlock_NB < DataManager.indexLevel_NB)
                    {
                        DataManager.levelUnlock_NB = DataManager.indexLevel_NB;
                    }

                    if (ModeHandler.Instance.isModeLevel)
                    {
                        modeVictory.SetActive(true);
                    }
                    else
                    {
                        if (DataManager.indexLevel_NB > 3 && AdmobAdsManager.Instance)
                        {
                            NB_Timer.Instance.PauseTime();
                            levelVictory.SetActive(true);
                           
                        }
                        else
                        {
                            levelVictory.SetActive(true);
                        }
                    }

                    CoinsManager.Instance.AddCoins(rewardCoinsOnLevelComplete);

                    break;
                case TypeManager.GameState.Lose:

                    break;
            }
        }

        public void AssignRandomWallpaper(SpriteRenderer spriteRenderer)
        {
            if (sprites.Length == 0 || spriteRenderer == null) return;

            int randomIndex = UnityEngine.Random.Range(0, sprites.Length);
            spriteRenderer.sprite = sprites[randomIndex];
        }

        void ShowInterstitialAd()
        {
            AdmobAdsManager.Instance.ShowInterstitial();
        }


        public void ChangeLevelIndex(int index)
        {
            currentLevelIndex = index;
            PlayerPrefs.SetInt(PlayerPrefsKey_CurrentLevel, currentLevelIndex);
        }

        void UpdateLevelText()
        {
            LevelText.text = $"LEVEL {currentLevelIndex}";
        }

        public void UpdateLevelTextOnLevelClick(int levelNumber)
        {
            LevelText.text = $"LEVEL {levelNumber}";
        }
    }
}
