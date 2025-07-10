using System.Collections.Generic;
using UnityEngine;

namespace NultBolts
{
    public class InGameWallPaper : MonoBehaviour
    {
        public GameObject btnInappForWallpaper;
        public static InGameWallPaper Instance;
        public List<WallPaperData> wallPaperDatas = new List<WallPaperData>();
        public GameObject[] allSelectors;
        public GameObject activeSelector;



        private const string unlockKeyPrefix = "Wallpaper_";

        [SerializeField] SpriteRenderer targetSprite;
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            CheckInapp();
            WallPaperData.OnWallpaperClicked += WallPaperData_OnWallpaperClicked;
        }

        private void OnEnable()
        {
            DisableAllSelector();
            activeSelector?.SetActive(true);
        }

        private void WallPaperData_OnWallpaperClicked(object sender, WallPaperData.OnWallpaperclickedEventArgs e)
        {
            targetSprite.sprite = e.sprite;
            activeSelector = e.Selector;
            DisableAllSelector();
        }

        void DisableAllSelector()
        {
            foreach (var selector in allSelectors)
            {
                selector.gameObject.SetActive(false);
            }
        }

        public void UnlockAllWallPapers()
        {
            foreach (var wallpaper in wallPaperDatas)
            {
                wallpaper.Unlock();
            }
            btnInappForWallpaper.SetActive(false);
            PlayerPrefs.SetInt("Wallpaper", 1);
        }

        void CheckInapp()
        {
            if (PlayerPrefs.GetInt("Wallpaper") == 1)
            {
                UnlockAllWallPapers();
                btnInappForWallpaper.SetActive(false);
            }
        }

        
    }
}
