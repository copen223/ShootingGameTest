using System;
using UnityEngine;

namespace ShootModule.Gun
{
    public class GunSprite : MonoBehaviour
    {
        [SerializeField] private float aimOffset_Center;
        [SerializeField] private Vector2 idleOffset;
        [SerializeField] private GameObject shootFire;
        [SerializeField] private float shootFireShowTime;
        
        [SerializeField] private float shootFireOffset_Center;
        
        public void SetPositionAndRotationByAim(Vector2 center,Vector2 dir)
        {
            transform.position = center + aimOffset_Center * dir;
            shootFire.transform.position = center + shootFireOffset_Center * dir;
            
            float rotation = Vector2.Angle(Vector2.right, dir);
            if (dir.y <= 0)
                rotation = 180f - rotation;
            transform.rotation = Quaternion.Euler(0,0,rotation);
            shootFire.transform.rotation = Quaternion.Euler(0,0,rotation);
        }
        
        /// <summary>
        /// idle状态下的偏移
        /// </summary>
        public void ResetPositionIdle()
        {
            transform.localPosition = idleOffset;
            transform.localRotation = Quaternion.Euler(0,0,70f);
        }

        private float shootFireTimer;
        public void ShowShootFire()
        {
            shootFire.SetActive(true);
            shootFireTimer += shootFireShowTime;
        }

        private void Update()
        {
            if (shootFireTimer > 0)
            {
                shootFireTimer -= Time.deltaTime;
                if (shootFireTimer <= 0)
                {
                    shootFireTimer = 0;
                    shootFire.SetActive(false);
                }
            }
        }
    }
}
