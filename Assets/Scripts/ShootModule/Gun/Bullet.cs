using Tool;
using UnityEngine;

namespace ShootModule.Gun
{
    public abstract class Bullet : TargetInPool
    {
        public abstract void ShootTo(Vector2 direction);
    }
}
