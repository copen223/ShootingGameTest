using UnityEngine;

namespace ActorModule
{
    public class MoveComponent : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        private Rigidbody2D rigidbody;
        
        public void Init(Rigidbody2D _rigidbody)
        {
            rigidbody = _rigidbody;
        }
        
        
        
        /// <summary>
        /// 改变移动时的速度
        /// </summary>
        /// <param name="speed"></param>
        public void ChangeMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }

        public void Walk(int directionIsRight)
        {
            rigidbody.velocity = Vector2.right * directionIsRight * moveSpeed;
        }
        public void StartWalk(int directionIsRight,float speed)
        {
            rigidbody.velocity = Vector2.right * directionIsRight * moveSpeed * speed;
        }

        public void StopWalk()
        {
            rigidbody.velocity = Vector2.zero;
        }
    }
}
