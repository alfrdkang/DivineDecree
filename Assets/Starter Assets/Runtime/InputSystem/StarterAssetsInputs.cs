using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        public static StarterAssetsInputs instance;

        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool shoot;
        public bool reload;
        public bool primary;
        public bool secondary;
        public bool pause;
        public bool crouch;
        public bool interact;
        public bool aim;
        public bool skill;
        public bool inventory;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
        private void Awake()
        {
            instance = this;
        }

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
        public void OnSkill(InputValue value)
        {
            SkillInput(value.isPressed);
        }
        public void OnInventory(InputValue value)
        {
            InventoryInput(value.isPressed);
        }

#endif
        public void MoveInput(Vector2 newMoveDirection)
        {
            if (!pause) move = newMoveDirection; else move = Vector2.zero;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            if (!pause) look = newLookDirection; else look = Vector2.zero;
        }

        public void JumpInput(bool newJumpState)
        {
            if (!pause) jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            if (!pause) sprint = newSprintState;
        }

        public void ShootInput(bool newShootState)
        {
            if (!pause) shoot = newShootState;
        }

        public void ReloadInput(bool newReloadState)
        {
            if (!pause) reload = newReloadState;
        }

        public void AimInput(bool newAimState)
        {
            if (!pause) aim = newAimState;
        }

        public void PrimaryInput(bool newPrimaryState)
        {
            if (!pause) primary = newPrimaryState;
        }

        public void SecondaryInput(bool newSecondaryState)
        {
            if (!pause) secondary = newSecondaryState;
        }

        public void PauseInput(bool newPauseState)
        {
            pause = newPauseState;
        }

        public void CrouchInput(bool newCrouchState)
        {
            if (!pause) crouch = newCrouchState;
        }

        public void InteractInput(bool newInteractState)
        {
            if (!pause) interact = newInteractState;
        }

        public void SkillInput(bool newSkillState)
        {
            if (!pause) skill = newSkillState;
        }

        public void InventoryInput(bool newInventoryState)
        {
            inventory = newInventoryState;
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