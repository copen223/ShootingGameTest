using UnityEngine;

namespace ShootModule.Gun
{
    public abstract class Ammo : MonoBehaviour
    {   
        //---------数值-----------
        [Header("弹药数")] 
        public int ammoLoad; 
        [SerializeField] protected int ammoLoad_Max; 
        public int ammoReserve; 
        [SerializeField] protected int ammoReserve_Max;
        
       //-------行为--------
       public abstract void Reload();
       public abstract Bullet GetBullet();
    }
}