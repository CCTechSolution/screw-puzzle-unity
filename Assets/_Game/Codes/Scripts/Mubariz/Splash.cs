using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace NultBolts
{
    public class Splash : MonoBehaviour
    {
        private AsyncOperation loading;

        // Start is called before the first frame update

        void Start()
        {

            //StartCoroutine(LoadNextScene());
        }



        //AD OPEN HERE
        //public IEnumerator LoadNextScene() {
        //    yield return new WaitUntil(() =>AdsManagerAdmob.Instance.OpenAppShownInSession);

        //    loading = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
        //}
    }
}
