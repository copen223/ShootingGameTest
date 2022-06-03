using System;
using ActorModule;
using ShootModule.Gun;
using UnityEngine;

namespace ActorModule.Player
{
    public class PlayerController : MonoBehaviour
    {
        //---------挂载对象------------
        [SerializeField] private Rigidbody2D rigidbody;
        [SerializeField] private MoveComponent moveComponent;
        [SerializeField] private PlayerMono playerMono;
        [SerializeField] private Gun gun;
        private Camera mainCamera;
        
        //----------参数--------------
        public Vector2 ViewDirection
        {
            get => viewDirection;
            private set
            {
                viewDirection = value;
                
                int scaleX = ViewDirection.x > 0 ? 1 : -1;
                transform.localScale = new Vector3(scaleX, 1, 1);
            }
        }

        private Vector2 viewDirection;// 视角

        //-----------时序------------
        private void Start()
        {
            moveComponent.Init(rigidbody);
            mainCamera = Camera.main;
        }

        private void Update()
        {
            StateUpdate();
        }

        #region 状态机

        

        
        //----------简易状态机-------------
        private PlayerState currentState = PlayerState.Idle;

        //--------------Update---------------
        private int lastMoveToRight;
        private int moveToRight;
        /// <summary>
        /// 根据输入进行横向移动
        /// </summary>
        private void MoveHorizontally()
        {
            moveToRight = 0;
            moveToRight += Input.GetKey(KeyCode.D) ? 1 : 0;
            moveToRight += Input.GetKey(KeyCode.A) ? -1 : 0;
            if (moveToRight == 0)
            {
                if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
                {
                    if (lastMoveToRight != 0)
                    {
                        moveComponent.StartWalk(-lastMoveToRight);
                        if (currentState != PlayerState.Aim)
                            viewDirection = Vector2.right * (-lastMoveToRight);
                    }
                    
                    moveComponent.Walk();
                    lastMoveToRight = 0;
                    return;
                }
                
                if(lastMoveToRight !=0 )
                    moveComponent.StopWalk();
                moveComponent.Walk();
                lastMoveToRight = 0;
            }
            else
            {
                if (lastMoveToRight != moveToRight)
                {
                    moveComponent.StartWalk(moveToRight);
                    if(currentState!= PlayerState.Aim)
                        ViewDirection = new Vector2(moveToRight, 0);
                }

                moveComponent.Walk();
                if(currentState!= PlayerState.Aim)
                    ViewDirection = new Vector2(moveToRight, 0);
                lastMoveToRight = moveToRight;
            }
        }

        private void MoveVertically()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (moveComponent.IfWillStand)
                    ifWillJump = true;
                if (moveComponent.IfStanding)
                {
                    moveComponent.Jump();
                    ifWillJump = false;
                    return;
                }
            }
            
            if (!moveComponent.IfStanding && (moveComponent.Velocity.y > 0))
            {
                if(Input.GetKeyUp(KeyCode.Space))
                {
                    moveComponent.ChangeYSpeed(0.7f);
                }
            }

