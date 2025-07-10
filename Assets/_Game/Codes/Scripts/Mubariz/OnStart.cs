using UnityEngine;

namespace NultBolts
{
    public class OnStart : MonoBehaviour
    {
        [SerializeField] GameObject gameObjectToEnable;
        [SerializeField] float timeToEnable;

        private void OnEnable()
        {
            Invoke(nameof(On), timeToEnable);
        }

        void On()
        {
            gameObjectToEnable.SetActive(true);
        }
    }
}
