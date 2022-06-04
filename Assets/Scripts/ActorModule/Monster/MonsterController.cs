using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ActorModule.Player;
using UnityEngine;

namespace ActorModule.Monster
{
    public class MonsterController : MonoBehaviour
    {
        public Vector2 Vel_Debug;
        
        [SerializeField] private List<Transform> patrolPositions;
        [SerializeField] private float arriveThreshold;
        [SerializeField] private float ildeWaitTime;
        [SerializeField] private float patrolRestTime;
        [SerializeField] private float moveSpeed;
        [SerializeField] private float rayDistance;
        [SerializeField] private float playerLeaveDisX;

        private float stiffTime;
        private float beatBackPower;
        private Vector2 beatBackDir;
        [SerializeField] private float deceleration;

        private Vector2 ViewDirection
        {
            get
            {
                return viewDirection;
            }
            set
            {
                viewDirection = value;
                int scaleX = viewDirection.x > 0 ? 1 : -1;
                var localScale = transform.localScale;
                localScale = new Vector3(scaleX, localScale.y, localScale.z);
                transform.localScale = localScale;
            }
        }

        private Vector2 viewDirection;
        
        private PlayerMono player_Target;

        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private MonsterMono monster;

        private float timer = 0;    // 计时器

        private void Start()
        {
            monster.OnBeHitEvent += OnBeHitCallBack;
        }

        private void Update()
        {
            StateUpdate();
            jumpTimer -= Time.deltaTime;

            Vel_Debug = rigidbody.velocity;
        }

        #region 状态机

        


        private void StateUpdate()
        {
            switch (currentState)
            {
                case MonsterState.Chase:
                    MonsterChaseUpdate();
                    break;
                case MonsterState.Idle:
                    MonsterIdleUpdate();
                    break;
                case MonsterState.Patrol:
                    break;
                case MonsterState.Stiff:
                    MonsterStiffUpdate();
                    break;
            }
        }

        private bool CheckViewTarget()
        {
            if (currentState != MonsterState.Chase)
            {
                var hits = Physics2D.RaycastAll(transform.position, ViewDirection, rayDistance);
                foreach (var hit in hits)
                {
                    // Debug.Log(hit.collider);
                    if (hit.collider.TryGetComponent(out PlayerHit player))
                    {
                        player_Target = player.player;
                        return true;
                    }
                }
            }

            return false;
        }
        
        /// <summary>
        /// 玩家不在追踪范围内返回false
        /// </summary>
        /// <returns></returns>
        private bool CheckViewTargetLeave()
        {
            /*var hits = Physics2D.RaycastAll(transform.position, viewDirection, rayDistance);
            foreach (var hit in hits)
            {
                // Debug.Log(hit.collider);
                if (hit.collider.TryGetComponent(out PlayerHit player))
                {
                    player_Target = player.player;
                    return false;
                }
            }*/
            float disX = Mathf.Abs(player_Target.transform.position.x - transform.position.x);
            if (disX < playerLeaveDisX)
                return false;
            return true;
                
        }
        


        private void MonsterChaseUpdate()
        {
            if(CheckViewTargetLeave())
            {
                ChangeStateTo(MonsterState.Idle);
                return;
            }
            int chaseRight = (player_Target.transform.position.x - transform.position.x) > 0 ? 1 : -1;
            ViewDirection = chaseRight > 0 ? Vector2.right : Vector2.left;
            var velocity = Vector2.right * moveSpeed * chaseRight;
            rigidbody.velocity = new Vector2(velocity.x, rigidbody.velocity.y);
            
            CheckAndJumpWhileMove();
        }

        private void MonsterIdleUpdate()
        {
            timer += Time.deltaTime;
            if(timer >= ildeWaitTime)
                ChangeStateTo(MonsterState.Patrol);
        }

