using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NultBolts
{
    public class NB_LevelController : LevelController
    {
        protected GameObject currentLevel;
        public DataLevel dataLevel;

        [Space]
        [Header("Editor Levels")]
        [SerializeField] bool isDontDestroyLevel = false;

        public static event Action OnLevelInstantiate;
        //public TextMeshProUGUI LevelText;

        public override void LoadLevel(int level)
        {
            level = GetLevel(level);
            dataLevel = DataManager.levelConfigs.dataLevels[level - 1];

            if (!isDontDestroyLevel)
                if (currentLevel != null)
                {
                    CloseLevel();
                }
            currentLevel = Instantiate(Resources.Load<GameObject>("ScrewNewLevelPrefab/Level " + dataLevel.keyPrefabLevel), this.transform);

            currentLevel.transform.localPosition = new Vector3(0f, -1.35f, 0f);
            currentLevel.gameObject.SetActive(true);
            NB_Timer.Instance.ResumeTime();
            OnLevelInstantiate?.Invoke();
            Debug.Log("Level instantiated");


            NultBoltsManager.Instance.gameState = TypeManager.GameState.Playing;
        }

        int GetLevel(int level)
        {

            return level;

        }
        public override void CloseLevel()
        {
            if (currentLevel)
                Destroy(currentLevel);
        }

        public override void NextLevel()
        {
            base.NextLevel();
        }
    }
}
