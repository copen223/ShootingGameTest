using System;
using System.Collections.Generic;
using ActorModule;
using Manager;
using Tool;
using UnityEngine;

namespace ShootModule.Gun.Ammos
{
    public class NormalAmmo : Ammo
    {
        //-------------链接--------
        [SerializeField] private Transform bulletParent;
        [SerializeField] private Bullet bulletPrefab;
        private TargetPool<Bullet> bulletPool;
        [SerializeField] private DamageInfo.ElementType elementType;
        private void Start()
        {
            bulletPool = new TargetPool<Bullet>(bulletPrefab,bulletParent);
        }

        public override void Reload()
        {
            int suplusAmmo = ammoLoad_Max - ammoLoad;

            if (ammoReserve >= suplusAmmo)
            {
                ammoReserve -= suplusAmmo;
                ammoLoad = ammoLoad_Max;
            }
            else
            {
                ammoLoad += ammoReserve;
                ammoReserve = 0;
            }
        }
        
        //-----------生产--------------

        /// <summary>
        /// 生成并获取一个子弹物体
        /// </summary>
        /// <returns></returns>
        public override Bullet GetBullet()
        {
            if (ammoLoad > 0)
            {
                ammoLoad--;
            }
            else
            {
                Reload();
                if(ammoLoad >0)
                    ammoLoad--;
                else
                {
                    return null;
                }
            }
            
            var bullet = bulletPool.GetActiveTarget();
            bullet.gameObject.SetActive(true);
            bullet.DamageType = elementType;
            bullet.SetColor(GameManager.Instance.GetElementColor(elementType));
            return bullet;
        }
    }
}