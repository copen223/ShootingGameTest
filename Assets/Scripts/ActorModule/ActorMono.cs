using System;
using System.Collections.Generic;
using UnityEngine;

namespace ActorModule
{
    public abstract class ActorMono : MonoBehaviour
    {
        [SerializeField] protected float healPoint;
        [SerializeField] protected float healPoint_Max;
        [SerializeField] protected List<BeHitPoint> beHitPoints = new List<BeHitPoint>();
        
        /// <summary>
        /// 是否无敌
        /// </summary>
        public bool isInvincible;

        private void Awake()
        {
            foreach (var hit in beHitPoints)
            {
                hit.Init(this);
            }
        }

        public abstract void BeHit(DamageInfo info);

        public abstract void Death();
    }
}
