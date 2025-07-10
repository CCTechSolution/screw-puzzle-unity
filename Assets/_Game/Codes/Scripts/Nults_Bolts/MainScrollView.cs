using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace NultBolts
{
    public class MainScrollView : MonoBehaviour
    {
        public ScrollRect scrollRect;

        private void OnEnable()
        {
            Invoke(nameof(SetReact), 0.2f);
        }
        private void SetReact()
        {
            Debug.Log("Latest level is " + NultBoltsManager.Instance.latestLevel);
            if (NultBoltsManager.Instance.latestLevel <= 5)
            {
                Debug.Log("first");
                scrollRect.verticalNormalizedPosition = 0f;
            } 
            else if (NultBoltsManager.Instance.latestLevel <= 10)
            {
                Debug.Log("second");
                scrollRect.verticalNormalizedPosition = 0.05f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 15)
            {
                Debug.Log("third");
                scrollRect.verticalNormalizedPosition = 0.10f;
            
            }else if (NultBoltsManager.Instance.latestLevel <= 20)
            {
                scrollRect.verticalNormalizedPosition = 0.15f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 25)
            {
                scrollRect.verticalNormalizedPosition = 0.2f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 30)
            {
                scrollRect.verticalNormalizedPosition = 0.25f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 35)
            {
                scrollRect.verticalNormalizedPosition = 0.3f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 40)
            {
                scrollRect.verticalNormalizedPosition = 0.35f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 45)
            {
                scrollRect.verticalNormalizedPosition = 0.40f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 50)
            {
                scrollRect.verticalNormalizedPosition = 0.45f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 55)
            {
                scrollRect.verticalNormalizedPosition = 0.5f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 60)
            {
                scrollRect.verticalNormalizedPosition = 0.55f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 65)
            {
                scrollRect.verticalNormalizedPosition = 0.60f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 70)
            {
                scrollRect.verticalNormalizedPosition = 0.65f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 75)
            {
                scrollRect.verticalNormalizedPosition = 0.70f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 80)
            {
                scrollRect.verticalNormalizedPosition = 0.75f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 85)
            {
                scrollRect.verticalNormalizedPosition = 0.80f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 90)
            {
                scrollRect.verticalNormalizedPosition = 0.85f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 95)
            {
                scrollRect.verticalNormalizedPosition = 0.90f;
            }
            else if (NultBoltsManager.Instance.latestLevel <= 100)
            {
                scrollRect.verticalNormalizedPosition = 0.95f;
            }
            
        }

        
    }
}
