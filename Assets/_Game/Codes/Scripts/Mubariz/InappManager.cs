using UnityEngine;

namespace NultBolts
{
    public class InappManager : MonoBehaviour
    {
        [SerializeField] GameObject closeButton;

        private void OnEnable()
        {
            Invoke(nameof(EnableCloseButton), 3f);
        }

        void EnableCloseButton()
        {
            closeButton.SetActive(true);
        }
    }
}
