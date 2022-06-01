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
        private void PlayerWalkUpate()
        {
            int moveToRight = 0;
            moveToRight += Input.GetKey(KeyCode.D) ? 1 : 0;
            moveToRight += Input.GetKey(KeyCode.A) ? -1 : 0;
            
            if (moveToRight == 0)
            {
                if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
                {
                    moveComponent.Walk(-lastMoveToRight);
                    return;
                }
                ChangeStateTo(PlayerState.Idle);
            }
            else
            {
                moveComponent.Walk(moveToRight);
                lastMoveToRight = moveToRight;
            }
        }

        private void PlayerIdleUpdate()
        {
            // Idle->Aim
            if(Input.GetKeyDown(KeyCode.Mouse1))
                ChangeStateTo(PlayerState.Aim);
            // Idle->Move
            int moveToRight = 0;
            moveToRight += Input.GetKey(KeyCode.D) ? 1 : 0;
            moveToRight += Input.GetKey(KeyCode.A) ? -1 : 0;
            
            if (moveToRight != 0)
                ChangeStateTo(PlayerState.Walk);
        }

        private void PlayerAimUpate()
        {
            // aim->Idle
            if(Input.GetKeyUp(KeyCode.Mouse1))
                ChangeStateTo(PlayerState.Idle);
            
            // 瞄准
            gun.AimTo(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            
            // 射击
            if (Input.GetKeyDown(KeyCode.Mouse0))
                gun.Shoot();
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
                    break;
                case PlayerState.Walk:
                    PlayerWalkUpate();
                    break;
                case PlayerState.Aim:
                    PlayerAimUpate();
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
                    break;
                case PlayerState.Walk:
                    break;
                case PlayerState.Aim:
                    gun.AimJitterReset();
                    break;
            }
        }
        
        private void ChangeStateTo(PlayerState targetState)
        {
            if (currentState == targetState)
                return;

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
