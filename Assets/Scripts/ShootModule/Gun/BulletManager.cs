using System;
using ShootModule.Effect;
using Tool;
using UnityEngine;

namespace ShootModule.Gun
{
    public class BulletManager : MonoBehaviour
    {
        [SerializeField] private HitFireEffect hitFireEffectPrefab;
        private TargetPool<HitFireEffect> hitFireEffectPool;

        public static BulletManager Instant;

        private void Awake()
        {
            if (Instant == null)
                Instant = this;
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            hitFireEffectPool = new TargetPool<HitFireEffect>(hitFireEffectPrefab, transform);
        }

        public void CreatHitFireAt(Vector2 position,bool ifToRight)
        {
            var hitFire = hitFireEffectPool.GetActiveTarget();
            hitFire.transform.position = position;
            hitFire.gameObject.SetActive(true);
            if(ifToRight) hitFire.transform.rotation = Quaternion.Euler(0,0,90f);
            else
                hitFire.transform.rotation = Quaternion.Euler(0,0,-90f);
        }
    }
}
