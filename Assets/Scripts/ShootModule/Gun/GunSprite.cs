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
        
        [SerializeField] private float shootOffset;
        [SerializeField] private float speed;

        private Vector2 targetPos;
        private float timerAfterShoot;
        [SerializeField] private float resetPositionTime;

        private void Start()
        {
            timerAfterShoot = resetPositionTime;
        }


        public void SetPositionAndRotationByAim(Vector2 center,Vector2 dir)
        {
            var dis = speed * Time.deltaTime;
            var pos = transform.position;
            
            targetPos = center + aimOffset_Center * dir;

            /*var curDis = Mathf.Lerp(shootOffset, aimOffset_Center, 
                Mathf.Sin(90f * (timerAfterShoot / resetPositionTime)));*/
            var curDis = Mathf.Lerp(shootOffset, aimOffset_Center, 
                timerAfterShoot / resetPositionTime);

            transform.position = center + curDis * dir;
            
            shootFire.transform.position = center + (shootFireOffset_Center + curDis) * dir;
            
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
            timerAfterShoot = resetPositionTime;
            transform.localRotation = Quaternion.Euler(0,0,70f);
        }

        public void ResetTimerAfterShoot()
        {
            timerAfterShoot = 0;
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
            
            timerAfterShoot += Time.deltaTime;
            
            if (timerAfterShoot >= resetPositionTime)
                timerAfterShoot = resetPositionTime;
        }
    }
}
