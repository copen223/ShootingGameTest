using UnityEngine;

namespace ShootModule.Gun
{
    public class BulletSprite : MonoBehaviour
    {
        public void SetRotationByDir(Vector2 direction)
        {
            float rotation = Vector2.Angle(Vector2.up, direction);
            if (direction.x > 0)
            {
                rotation = 180f - rotation;
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, -1, 1);
            }

            transform.rotation = Quaternion.Euler(0,0,rotation);
        }
    }
}