        private void MonsterStiffUpdate()
        {
            if (stiffTime <= 0)
            {
                ChangeStateTo(MonsterState.Chase);
                return;
            }

            var vel = rigidbody.velocity;
            int dir = vel.x > 0 ? 1 : -1;
            var velX = vel.x - deceleration * dir * Time.deltaTime;
            if (dir * velX <= 0)
                velX = 0;
            rigidbody.velocity = new Vector2(velX, vel.y);
            stiffTime -= Time.deltaTime;
        }
        
        

        private bool ifStopPatrol;
        private async void MonsterPatrolStart()
        {
            ifStopPatrol = false;
            var targetPos = GetNextPosition(null);
            var direction = targetPos.transform.position.x - transform.position.x > 0 ? Vector2.right : Vector2.left;
            
            while (true)
            {
                if (ifStopPatrol)
                {
                    patrolTimer = 0;
                    break;
                }
                
                if (patrolTimer > patrolRestTime)
                {
                    ChangeStateTo(MonsterState.Idle);
                    patrolTimer = 0;
                    break;
                }

                if (CheckViewTarget())
                {
                    ChangeStateTo(MonsterState.Chase);
                    patrolTimer = 0;
                    break;
                }

                var dis = Mathf.Abs(targetPos.position.x - transform.position.x);
                if (dis < moveSpeed * Time.deltaTime)
                    transform.position = new Vector3(targetPos.position.x,transform.position.y);
                if (dis < arriveThreshold)
                {
                    targetPos = GetNextPosition(targetPos);
                    direction = targetPos.transform.position.x - transform.position.x > 0 ? Vector2.right : Vector2.left;
                }
                
                rigidbody.velocity = new Vector2(direction.x * moveSpeed, rigidbody.velocity.y);
                ViewDirection = rigidbody.velocity.x > 0 ? Vector2.right : Vector2.left;
                
                CheckAndJumpWhileMove(); // 跳跃检测
                
                patrolTimer += Time.deltaTime;
                await Task.Delay((int)(Time.deltaTime *1000));
            }
        }

        private int patrolIndex;
        private float patrolTimer;
        private Transform GetNextPosition(Transform curTarget)
        {
            patrolIndex++;
            if (curTarget == null)
            {
                patrolIndex = 0;
                return patrolPositions[patrolIndex];
            }
            else
            {
                if (patrolIndex >= patrolPositions.Count)
                    patrolIndex = 0;
                return patrolPositions[patrolIndex];
                
            }
        }
        
        private void ChangeStateTo(MonsterState targetState)
        {
            if (currentState == targetState)
                return;

            currentState = targetState;
            StateStart(currentState);
        }
        
        private void StateStart(MonsterState curState)
        {
            switch (curState)
            {
                case MonsterState.Chase:
                    break;
                case MonsterState.Idle:
                    rigidbody.velocity = Vector2.zero;
                    timer = 0;
                    break;
                case MonsterState.Patrol:
                    MonsterPatrolStart();
                    break;
                case MonsterState.Stiff:
                    var velX = (beatBackPower * beatBackDir).x;
                    rigidbody.velocity = new Vector2(velX, rigidbody.velocity.y);
                    /*rigidbody.AddForce(beatBackPower * beatBackDir,ForceMode2D.Impulse);*/
                    break;
            }
        }
        
        [Header("跳跃相关")]
        [SerializeField] private Vector2 jumpRayDir;
        [SerializeField] private Vector2 jumpRayUpDir;
        [SerializeField] private float jumpRayDis_Obstacle;
        [SerializeField] private float jumpRayDis_Fall;
        [SerializeField] private float jumpRayDis_Stand;
        [SerializeField] private float jumpRayDis_Up;
        [SerializeField] private float jumpRayDis_UpObstacle;
        [SerializeField] private float jumpSpeed;
        [SerializeField] private float jumpCD;

        [SerializeField] private float jumpUpDisThreshold_Chase;
        
        private float jumpTimer;
        

