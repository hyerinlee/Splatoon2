using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static Splatoon2.Define;

namespace Splatoon2
{
    [RequireComponent(typeof(PlayerInput))]
    public class InputActionHandler : Singleton<InputActionHandler>
    {
        [SerializeField] private PlayerInput playerInput;

        public static event Action onOK, onCancel, onPressLR, onStart, onTry, onPressE, onJump, onAttack;
        public static event Action<int> onHorizontalMove, onVerticalMove, onMoveByLR;
        public static event Action<bool, Vector2> onMove;





        public void Active(bool isActive)
        {
            playerInput.enabled = isActive;
        }

        public void SetDefaultActionMap(ACTION_MAP actionMap)
        {
            playerInput.defaultActionMap = ACTION_MAP_TO_STR[actionMap];
        }

        public void ChangeActionMap(ACTION_MAP actionMap)
        {
            playerInput.SwitchCurrentActionMap(ACTION_MAP_TO_STR[actionMap]);
        }



        // ▼ 인스펙터에서 추가 =============================================================================================================

        #region PlayerInkopolis & PlayerIngame ----------------------------------------------------------------------------------------------
        public void OnMove(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
            {
                onMove(true, context.ReadValue<Vector2>());
            }
            else if (context.phase.Equals(InputActionPhase.Canceled))
            {
                onMove(false, Vector2.zero);
            }
        }

        public void OnMoveCamera(InputAction.CallbackContext context)
        {
            // 카메라 시점 변경
        }

        public void OnResetCamera(InputAction.CallbackContext context)
        {
            // 카메라의 영점을 현재 위치로 고정
        }

        public void OnPressX(InputAction.CallbackContext context)
        {
            // 메뉴 버튼
        }
        #endregion



        #region PlayerInkopolis & UI --------------------------------------------------------------------------------------------------------
        public void OnOK(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
            {
                onOK?.Invoke();
            }
        }
        #endregion



        #region PlayerInkopolis ------------------------------------------------------------------------------------------------------------
        #endregion



        #region PlayerIngame ---------------------------------------------------------------------------------------------------------------
        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
            {
                onJump();
            }
        }

        public void OnAttack(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
            {
                onAttack();
            }
        }

        public void OnSubAttack(InputAction.CallbackContext context)
        {
            // 서브 웨폰 사용
        }

        public void OnChange(InputAction.CallbackContext context)
        {
            // 플레이어 체형 변경
        }
        #endregion



        #region UI -------------------------------------------------------------------------------------------------------------------------

        /// <summary> (UI에서 사용됨) </summary>
        public void OnCursorMove(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
            {
                var value = context.ReadValue<Vector2>();
                onHorizontalMove?.Invoke((int)value.x);
                onVerticalMove?.Invoke((int)value.y);
            }
        }

        public void OnCancel(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
            {
                onCancel?.Invoke();
            }
        }

        public void OnLR(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
            {
                onMoveByLR?.Invoke((int)context.ReadValue<float>());
                onPressLR?.Invoke();
            }
        }

        public void OnTry(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
            {
                onTry?.Invoke();
            }
        }

        public void OnPressE(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
            {
                onPressE.Invoke();
            }
        }
        #endregion



        #region Title ----------------------------------------------------------------------------------------------------------------------
        public void OnStart(InputAction.CallbackContext context)
        {
            if (context.phase.Equals(InputActionPhase.Performed))
            {
                onStart.Invoke();
            }
        }
        #endregion
    }
}