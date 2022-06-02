using ActorModule;
using Tool;
using UnityEngine;

namespace ShootModule.Gun
{
    public abstract class Bullet : TargetInPool
    {
        public float Damage;
        public DamageInfo.ElementType DamageType;
        protected Gun gun;
        [SerializeField] protected BulletSprite sprite;
        
        public abstract void ShootTo(Vector2 direction);
        public abstract void Init(Gun _gun);
    }
}