            if (ifWillJump && moveComponent.IfStanding)
            {
                moveComponent.Jump();
                ifWillJump = false;
                return;
            }

        }
        
        private void PlayerWalkUpdate()
        {
            // walk->jump
            if ((Input.GetKeyDown(KeyCode.Space)||Input.GetKeyUp(KeyCode.Space)) && moveComponent.IfStanding)
            {
                ChangeStateTo(PlayerState.Jump);
                return;
            }
            
            // walk->Aim
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ChangeStateTo(PlayerState.Aim);
                return;
            }

            // walk->idle
            if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            {
                ChangeStateTo(PlayerState.Idle);
                return;
            }

            // move
            MoveHorizontally();
        }
        
        bool ifWillJump = false;
        private void PlayerJumpUpdate()
        {
            MoveVertically();
            MoveHorizontally();
            
            // jump->idle
            if (moveComponent.IfStanding && moveComponent.Velocity.y <= Mathf.Epsilon)
            {
                if(Mathf.Abs(rigidbody.velocity.x) > Mathf.Epsilon)
                    ChangeStateTo(PlayerState.Walk);
                else
                    ChangeStateTo(PlayerState.Idle);
            }
            // jump->aim
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ChangeStateTo(PlayerState.Aim);
            }
        }

        private void PlayerIdleUpdate()
        {
            
            // Idle->Aim
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                ChangeStateTo(PlayerState.Aim);
                return;
            }
            
            // Idle->jump
            if ((Input.GetKeyDown(KeyCode.Space)||Input.GetKeyUp(KeyCode.Space)) && moveComponent.IfStanding)
            {
                ChangeStateTo(PlayerState.Jump);
                return;
            }

            // Idle->Move
            moveToRight = 0;
            moveToRight += Input.GetKey(KeyCode.D) ? 1 : 0;
            moveToRight += Input.GetKey(KeyCode.A) ? -1 : 0;
            
            if (moveToRight != 0)
            {
                ChangeStateTo(PlayerState.Walk);
                return;
            }
            
            // 减速
            moveComponent.Walk();
            lastMoveToRight = 0;
        }

        private void PlayerAimUpdate()
        {
            // aim->Idle; aim->Jump
            if (Input.GetKeyUp(KeyCode.Mouse1))
            {
                gun.StopAiming();
                moveComponent.ChangeMoveSpeedMultiply(1f);
                
                ChangeStateTo(PlayerState.Idle);
                return;
            }


            // 瞄准
            Vector3 mousePosWorld = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            bool ifAimingRight = (mousePosWorld - transform.position).x > 0;
            gun.AimTo(mousePosWorld);
            
            // 射击
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                gun.Shoot();
                float recoil = gun.Recoil;
                Vector2 force = ifAimingRight ? recoil * Vector2.left : recoil * Vector2.right;
                
                // 后坐力
                rigidbody.AddForce(force, ForceMode2D.Impulse);
                moveComponent.StopWalk();
                lastMoveToRight = 0;
            }
            
            // 瞄准带来的视角
            ViewDirection = (mousePosWorld - transform.position).x > 0? Vector2.right : Vector2.left;
            
            // 移动
            MoveHorizontally();
            MoveVertically();
            
            // 移速修正
            if(moveComponent.IfStanding)
                moveComponent.ChangeMoveSpeedMultiply(playerMono.SpeedMultiply_Aiming);
            else
            {
                moveComponent.ChangeMoveSpeedMultiply(1);
            }
            moveComponent.StartWalk(moveToRight);
        }
        
        //--------------UpdateEnd---------------

        void StateUpdate()
        {
            switch (currentState)
            {
                case PlayerState.Idle:
                    PlayerIdleUpdate();
                    break;
                case PlayerState.Jump:
                    PlayerJumpUpdate();
                    break;
                case PlayerState.Walk:
                    PlayerWalkUpdate();
                    break;
                case PlayerState.Aim:
                    PlayerAimUpdate();
                    break;
            }
            
        }

        void StateStart(PlayerState state)
        {
            switch (state)
            {
                case PlayerState.Idle:
                    moveComponent.StopWalk();
                    break;
                case PlayerState.Jump:
                    moveComponent.Jump();
                    break;
                case PlayerState.Walk:
                    moveComponent.StartWalk(moveToRight);
                    break;
                case PlayerState.Aim:
                    gun.AimJitterReset();
                    break;
            }
        }
        
        private void ChangeStateTo(PlayerState targetState)
        {
            /*if (currentState == targetState)
                return;*/

            currentState = targetState;
            OnStateChangeToEvent?.Invoke(targetState);
            StateStart(currentState);
        }
        
        event Action<PlayerState> OnStateChangeToEvent;

        private enum PlayerState
        {
            Idle,
            Walk,
            Jump,
            Aim
        }
        
        #endregion
    }
}
