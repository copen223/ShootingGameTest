using System;
using ShootModule.Gun;
using UnityEngine;
using System.Threading.Tasks;

namespace ActorModule
{
    public class BeHitPoint:MonoBehaviour
    {
        public DamageInfo.ElementType Element;
        public BehitType Type;
        private ActorMono actor;

        public void Init(ActorMono _actor)
        {
            actor = _actor;
        }

        private void Start()
        {
            originalColor = renderer.color;
        }
        
        public void BeHit(Bullet hitBullet)
        {
            DamageInfo damageInfo = new DamageInfo(
                hitBullet, this);
            actor.BeHit(damageInfo);
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
            await Task.Delay((int)(time * 1000));
        }
        
        
        public enum BehitType
        {
            Normal,
            Weakness,
            Tough
        }
        
    }
}