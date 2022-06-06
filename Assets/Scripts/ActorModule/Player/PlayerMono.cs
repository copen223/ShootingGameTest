using System;
using UnityEngine;
using System.Threading.Tasks;
using UI;

namespace ActorModule.Player
{
    public class PlayerMono : ActorMono
    {
        public float SpeedMultiply_Aiming = 0.7f;

        [SerializeField] private float InvincibilityTime_BeHit;
        private float invincibilityTimer;

        [SerializeField] private float behitPower;

        public override void BeHit(DamageInfo info)
        {
            float damage = info.finalDamage;
            
            healPoint -= damage;
            healPoint = healPoint <= 0 ? 0 : healPoint;
            OnHealPointChangeEvent?.Invoke(healPoint/healPoint_Max);

            isInvincible = true;
            KeepInvincible();

            int forceDirX = ((Vector2) transform.position - info.damagePos).x > 0 ? 1 : -1;
            Vector2 force = new Vector2(forceDirX, 1).normalized;
            Debug.Log(force);
            OnBeHitEvent?.Invoke(force * behitPower);

            if(healPoint == 0)
                Death();
        }

        async void KeepInvincible()
        {
            invincibilityTimer = 0;
            while (invincibilityTimer < InvincibilityTime_BeHit)
            {
                invincibilityTimer += Time.deltaTime;   
                await Task.Yield();
            }
            isInvincible = false;
        }

        public override void Death()
        {
            Debug.Log("玩家死亡");
        }

        public event Action<Vector2> OnBeHitEvent;
        public event Action<float> OnHealPointChangeEvent;
    }
}
