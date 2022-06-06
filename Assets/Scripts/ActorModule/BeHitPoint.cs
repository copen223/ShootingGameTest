using System;
using System.Runtime.InteropServices;
using ShootModule.Gun;
using UnityEngine;
using System.Threading.Tasks;
using ActorModule.Monster;
using AudioModule;
using FloatTextModule;
using Manager;
using ShootModule.Effect;
using Tool;

namespace ActorModule
{
    public class BeHitPoint:MonoBehaviour
    {
        public DamageInfo.ElementType Element;
        public BehitType Type;
        public ActorMono actor { private set; get; }

        [SerializeField] protected AudioController audio;
        [SerializeField] protected BeHitEffect beHitEffectPrefab;
        protected TargetPool<BeHitEffect> beHitEffectPool;
        [SerializeField] protected SpriteRenderer framerRenderer;

        public void Init(ActorMono _actor)
        {
            actor = _actor;
        }

        private void Start()
        {
            framerRenderer.color = GameManager.Instance.GetElementColor(Element);
            originalColor = renderer.color;
            if(beHitEffectPrefab != null)
                beHitEffectPool = new TargetPool<BeHitEffect>(beHitEffectPrefab, transform);
        }
        
        public virtual void BeHit(Bullet hitBullet)
        {
            DamageInfo damageInfo = new DamageInfo(
                hitBullet, this);
            actor?.BeHit(damageInfo);

            //--------反馈---------
            // 产生伤害文字
            // 命中部位受击粒子反馈
            // 闪烁
            bool ifWeakness = Type == BehitType.Weakness;
            bool ifTough = Type == BehitType.Tough;
            int elementWeakness = damageInfo.ifBeHitWeakElement;
            
            if(elementWeakness != -1)
                FlashOnBehit();
            
            if(ifWeakness || elementWeakness == 1)
                audio.Play(1);
            else if(elementWeakness == -1)
                audio.Play(2);
            else if(ifTough)
                audio.Play(2);
            else
            {
                audio.Play(0);
            }

            //Color damageColor = ifWeakness ? Color.red : Color.white;
            
            if(elementWeakness != -1)
                CameraController.MainInstance.Shake(ifWeakness);

            if (ifWeakness || elementWeakness == 1)
            {
                var effect = beHitEffectPool?.GetActiveTarget();
                if (effect != null)
                    effect.transform.position = transform.position + Vector3.back * 5;
            }

            /*FloatTextManager.Instance.CreatDamageFloatTextAt(damageInfo.finalDamage + "",transform.position
            ,damageColor,ifWeakness);*/
        }

        public virtual void BeHit(DamageBody damageBody)
        {
            if(actor.isInvincible)
                return;
            DamageInfo damageInfo = new DamageInfo(damageBody, this);
            actor.BeHit(damageInfo);
            bool ifWeakness = Type == BehitType.Weakness;
            
            FlashOnBehit();
            
            if(ifWeakness)
                audio.Play(1);
            else
                audio.Play(0);
        }
        
        
        //--------受击闪烁---------
        protected Color originalColor;
        [SerializeField] protected Color beHitColor;
        [SerializeField] private float flashTime;
        [SerializeField] protected SpriteRenderer renderer;

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