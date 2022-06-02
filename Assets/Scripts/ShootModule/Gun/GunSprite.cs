using UnityEngine;

namespace ShootModule.Gun
{
    public class GunSprite : MonoBehaviour
    {
        [SerializeField] private float aimOffset_Center;
        [SerializeField] private Vector2 idleOffset;
        public void SetPositionAndRotationByAim(Vector2 center,Vector2 dir)
        {
            transform.position = center + aimOffset_Center * dir;
            float rotation = Vector2.Angle(Vector2.right, dir);
            if (dir.y <= 0)
                rotation = 180f - rotation;
            transform.rotation = Quaternion.Euler(0,0,rotation);
        }
        
        /// <summary>
        /// idle状态下的偏移
        /// </summary>
        public void ResetPositionIdle()
        {
            transform.localPosition = idleOffset;
            transform.localRotation = Quaternion.Euler(0,0,70f);
        }
     
    }
}
