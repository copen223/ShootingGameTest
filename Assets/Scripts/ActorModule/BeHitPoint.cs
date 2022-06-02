using System;
using ShootModule.Gun;
using UnityEngine;

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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out Bullet hitBullet))
            {
                DamageInfo damageInfo = new DamageInfo(hitBullet.DamageType, hitBullet.Damage, this);
                actor.BeHit(damageInfo);
            }
        }
    }
}