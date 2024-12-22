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

        // Update is called once per frame
        void Update()
        {
            MoveInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            LookInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            JumpInput = Input.GetKeyDown(KeyCode.Space);
            ShootInput = Input.GetMouseButtonDown(0);
            ReloadInput = Input.GetKeyDown(KeyCode.R);
            PickUpInput = Input.GetKeyDown(KeyCode.E);
            SprintInput = Input.GetKey(KeyCode.LeftShift);
        }
    }
}