using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace NultBolts
{
    public class AudioManager : SingletonMonoBehaviour<AudioManager>
    {
        [Space(5)]
        [Header("Sound Buttons")]
        public Button soundButton;
        public Button soundButton2;
        public Button soundButton3;

        [Space(5)]
        [Header("Visuals")]
        public Image soundImgae;
        public Image soundImgae2;
        public Image soundImgae3;

        [Space(5)]
        [Header("On Off Sprites")]
        public Sprite soundOnSprite;
        public Sprite soundOffSprite;
        private bool MusicEnable => true;
        private bool SoundEnable => true;

        public bool soundPlaying;

        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource soundSource;


        private Dictionary<Audio, AudioClip> dicAudio = new Dictionary<Audio, AudioClip>();

        private static List<Audio> hitAudios = new List<Audio> { Audio.Hit1, Audio.Hit2, Audio.Hit3 };
        public override void Awake()
        {
            AddClip();
            base.Awake();

            if (soundPlaying)
            {
                musicSource.mute = false;
                musicSource.Play();
            }
               
        }
        private void Start()
        {
            soundPlaying = true;
            soundImgae.sprite = soundOnSprite;
            soundImgae2.sprite = soundOnSprite;
            soundImgae3.sprite = soundOnSprite;
            soundButton.onClick.AddListener(SoundButton);
            soundButton2.onClick.AddListener(SoundButton);
            soundButton3.onClick.AddListener(SoundButton);
        }
        private void AddClip()
        {
            dicAudio.Add(Audio.Music, Resources.Load<AudioClip>("Audio/Musics/Music"));
            dicAudio.Add(Audio.PopupShow, Resources.Load<AudioClip>("Audio/Screw/PopupShow"));
            dicAudio.Add(Audio.Button, Resources.Load<AudioClip>("Audio/Screw/Button"));
            dicAudio.Add(Audio.Bomb, Resources.Load<AudioClip>("Audio/Screw/bomb"));
            dicAudio.Add(Audio.Hit1, Resources.Load<AudioClip>("Audio/Screw/hit1"));
            dicAudio.Add(Audio.Hit2, Resources.Load<AudioClip>("Audio/Screw/hit2"));
            dicAudio.Add(Audio.Hit3, Resources.Load<AudioClip>("Audio/Screw/hit3"));
            dicAudio.Add(Audio.OnWin, Resources.Load<AudioClip>("Audio/Screw/OnWin"));
            dicAudio.Add(Audio.Pin, Resources.Load<AudioClip>("Audio/Screw/Pin"));
            dicAudio.Add(Audio.Unpin, Resources.Load<AudioClip>("Audio/Screw/UnPin"));
            dicAudio.Add(Audio.Unlock, Resources.Load<AudioClip>("Audio/Screw/Unlock"));
            dicAudio.Add(Audio.Sawing, Resources.Load<AudioClip>("Audio/Screw/Sawing"));
            dicAudio.Add(Audio.GetReward, Resources.Load<AudioClip>("Audio/Screw/GetReward"));
            dicAudio.Add(Audio.GetItem, Resources.Load<AudioClip>("Audio/Screw/GetItem"));
            dicAudio.Add(Audio.Progress, Resources.Load<AudioClip>("Audio/Screw/Progress"));
            dicAudio.Add(Audio.CollectCoin, Resources.Load<AudioClip>("Audio/Screw/04_Collect"));
            dicAudio.Add(Audio.Tap, Resources.Load<AudioClip>("Audio/Screw/tap"));
            dicAudio.Add(Audio.AddCoin, Resources.Load<AudioClip>("Audio/Screw/AddCoin"));
            dicAudio.Add(Audio.UpdateCoin, Resources.Load<AudioClip>("Audio/Screw/UpdateCoin"));
        }

        private void Update()
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    PlayMusic(Audio.Music);
            //}
        }

        public void PlaySound(Audio audio)
        {
            if (soundPlaying)
            {
                if (SoundEnable) soundSource.PlayOneShot(dicAudio[audio], 1);
            }
        }

        public void PlaySound(Audio audio, float vol)
        {
            if (soundPlaying)
            {
                if (SoundEnable) soundSource.PlayOneShot(dicAudio[audio], vol);
            }
        }

        public void PlaySound(Audio audio, bool loop = false)
        {
            if (SoundEnable)
            {
                soundSource.clip = dicAudio[audio];
                soundSource.loop = loop;
                soundSource.Play();
            }
        }

        public void PlayMusic(Audio audio, bool loop = true)
        {
            //float vol = MusicEnable ? .5f : 0;
            //musicSource.volume = vol;
            //if (musicSource.clip == dicAudio[audio] && loop == musicSource.loop)
            //{
            //    return;
            //}
            //musicSource.clip = dicAudio[audio];
            //musicSource.loop = loop;
            //musicSource.Play();
        }

        public void EnableMusic()
        {
            musicSource.volume = MusicEnable ? 1 : 0;
        }
        public static Audio RandomHitPlateAudio()
        {
            return hitAudios[Random.Range(0, hitAudios.Count)];
        }

        public void StopSound() => soundSource.Stop();

        public void PauseAudio() => AudioListener.pause = true;
        public void ResumeAudio() => AudioListener.pause = false;


        void SoundButton()
        {
            if (soundPlaying)
            {
                PauseAudio();
                soundPlaying = false;
                soundImgae.sprite = soundOffSprite;
                soundImgae2.sprite = soundOffSprite;
                musicSource.mute = true;
                musicSource.Pause();
            }
            else
            {
                ResumeAudio();
                soundPlaying = true;
                soundImgae.sprite = soundOnSprite;
                soundImgae2.sprite = soundOnSprite;
                musicSource.mute = false;
                musicSource.Play();
            }
        }
    }

    public enum Audio
    {
        Music,
        PopupShow,
        Button,
        Bomb,
        Hit1,
        Hit2,
        Hit3,
        OnWin,
        Pin,
        Unpin,
        Unlock,
        Sawing,
        GetReward,
        GetItem,
        Progress,
        CollectCoin,
        Button2,
        Tap,
        AddCoin,
        UpdateCoin
    }

   
}