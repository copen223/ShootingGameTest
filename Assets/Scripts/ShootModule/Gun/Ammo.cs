using UnityEngine;

namespace ShootModule.Gun
{
    public abstract class Ammo : MonoBehaviour
    {   
        //---------数值-----------
        [Header("弹药数")]
       [SerializeField] protected int ammoLoad;
       [SerializeField] protected int ammoLoad_Max;
       [SerializeField] protected int ammoReserve;
       [SerializeField] protected int ammoReserve_Max;
        
       //-------行为--------
       public abstract void Reload();
       public abstract Bullet GetBullet();
    }
}