using System;
using Tool;
using UnityEngine;

namespace AudioModule
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioEffect : TargetInPool
    {
        public AudioSource AudioSource;
        public override void OnReset()
        {
            gameObject.SetActive(true);
        }

        private void Update()
        {
            if(ifPlaying)
                playTime += Time.deltaTime;
            if (playTime >= clipLength)
            {
                OnPlayEndEvent?.Invoke(this);
                ifPlaying = false;
                OnEndUsing();
            }
        }

        private float playTime;
        private float clipLength;
        private bool ifPlaying;
        
        public void Play(AudioClip clip)
        {
            AudioSource.clip = clip;
            AudioSource.Play();
            playTime = 0;
            ifPlaying = true;
            clipLength = AudioSource.clip.length;
        }

        public event Action<AudioEffect> OnPlayEndEvent;
    }
}
