using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool shoot;
        public bool reload;
        public bool slide;
        public bool primary;
        public bool secondary;
        public bool pause;
        public bool crouch;
        public bool interact;
        public bool aim;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value)
        {
            MoveInput(value.Get<Vector2>());
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                LookInput(value.Get<Vector2>());
            }
        }

        public void OnJump(InputValue value)
        {
            JumpInput(value.isPressed);
        }

        public void OnSprint(InputValue value)
        {
            SprintInput(value.isPressed);
        }
        public void OnAim(InputValue value)
        {
            AimInput(value.isPressed);
        }
        public void OnShoot(InputValue value)
        {
            ShootInput(value.isPressed);
        }
        public void OnReload(InputValue value)
        {
            ReloadInput(value.isPressed);
        }
        public void OnSlide(InputValue value)
        {
            SlideInput(value.isPressed);
        }
        public void OnPrimary(InputValue value)
        {
            PrimaryInput(value.isPressed);
        }
        public void OnSecondary(InputValue value)
        {
            SecondaryInput(value.isPressed);
        }
        public void OnPause(InputValue value)
        {
            PauseInput(value.isPressed);
        }
        public void OnCrouch(InputValue value)
        {
            CrouchInput(value.isPressed);
        }
        public void OnInteract(InputValue value)
        {
            InteractInput(value.isPressed);
        }

#endif


        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            sprint = newSprintState;
        }

        public void AimInput(bool newAimState)
        {
            aim = newAimState;
        }

        public void ShootInput(bool newShootState)
        {
            shoot = newShootState;
        }

        public void ReloadInput(bool newReloadState)
        {
            reload = newReloadState;
        }

        public void SlideInput(bool newSlideState)
        {
            slide = newSlideState;
        }

        public void PrimaryInput(bool newPrimaryState)
        {
            primary = newPrimaryState;
        }

        public void SecondaryInput(bool newSecondaryState)
        {
            secondary = newSecondaryState;
        }

        public void PauseInput(bool newPauseState)
        {
            pause = newPauseState;
        }

        public void CrouchInput(bool newCrouchState)
        {
            crouch = newCrouchState;
        }

        public void InteractInput(bool newInteractState)
        {
            interact = newInteractState;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }

}