using UnityEngine;
using UnityEngine.UI;

namespace NultBolts
{
    public class VibrationManager : MonoBehaviour
    {
        public Button vibrationButton;
        public Button vibrationButton2;

        public Image vibrationImage;
        public Image vibrationImage2;

        public Sprite onSprite;
        public Sprite offSprite;


        private void Start()
        {
            NultBoltsManager.Instance.CanVibrate = true;
            vibrationButton.onClick.AddListener(Toggle);
            vibrationButton2.onClick.AddListener(Toggle);
        }

        void Toggle()
        {
            if (NultBoltsManager.Instance.CanVibrate)
            {
                vibrationImage.sprite = offSprite;
                vibrationImage2.sprite = offSprite;
                NultBoltsManager.Instance.CanVibrate = false;
            }
            else
            {
                vibrationImage.sprite = onSprite;
                vibrationImage2.sprite = onSprite;
                NultBoltsManager.Instance.CanVibrate = true;
            }
        }
    }
}
