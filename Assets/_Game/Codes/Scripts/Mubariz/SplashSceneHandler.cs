using UnityEngine;
using UnityEngine.SceneManagement;

namespace NultBolts
{
    public class SplashSceneHandler : MonoBehaviour
    {
        [SerializeField] float timeToLoadNextScene = 8f;
        [SerializeField] float appOpenLoadTime = 3f;
        [SerializeField] float appOpenShowTime = 5.5f;
        [SerializeField] string sceneNameToLoad;
        private void OnEnable()
        {
            Invoke(nameof(LoadAppOpen), appOpenLoadTime);
            Invoke(nameof(ShowAppOpen), appOpenShowTime);
            Invoke(nameof(Load_Scene), timeToLoadNextScene);    
        }

        void Load_Scene()
        {
            SceneManager.LoadScene(sceneNameToLoad);
        }

        void LoadAppOpen()
        {
            AdmobAdsManager.Instance.LoadAppOpenAd();
        }
        void ShowAppOpen()
        {

            AdmobAdsManager.Instance.ShowAppOpenAd();
        }
    }
}
