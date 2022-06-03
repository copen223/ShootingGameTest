using System;
using ActorModule;
using ShootModule.Gun.Guns;
using UnityEngine;

namespace ShootModule.Gun.Bullets
{
    public class NormalBullet:Bullet
    {
        [SerializeField] Rigidbody2D rigidbody;
        [SerializeField] private CapsuleCollider2D collider;
        
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
            DamageCount = damageCountSet;
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

            if (col.TryGetComponent(out BeHitPoint hitTarget))
            {
                var startPos = transform.position;
                var endPos = col.transform.position;
                var dis = Vector2.Distance(startPos, endPos);
                var dir = (endPos - startPos).normalized;
                
                var hits = Physics2D.CapsuleCastAll(startPos, collider.size,
                    CapsuleDirection2D.Vertical,transform.rotation.eulerAngles.z, dir, dis);
                foreach (var hit in hits)
                {
                    if (hit.collider.TryGetComponent(out BeHitPoint beHitPoint))
                    {
                        if (DamageCount <= 0)
                        {
                            OnEndUsing();
                            return;
                        }

                        DamageCount -= 1;
                        beHitPoint.BeHit(this);
                    }
                    
                }
            }
        }
    }
}