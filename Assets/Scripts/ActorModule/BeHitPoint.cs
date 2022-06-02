using System;
using ShootModule.Gun;
using UnityEngine;
using System.Threading.Tasks;

namespace ActorModule
{
    public class BeHitPoint:MonoBehaviour
    {
        public DamageInfo.ElementType Element;
        private ActorMono actor;

        public void Init(ActorMono _actor)
        {
            actor = _actor;
        }

        private void Start()
        {
            originalColor = renderer.color;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Bullet hitBullet))
            {
                DamageInfo damageInfo = new DamageInfo(hitBullet.DamageType, hitBullet.Damage, this);
                actor.BeHit(damageInfo);
            }
        }
        
        //--------受击闪烁---------
        private Color originalColor;
        [SerializeField] private Color beHitColor;
        [SerializeField] private float flashTime;
        [SerializeField] private SpriteRenderer renderer;

        private bool ifFlashing = false;
        public async void FlashOnBehit()
        {
            if(ifFlashing)
                return;
            ifFlashing = true;
            renderer.color = beHitColor;
            await WaitTime(flashTime);
            renderer.color = originalColor;
            ifFlashing = false;
        }

        private async Task WaitTime(float time)
        {
            Debug.Log("dsf");
            await Task.Delay((int)(time * 1000));
        }
        
        
    }
}