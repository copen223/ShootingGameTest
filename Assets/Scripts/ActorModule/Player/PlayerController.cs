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
        public Vector2 ViewDirection;   // 视角

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
                        var scale = transform.localScale;
                        var scalX = scale.x;
                        transform.localScale = new Vector3(-scalX, 1, 1);
                    }
                    
                    moveComponent.Walk();
                    lastMoveToRight = 0;
                    ViewDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                    
                    return;
                }
                moveComponent.StopWalk();
                moveComponent.Walk();
                lastMoveToRight = 0;
                ViewDirection = Vector2.zero;
            }
            else
            {
                if (lastMoveToRight != moveToRight)
                {
                    moveComponent.StartWalk(moveToRight);
                    transform.localScale = new Vector3(moveToRight, 1, 1);
                }

                moveComponent.Walk();
                lastMoveToRight = moveToRight;
                ViewDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            }
        }

        private void MoveVertically()
        {
            if (Input.GetKeyDown(KeyCode.Space)||Input.GetKeyUp(KeyCode.Space))
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
            if (moveComponent.IfStanding)
            {
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
            ViewDirection = Vector2.zero;
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
            gun.AimTo(mousePosWorld);

            // 射击
            if (Input.GetKeyDown(KeyCode.Mouse0))
                gun.Shoot();
            
            // 移动
            MoveHorizontally();
            MoveVertically();
            
            // 瞄准带来的视角
            ViewDirection = (mousePosWorld - transform.position).x>0? Vector2.right : Vector2.left;
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
                    moveComponent.ChangeMoveSpeedMultiply(playerMono.SpeedMultiply_Aiming);
                    moveComponent.StartWalk(moveToRight);   // 这两行是用作改移速
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
