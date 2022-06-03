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
                // 命中部位受击粒子反馈
                // 闪烁
                beHitPoint.FlashOnBehit();
                switch (beHitPoint.Type)
                {
                    case BeHitPoint.BehitType.Normal:
                        break;
                    case BeHitPoint.BehitType.Tough:
                        damage_Point /= 4;
                        break;
                    case BeHitPoint.BehitType.Weakness:
                        damage_Point *= 4;
                        break;
                }
                
                damage += damage_Point;
            }

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