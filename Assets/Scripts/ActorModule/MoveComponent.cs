using System;
using System.Collections.Generic;
using System.Threading;
using UnityEditor;
using UnityEngine;
using System.Threading.Tasks;

namespace ActorModule
{
    public class MoveComponent : MonoBehaviour
    {
        //---------移速-----------
        public Vector2 Velocity => rigidbody.velocity; 
        [SerializeField] private float moveSpeed;
        private float moveSpeedMultiply = 1;  // 移速乘值
        private float MoveSpeed_Final => moveSpeed * moveSpeedMultiply;

        [SerializeField]private float jumpSpeed;
        public float jumpSpeedMultiply = 1;
        private float JumpSpeed_Final => moveSpeed * jumpSpeedMultiply;
        
        [SerializeField]private float acceleration = 2; // 加速度
        [SerializeField] private float deceleration;
        
        //--------站立检测--------
        [SerializeField] private Vector2 StandBox = new Vector2(1.2f,0.5f);
        [SerializeField] private Vector2 WillStandBox = new Vector2(1.2f,0.5f);
        [SerializeField] private float StandDis = 1f;
        [SerializeField] private float WillStandDis = 1.2f;
        
        
        /// <summary>
        /// 是否处于站立情况
        /// </summary>
        public bool IfStanding;
        /// <summary>
        /// 是否将要处于站立，跳跃优化用
        /// </summary>
        public bool IfWillStand;
        
        //--------挂载对象---------
        private Rigidbody2D rigidbody;
        
        public void Init(Rigidbody2D _rigidbody)
        {
            rigidbody = _rigidbody;
        }

        private void Update()
        {
            CheckIfStanding();
            CheckIfWillStand();
            vel = rigidbody.velocity;
        }

        public Vector2 vel;
        
        //---------射线检测----------
        /// <summary>
        /// 检测是否站立
        /// </summary>
        private void CheckIfStanding()
        {
            var hits = Physics2D.BoxCastAll(transform.position, StandBox,
                0, Vector2.down, StandDis);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    IfStanding = true;
                    return;
                }
            }

            IfStanding = false;
        }
        /// <summary>
        /// 检测是否将要处于站立
        /// </summary>
        private void CheckIfWillStand()
        {
            var hits = Physics2D.BoxCastAll(transform.position, WillStandBox,
                0, Vector2.down, WillStandDis);
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    IfWillStand = true;
                    return;
                }
            }

            IfWillStand = false;
        }
        
        
        
        /// <summary>
        /// 改变移动时的速度
        /// </summary>
        /// <param name="speed"></param>
        public void ChangeMoveSpeed(float speed)
        {
            moveSpeed = speed;
        }
        /// <summary>
        /// 速度乘数
        /// </summary>
        /// <param name="speed"></param>
        public void ChangeMoveSpeedMultiply(float multiply)
        {
            moveSpeedMultiply = multiply;
        }

        public void StartWalk(int directionIsRight)
        {
            float velocityX = directionIsRight * MoveSpeed_Final;
            Vector3 velocity = rigidbody.velocity;
            
            moveTime = (velocityX - velocity.x) / acceleration;
            moveTimer = 0;
            targetVelocity_X = velocityX;
            startVelocity_X = velocity.x;
        }

        private float moveTimer = 0;
        private float moveTime = 2f;
        private float targetVelocity_X = 0;
        private float startVelocity_X = 0;
        
        public void Walk()
        {
            moveTimer += Time.deltaTime;
            
            var velecity = rigidbody.velocity;
            float t = Mathf.Abs(moveTimer / moveTime);
            t = t > 1 ? 1 : t;
            
            float velX = Mathf.Lerp(startVelocity_X, targetVelocity_X, Mathf.Sin(t * 90f * Mathf.Deg2Rad));
            rigidbody.velocity = new Vector2(velX, rigidbody.velocity.y);
        }

        public async void StopWalk()
        {
            Vector3 velocity = rigidbody.velocity;
            
            moveTime = Mathf.Abs((0 - velocity.x) / deceleration);
            moveTimer = 0;
            targetVelocity_X = 0;
            startVelocity_X = velocity.x;
        }

        public void Jump()
        {
            Vector3 velocity = rigidbody.velocity;
            rigidbody.velocity = new Vector2(velocity.x,jumpSpeed);
        }

        private bool isAwaitingTask = false;

        private void CancelTask()
        {
            if (isAwaitingTask)
            {
                CancellationTokenSource canceleTokenSource = new CancellationTokenSource();
                canceleTokenSource.Cancel();
            }
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Vector2 pos1 = (Vector2) transform.position + Vector2.down * StandDis;
            Gizmos.DrawWireCube(pos1,StandBox);
            
            Gizmos.color = Color.green;
            Vector2 pos2 = (Vector2) transform.position + Vector2.down * WillStandDis;
            Gizmos.DrawWireCube(pos2,WillStandBox);
        }
    }
}
