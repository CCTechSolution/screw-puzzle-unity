using Unity.VisualScripting;
using UnityEngine;

namespace NultBolts
{
    public class NB_LevelLoader : LevelController
    {
        public static NB_LevelLoader Instance;

        [SerializeField] LevelButton[] levelButton;

        public bool allLevelsUnlocked;

        public GameObject btnAllLevelsUnlockInapp;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (PlayerPrefs.GetInt("AllLevels") == 1)
            {
                UnlockAllLevels();
                btnAllLevelsUnlockInapp.SetActive(false);
            }
        }

        public void UnlockAllLevels()
        {
            foreach (var level in levelButton)
            {
                level.isUnlocked = true;
            }
        }
    }
}
