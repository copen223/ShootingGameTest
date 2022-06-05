using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActorModule.Monster
{
    public class MonsterMono:ActorMono
    {
        public override void BeHit(DamageInfo info)
        {
            float damage = info.finalDamage;
            
            healPoint -= damage;
            healPoint = healPoint <= 0 ? 0 : healPoint;
            OnBeHitEvent?.Invoke(info);
            
            if(healPoint == 0)
                Death();
        }

        public event Action<DamageInfo> OnBeHitEvent;

        public override void Death()
        {
            Destroy(gameObject);
        }
    }
}