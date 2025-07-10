using UnityEngine;
using UnityEngine.Events;

namespace NultBolts
{
    public class OnEnableDisable : MonoBehaviour
    {
        public UnityEvent OnDisableEvent;
        public UnityEvent OnEnableEvent;

        private void OnEnable()
        {
            OnEnableEvent?.Invoke();
        }

        private void OnDisable()
        {
            OnDisableEvent?.Invoke();
        }
    }
}
