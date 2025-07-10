using System.Collections.Generic;
using UnityEngine;

namespace NultBolts
{
    public class DuringLevelReward : MonoBehaviour
    {
        public List<Screw> screws = new List<Screw>();
        bool unpin;

        private void OnEnable()
        {
            Check();
            

            if (AdmobAdsManager.Instance)
            {
                AdmobAdsManager.Instance.LoadRewardedVideo();
            }
        }

        void Check()
        {
            // Find all objects in the scene with the BoardScrewHole script attached
            Screw[] screwHoles = FindObjectsOfType<Screw>();

            foreach (Screw littleScrew in screwHoles)
            {

                // Check if the adsLocked boolean is true
                if (littleScrew)
                {
                    screws.Add(littleScrew);
                    littleScrew.UnPin();
                   
                }
            }
        }

        public void UnPin()
        {
            unpin = true;
        }

        private void OnDisable()
        {
            screws?.Clear();
        }

       

    }
}
