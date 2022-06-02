using System.Collections.Generic;
using UnityEngine;

namespace ShootModule.Gun
{
    public abstract class Gun : MonoBehaviour
    {
        //----------数值-----------
        public float BasedDamage;
        
        //----------链接-----------
        [SerializeField]protected List<Ammo> ammoList = new List<Ammo>();
        protected Ammo selectedAmmo;

        //---------行为-----------
        protected abstract void Init();
        protected abstract void Reload(Ammo ammo);
        protected abstract void SelectAmmo(int index);
        public abstract void AimTo(Vector2 targetWorldPosition);
        public abstract void AimJitterReset();
        public abstract void Shoot();
        public abstract void StopAiming();
    }
}