        private void CheckAndJumpWhileMove()
        {
            var rayDir = new Vector2(jumpRayDir.x * transform.localScale.x, jumpRayDir.y).normalized;  
            var hits = Physics2D.RaycastAll(transform.position,
                rayDir,jumpRayDis_Obstacle);
            var hits_Fall = Physics2D.RaycastAll(transform.position,
                rayDir,jumpRayDis_Fall);
            var hits_Stand = Physics2D.RaycastAll(transform.position,
                Vector2.down,jumpRayDis_Stand);
            
            var rayUpDir = new Vector2(jumpRayUpDir.x * transform.localScale.x, jumpRayUpDir.y).normalized;  
            var hits_Up = Physics2D.RaycastAll(transform.position,
                rayUpDir,jumpRayDis_Up);
            var hits_UpObstacle = Physics2D.RaycastAll(transform.position,
                Vector2.up,jumpRayDis_UpObstacle);
            
            bool ifStand = false;
            foreach (var hit in hits_Stand)
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    ifStand = true;
                    break;
                }
            }
            
            foreach (var hit in hits)
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    if (ifStand)
                    {
                        Jump(1);
                        return;
                    }
                }
            }
            
            // 悬崖 需要跳过去
            bool ifWillFall = true;
            foreach (var hit in hits_Fall)
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    if (ifStand)
                    {
                        ifWillFall = false;
                        break;
                    }
                }
            }

            
            
            // 跳上平台
            bool ifCanJumpOverObstacle = true;
            foreach (var hit in hits_UpObstacle)
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    if (ifStand)
                    {
                        ifCanJumpOverObstacle = false;
                        break;
                    }
                }
            }
            if (currentState == MonsterState.Chase)
            {
                var disY = player_Target.transform.position.y - transform.position.y;
                Debug.Log(disY);
                if (disY > jumpUpDisThreshold_Chase)
                {
                    foreach (var hit in hits_Up)
                    {
                        if (hit.collider.CompareTag("Ground") && ifCanJumpOverObstacle)
                        {
                            Jump(1.2f);
                            return;
                        }
                    }
                }
            }
            
            // 悬崖跳处理
            if (ifStand && ifWillFall)
            {
                if (currentState == MonsterState.Chase)
                {
                    if(ifCanJumpOverObstacle)
                        return;
                }
                Jump(1);
                return;
            }
            
        }

        private void Jump(float multiply)
        {
            if( jumpTimer > 0)
                return;
            var velY = multiply * jumpSpeed;
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, velY);
            jumpTimer = jumpCD;
        }


        private MonsterState currentState = MonsterState.Idle;
        
        private enum MonsterState
        {
            Idle,
            Patrol,
            Chase,
            Stiff
        }
        #endregion

        #region 事件

        private void OnBeHitCallBack(DamageInfo info)
        {
            if(currentState == MonsterState.Idle || 
               currentState== MonsterState.Patrol)
            player_Target = info.sourceActor as PlayerMono;
            ifStopPatrol = true;
            ChangeStateTo(MonsterState.Chase);

            foreach (var behit in info.BeHitPoints)
            {
                if (behit.Type == BeHitPoint.BehitType.Weakness)
                {
                    var dir = transform.position.x - info.damagePos.x > 0 ? Vector2.right : Vector2.left;
                    beatBackDir = dir;
                    beatBackPower = info.sourceBullet.Powoer;
                    stiffTime = info.damage/beatBackPower;
                    Debug.Log(stiffTime);
                    ChangeStateTo(MonsterState.Stiff);
                    return;
                }
            }
        }
        

        #endregion

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position,(Vector2)transform.position + ViewDirection * rayDistance);

            var rayDir = new Vector2( jumpRayDir.x * transform.localScale.x,jumpRayDir.y).normalized;
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position,(Vector2)transform.position + rayDir * jumpRayDis_Fall);
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position,(Vector2)transform.position + rayDir * jumpRayDis_Obstacle);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position,(Vector2)transform.position + Vector2.down * jumpRayDis_Stand);
            
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position,(Vector2)transform.position + Vector2.up * jumpRayDis_UpObstacle);
            
            var rayUpDir = new Vector2(jumpRayUpDir.x * transform.localScale.x, jumpRayUpDir.y).normalized; 
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position,(Vector2)transform.position + rayUpDir * jumpRayDis_Up);
        }
    }
}
