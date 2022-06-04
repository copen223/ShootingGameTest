using System.Collections.Generic;
using UnityEngine;

namespace AudioModule
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private List<AudioClip> audioClips;

        public void Play(int index)
        {
            var clip = audioClips[index];

            var audio = AudioManager.Instance.GetOneAudioEffect();
            audio.transform.parent = transform;
            audio.transform.localPosition = Vector2.zero;
            audio.Play(clip);
            
        }
    }
}
