using System;
using System.Threading.Tasks;
using ActorModule;
using ShootModule.Effect;
using ShootModule.Gun.Guns;
using Tool;
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

        [SerializeField] private int suspendCount;
        [SerializeField] private float suspendScale;

        private Vector2 shootDir;

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
            shootDir = direction.normalized;
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            var endPos = col.transform.position;
            // var dis = Vector2.Distance(startPos, endPos);
            float dis = rigidbody.velocity.magnitude * Time.deltaTime;
            Vector2 startPos = (Vector2)transform.position - dis * shootDir;
            var dir = shootDir;
            
            /*var rayHits = Physics2D.RaycastAll(startPos, dir, dis);
            Vector2 hitPoint = Vector2.zero;
            foreach (var hit in rayHits)
            {
                if (hit.collider != collider)
                {
                    hitPoint = hit.point;
                    break;
                }
            }*/

            if (col.CompareTag("Ground"))
            {
                var hits = Physics2D.CapsuleCastAll(startPos, collider.size,
                    CapsuleDirection2D.Vertical,transform.rotation.eulerAngles.z, dir, dis * 2f);

                BulletManager.Instant.CreatHitFireAt(hits[0].point,dir.x < 0);
                OnEndUsing();
                return;
            }

            if (col.TryGetComponent(out BeHitPoint hitTarget))
            {
                var hits = Physics2D.CapsuleCastAll(startPos, collider.size,
                    CapsuleDirection2D.Vertical,transform.rotation.eulerAngles.z, dir, dis * 2f);

                foreach (var hit in hits)
                {
                    if (hit.collider.TryGetComponent(out BeHitPoint beHitPoint))
                    {
                        if (DamageCount <= 0)
                        {
                            OnEndUsing();
                            return;
                        }
                        if(beHitPoint.actor == SourceActor)
                            continue;
                        
                        
                        beHitPoint.BeHit(this);
                        DamageCount -= 1;

                        Time.timeScale = suspendScale;
                        TimeScaleReset();
                        
                        if (DamageCount <= 0)
                        {
                            BulletManager.Instant.CreatHitFireAt(hit.point,dir.x < 0);
                            OnEndUsing();
                            return;
                        }
                        
                    }
                    
                }
            }
        }

        private async void TimeScaleReset()
        {
            int count = 0;
            while (count < suspendCount)
            {
                await Task.Delay(10);
                count++;
            }
            Time.timeScale = 1;
        }
    }
}