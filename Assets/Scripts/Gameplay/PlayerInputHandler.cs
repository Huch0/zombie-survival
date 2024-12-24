using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Scripts.Game;

namespace Unity.Scripts.Gameplay
{
    public class PlayerInputHandler : MonoBehaviour
    {
        public Vector2 MoveInput { get; private set; }
        public Vector2 LookInput { get; private set; }
        public bool JumpInput { get; private set; }
        public bool ShootInput { get; private set; }
        public bool ReloadInput { get; private set; }
        public bool PickUpInput { get; private set; }
        public bool SprintInput { get; private set; }

        private bool isCursorLocked = true;

        void Start()
        {
            LockCursor(); // Lock the cursor at the start of the game
        }

        void Update()
        {
            // Toggle cursor lock/unlock with ESC
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isCursorLocked = !isCursorLocked;
                if (isCursorLocked)
                {
                    LockCursor();
                }
                else
                {
                    UnlockCursor();
                }
            }

            // If the cursor is unlocked, disable all inputs
            if (!isCursorLocked)
            {
                MoveInput = Vector2.zero;
                LookInput = Vector2.zero;
                JumpInput = false;
                ShootInput = false;
                ReloadInput = false;
                PickUpInput = false;
                SprintInput = false;
                return;
            }

            // Handle player inputs when cursor is locked
            MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            LookInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            JumpInput = Input.GetKeyDown(KeyCode.Space);
            ShootInput = Input.GetMouseButton(0); // Shooting continuously while holding the button
            ReloadInput = Input.GetKeyDown(KeyCode.R);
            PickUpInput = Input.GetKeyDown(KeyCode.E);
            SprintInput = Input.GetKey(KeyCode.LeftShift);
        }

        private void LockCursor()
        {
            Cursor.lockState = CursorLockMode.Locked; // Lock the cursor to the center of the screen
            Cursor.visible = false;                  // Hide the cursor
        }

        private void UnlockCursor()
        {
            Cursor.lockState = CursorLockMode.None;  // Release the cursor
            Cursor.visible = true;                   // Show the cursor
        }
    }
}
