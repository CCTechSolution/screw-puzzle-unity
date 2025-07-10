using UnityEngine;
using UnityEngine.UI;

namespace NultBolts
{
    public class Vibration : MonoBehaviour
    {
        public Button vibrationButton;

        private void Start()
        {
            vibrationButton.onClick.AddListener(Vibrate);
        }

        void Vibrate()
        {
            Debug.Log("vibration should happen");
            Handheld.Vibrate();
        }
    }
}
