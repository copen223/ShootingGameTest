using System;
using System.Threading;
using Tool;
using UnityEngine;

namespace AudioModule
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private AudioEffect audioEffectPrefab;
        [SerializeField] private Transform audioEfeectParent;

        private TargetPool<AudioEffect> audioPool;

        public static AudioManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            audioPool = new TargetPool<AudioEffect>(audioEffectPrefab, audioEfeectParent);
        }

        public AudioEffect GetOneAudioEffect()
        {
            return audioPool.GetActiveTarget();
        }
    }
}
