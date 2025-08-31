//using UnityEngine;

//namespace NultBolts
//{
//    public class FirebaseInit : MonoBehaviour
//    {
//        // Start is called once before the first execution of Update after the MonoBehaviour is created
//        void Start()
//        {

//        }

//        // Update is called once per frame
//        void Update()
//        {

//        }
//    }
//}



using System.Collections;
using UnityEngine;
using Firebase;
using Firebase.Analytics;

public class FirebaseInit : MonoBehaviour
{
    public static FirebaseInit instance;
    void Start()
    {
        Invoke("Start1", 2f);
    }
    void Start1()
    {
        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }



        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Firebase is ready
                Debug.Log("Firebase initialized");

                // Example: log event when app starts
                FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAppOpen);
            }
            else
            {
                Debug.LogError(System.String.Format(
                  "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
            }
        });
    }

    public void LogEvent(string str)
    {
        FirebaseAnalytics.LogEvent(str);
        print("Firebase LogEvent: " + str);
    }




}
