using System;
using UnityEngine;
using UnityEngine.UI;

namespace NultBolts
{
    public class WallPaperData : MonoBehaviour
    {
        public Sprite m_Sprite;
        public bool isUnlocked;
        public int count; // Unique ID for wallpaper
        public Button button;
        public GameObject lockIcon; // Optional: drag your lock icon here in Inspector
        public GameObject m_Selector;

        private const string UnlockKeyPrefix = "WALLPAPER_UNLOCKED_";

        private const string WallPaperUnlockedKey = "WallPaper";

        public static event EventHandler<OnWallpaperclickedEventArgs> OnWallpaperClicked;

        public class OnWallpaperclickedEventArgs : EventArgs
        {
            public Sprite sprite;
            public GameObject Selector;
        }

        private void Start()
        {
            if (PlayerPrefs.GetInt(WallPaperUnlockedKey) == 1)
            {
                isUnlocked = true;
            }
            LoadState();
            UpdateUI();

            button.onClick.AddListener(Clicked);
        }

        void Clicked()
        {
            Debug.Log("button is pressed");

            if (isUnlocked)
            {
                Debug.Log("image is unlocked");
                OnWallpaperClicked?.Invoke(this, new OnWallpaperclickedEventArgs()
                {
                    sprite = m_Sprite,
                    Selector = m_Selector
                    
                });
                m_Selector.SetActive(true);
            }
            else
            {
                RewardLoading.Instance.StartLoading(OnAdWatched);
                //AdmobAdsManager.Instance.ShowRewardedVideo(OnAdWatched);
            }
        }

        void OnAdWatched()
        {
            Invoke(nameof(ChangeLockState), 0.1f); // optional delay
            AdmobAdsManager.Instance.Btn_Reward_Done();
        }

        void ChangeLockState()
        {
            isUnlocked = true;
            SaveState();
            UpdateUI();
        }

        void SaveState()
        {
            PlayerPrefs.SetInt(UnlockKeyPrefix + count, 1);
            PlayerPrefs.Save();
        }

        void LoadState()
        {
            // First 3 unlocked by default
            if (count < 3  || PlayerPrefs.GetInt(WallPaperUnlockedKey)==1)
            {
                isUnlocked = true;
                SaveState(); // save default unlocked state
            }
            else
            {
                isUnlocked = PlayerPrefs.GetInt(UnlockKeyPrefix + count, 0) == 1;
            }
        }

        void UpdateUI()
        {
            if (lockIcon != null)
                lockIcon.SetActive(!isUnlocked);
        }

        public void Unlock()
        {
            isUnlocked = PlayerPrefs.GetInt(UnlockKeyPrefix + count, 0) == 1;
        }
    }
}
