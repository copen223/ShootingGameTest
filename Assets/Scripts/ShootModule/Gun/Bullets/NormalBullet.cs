using System;
using ShootModule.Gun.Guns;
using UnityEngine;

namespace ShootModule.Gun.Bullets
{
    public class NormalBullet:Bullet
    {
        [SerializeField] Rigidbody2D rigidbody;
        
        [SerializeField] private float shootImpulse;
        [SerializeField] private float maxTime_Existence;
        private float time_Existence = 0;

        private void Update()
        {
            time_Existence += Time.deltaTime;
            if(time_Existence >= maxTime_Existence)
               OnEndUsing();
        }

        public override void Init(Gun _gun)
        {
            gun = _gun;
            Damage = gun.BasedDamage;
        }

        public override void OnReset()
        {
            time_Existence = 0;
            gameObject.SetActive(true);
        }

        public override void ShootTo(Vector2 direction)
        {
            rigidbody.AddForce(direction.normalized * shootImpulse,ForceMode2D.Impulse);
            sprite.SetRotationByDir(direction);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.CompareTag("Ground"))
            {
                OnEndUsing();
            }
        }
    }
}