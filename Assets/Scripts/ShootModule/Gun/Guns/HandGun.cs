using AudioModule;
using UnityEngine;

namespace ShootModule.Gun.Guns
{
    public class HandGun : Gun
    {
        [SerializeField] private LineRenderer laserSights;
        private float jitterAngle;  // 当前晃动角度的绝对值
        [SerializeField] private float jitterAngle_Reset;   // 角度晃动大小
        [SerializeField] private float jitterAngle_ResetRandomValue;    // 随机参量
        private float jitterAngle_CurReset; // 随机计算后的当前晃动角度重置值
        private float jitterTime;   // 当前瞄准进行时间，用于插值
        [SerializeField] private float jitterTime_Reset;    // 瞄准晃动时间
        [SerializeField] private float jitterTime_ResetRandomValue; //随机参量
        private float jitterTime_CurReset;  // 随机计算后的当前晃动角度重置值
        private int ifJitterUp = 1;
        private Vector2 curShootDir;

        [SerializeField] private GunSprite sprite;
        [SerializeField] private AudioController audio;
        
        protected override void Init()
        {
            throw new System.NotImplementedException();
        }

        protected override void Reload(Ammo loadAmmo)
        {
            loadAmmo.Reload();   
        }

        protected override void SelectAmmo(int index)
        {
            selectedAmmo = ammoList[index];
        }

        

        public override void AimTo(Vector2 worldPosition)
        {
            jitterTime += Time.deltaTime;
            ifJitterUp *= -1;
            
            jitterAngle = Mathf.LerpAngle(jitterAngle_CurReset, 0, jitterTime / jitterTime_CurReset);
            float curJitterAngle = jitterAngle * ifJitterUp;
            
            var laserStartPoint = transform.position;
            Vector3 referenceDirection = worldPosition - (Vector2)laserStartPoint;
            Vector2 laserDir = (Quaternion.Euler(0, 0, curJitterAngle) * referenceDirection);
            Vector2 laserEndPoint = laserStartPoint + (Vector3) laserDir.normalized * 20f;
            laserSights.positionCount = 2;
            laserSights.SetPositions(new Vector3[]{laserStartPoint,laserEndPoint});
            laserSights.gameObject.SetActive(true);

            curShootDir = laserDir;
            
            sprite.SetPositionAndRotationByAim(transform.position,laserDir.normalized);
        }

        public override void AimJitterReset()
        {
            jitterAngle_CurReset = jitterAngle_Reset +
                                   Random.Range(-jitterAngle_ResetRandomValue, jitterAngle_ResetRandomValue);
            jitterTime = 0;
            jitterTime_CurReset = jitterTime_Reset +
                                  Random.Range(-jitterTime_ResetRandomValue, jitterTime_ResetRandomValue);
        }


        public override void Shoot()
        {
            AimJitterReset();

            var curAmmo = ammoList[0];
            var bullet = curAmmo.GetBullet();
            bullet.Init(this);
            bullet.transform.position = transform.position;
            bullet.ShootTo(curShootDir);
            sprite.ShowShootFire();
            audio.Play(0);
        }

        public override void StopAiming()
        {
            laserSights.positionCount = 0;
            laserSights.gameObject.SetActive(false);
            sprite.ResetPositionIdle();
        }
    }
}
