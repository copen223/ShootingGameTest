using System;
using Tool;
using UnityEngine;

namespace ShootModule.Effect
{
    public class BeHitEffect : TargetInPool
    {
        [SerializeField] private ParticleSystem particle;
        public override void OnReset()
        {
            gameObject.SetActive(true);
            particle.Play();
        }

        private void Update()
        {
            if(particle.isStopped)
                OnEndUsing();
        }
    }
}
