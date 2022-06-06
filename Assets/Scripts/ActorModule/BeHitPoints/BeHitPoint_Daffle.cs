using Manager;
using ShootModule.Effect;
using ShootModule.Gun;
using Tool;
using UnityEngine;

namespace ActorModule.BeHitPoints
{
    public class BeHitPoint_Daffle: BeHitPoint
    {

        [SerializeField] private float HealPoint;
        private void Start()
        {
            framerRenderer.color = GameManager.Instance.GetElementColor(Element);
            originalColor = renderer.color;
            if(beHitEffectPrefab != null)
                beHitEffectPool = new TargetPool<BeHitEffect>(beHitEffectPrefab, transform);
        }

        public override void BeHit(Bullet hitBullet)
        {
           
            DamageInfo damageInfo = new DamageInfo(
                hitBullet, this);
            HealPoint -= damageInfo.finalDamage;
            if(HealPoint<=0)
                gameObject.SetActive(false);

            hitBullet.Damage = 0;
            DamageInfo damageInfo2 = new DamageInfo(
                hitBullet, this);
            actor.BeHit(damageInfo2);
            
            base.BeHit(hitBullet);

        }
    }
}