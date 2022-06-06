using System;
using System.Collections.Generic;
using Manager;
using UnityEngine;

namespace ActorModule.Monster
{
    public class MonsterMono:ActorMono
    {
        public DamageInfo.ElementType deathType;
        private bool ifBeWeakNeesKill;

        private void Start()
        {
            foreach (var hit in beHitPoints)
            {
                hit.Init(this);
            }
        }

        public override void BeHit(DamageInfo info)
        {
            float damage = info.finalDamage;

            healPoint -= damage;
            healPoint = healPoint <= 0 ? 0 : healPoint;
            OnBeHitEvent?.Invoke(info);

            if (healPoint == 0)
            {
                deathType = info.beHitPoint.Element;
                if (info.ifBeHitWeakElement == 1)
                    ifBeWeakNeesKill = true;
                if (info.beHitPoint.Type == BeHitPoint.BehitType.Weakness)
                    ifBeWeakNeesKill = true;
                Death();
            }
        }

        public event Action<DamageInfo> OnBeHitEvent;

        public override void Death()
        {
            if(ifBeWeakNeesKill)
                GameManager.Instance.CreatAmmoAt(deathType,transform.position);
            Invoke(nameof(DestroyThis),0.5f);
        }

        private void DestroyThis()
        {
            Destroy(gameObject);
        }
    }
}