using UnityEngine;
using UnityEngine.Events;

namespace NultBolts
{
    public class BoardScrewHole : MonoBehaviour, IHole
    {
        [SerializeField] private GameObject adIcon;
        [SerializeField] private GameObject lockObj;
        [SerializeField] private bool hasCrew;

        public const bool lockScrewOnly = false;

        public bool isLockAll { get; set; }
        public bool isLocked;
        public bool adsLocked;

        public bool CanPin => !hasCrew;

        public UnityEvent OnClick;


        private void OnEnable()
        {
            adIcon.SetActive(adsLocked);
            lockObj.SetActive(isLocked);
            if (adsLocked)
            {
                RedColor();
            }

            //if (adsLocked)
            //{
            //    UnlockAds();
            //}
        }

        public void Pin(Screw screw)
        {
            hasCrew = true;
            OnClick?.Invoke();
            Debug.Log("Screw Inserted");

            if (NultBoltsManager.Instance.CanVibrate)
            {
                Handheld.Vibrate();
                Debug.Log("Mobile Vibrated");
            }
        }

        public void UnPin()
        {
            hasCrew = false;
        }

        public void UnlockAds()
        {
            adsLocked = false;
            adIcon.SetActive(false);
            TransparentColor();
        }

        public void Unlock()
        {
            isLocked = false;
            lockObj.SetActive(false);
        }

        private void OnMouseDown()
        {
            Debug.Log("i click on hole");
            if (adsLocked)
            {
                RewardLoading.Instance.StartLoading(UnlockAds);
            }
        }

        void RedColor()
        {
            Color redColor;
            if (ColorUtility.TryParseHtmlString("#FF0000", out redColor))
            {
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.color = redColor;
                }
                else
                {
                    Debug.LogWarning("SpriteRenderer not found on: " + gameObject.name);
                }
            }
        }
        void TransparentColor()
        {
            Debug.Log("entered");
            Color whiteColor;
            if (ColorUtility.TryParseHtmlString("#FFFFFF", out whiteColor))
            {
                SpriteRenderer sr = GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    Debug.Log("color changed to whtie");
                    sr.color = whiteColor;
                }
                else
                {
                    Debug.LogWarning("SpriteRenderer not found on: " + gameObject.name);
                }
            }
        }
    }
}
