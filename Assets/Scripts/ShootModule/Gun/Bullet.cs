using ActorModule;
using Tool;
using UnityEngine;

namespace ShootModule.Gun
{
    public abstract class Bullet : TargetInPool
    {
        public int DamageCount = 1;
        [SerializeField] protected int damageCountSet = 1;
        public float Powoer;
        public float Damage;
        public DamageInfo.ElementType DamageType;
        protected Gun gun;
        [SerializeField] protected BulletSprite sprite;
        public ActorMono SourceActor => gun.user;

        public abstract void ShootTo(Vector2 direction);
        public abstract void Init(Gun _gun);

        public virtual void SetColor(Color color)
        {
            sprite.SetColor(color);
        }
    }
}
