using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActorModule.Monster
{
    public class MonsterMono:ActorMono
    {
        public override void BeHit(DamageInfo info)
        {
            float damage = 0;
            List<float> muls = info.GetDamageMultipliesByElementCheck();
            
            int i = 0;
            foreach (var beHitPoint in info.BeHitPoints)
            {
                float damage_Point = info.damage * muls[i];
                i++;
                
                // 产生伤害文字
                damage += damage_Point;
            }

            healPoint -= damage;
            healPoint = healPoint <= 0 ? 0 : healPoint;
            
            if(healPoint == 0)
                Death();
        }

        public override void Death()
        {
            Destroy(gameObject);
        }
    }
}