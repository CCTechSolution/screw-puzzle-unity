using UnityEngine;
using UnityEngine.UI;

namespace NultBolts
{
    public class CoinsManager : MonoBehaviour
    {
        public int defaultCoins = 25;
        public int coins;
        public Text[] allCoinsText;

        public static CoinsManager Instance;

        const string PlayerPrefsKey_Coins = "Coins";
        private void Awake()
        {
            Instance = this;
        }
        private void Start()
        {
            coins = PlayerPrefs.GetInt(PlayerPrefsKey_Coins, defaultCoins);
            UpdateCoins();
        }

        void UpdateCoins()
        {
            foreach (var text in allCoinsText)
            {
                text.text = coins.ToString();
            }
        }

        public void AddCoins(int amount)
        {
            coins += amount;
            PlayerPrefs.SetInt(PlayerPrefsKey_Coins, coins);
            Debug.Log(amount + "Added");
            UpdateCoins();
        }

        public void SpendCoins(int amount)
        {
            coins -= amount;
            PlayerPrefs.SetInt(PlayerPrefsKey_Coins, coins);
            UpdateCoins();
        }
    }
}
