using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Scripts.Game;

namespace Unity.Scripts.Gameplay
{
    [RequireComponent(typeof(Health), typeof(CharacterController), typeof(PlayerInputHandler))]
    public class PlayerCharacterController : MonoBehaviour
    {
        public GameFlowManager gameFlowManager;
        private CharacterController characterController;
        private PlayerInputHandler inputHandler;
        private Camera playerCamera;
        public Gun gun;
        public Health health;

        // Camera settings
        float m_CameraVerticalAngle = 0f;
        public float RotationSpeed = 200f;
        public float RotationMultiplier = 1f;

        public float moveSpeed = 3f;
        public float sprintSpeed = 6f;
        public float jumpHeight = 1f;
        public float gravity = -9.81f;

        private Vector3 velocity;
        private bool isGrounded;

        private Vector3 knockbackDirection;
        private float knockbackSpeed;
        private float knockbackDuration;
        private float knockbackTimer;
        private bool isKnockedBack;
        private PlayerInteraction playerInteraction;


        // Start is called before the first frame update
        void Start()
        {
            characterController = GetComponent<CharacterController>();
            inputHandler = GetComponent<PlayerInputHandler>();
            playerCamera = GetComponentInChildren<Camera>();
            gun = GetComponentInChildren<Gun>();
            health = GetComponent<Health>();
            playerInteraction = GetComponent<PlayerInteraction>();

            health.OnDie += OnDie;
            health.OnDamaged += OnDamaged;

            print("PlayerCamera: " + playerCamera);
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovement();
            HandleCameraRotation();
            HandleGunRotation();
            HandleActions();
        }

        void HandleMovement()
        {
            if (isKnockedBack)
            {
                // Apply knockback movement
                characterController.Move(knockbackDirection * knockbackSpeed * Time.deltaTime);
                knockbackTimer += Time.deltaTime;

                // Check if knockback duration has elapsed
                if (knockbackTimer >= knockbackDuration)
                {
                    isKnockedBack = false;
                }
            }
            else
            {
                // Existing movement code
                isGrounded = characterController.isGrounded;
                if (isGrounded && velocity.y < 0)
                {
                    velocity.y = -2f;
                }

                Vector3 move = transform.right * inputHandler.MoveInput.x + transform.forward * inputHandler.MoveInput.y;
                float speed = inputHandler.SprintInput ? sprintSpeed : moveSpeed;
                characterController.Move(move * speed * Time.deltaTime);

                if (inputHandler.JumpInput && isGrounded)
                {
                    velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
                }

                velocity.y += gravity * Time.deltaTime;
                characterController.Move(velocity * Time.deltaTime);
            }
        }


        void HandleCameraRotation()
        {
            // horizontal character rotation
            {
                // rotate the transform with the input speed around its local Y axis
                transform.Rotate(new Vector3(0f, inputHandler.LookInput.x * RotationSpeed * RotationMultiplier, 0f), Space.Self);
            }

            // vertical camera rotation
            {
                // add vertical inputs to the camera's vertical angle
                m_CameraVerticalAngle -= inputHandler.LookInput.y * RotationSpeed * RotationMultiplier;

                // limit the camera's vertical angle to min/max
                m_CameraVerticalAngle = Mathf.Clamp(m_CameraVerticalAngle, -89f, 89f);

                // apply the vertical angle as a local rotation to the camera transform along its right axis (makes it pivot up and down)
                playerCamera.transform.localEulerAngles = new Vector3(m_CameraVerticalAngle, 0f, 0f);
            }

            // print("LookInput.x: " + inputHandler.LookInput.x + " LookInput.y: " + inputHandler.LookInput.y + " m_CameraVerticalAngle: " + m_CameraVerticalAngle);
        }

        void HandleGunRotation()
        {
            // Implement gun rotation logic
            // Rotate the gun based on the camera's rotation
            gun.transform.rotation = playerCamera.transform.rotation;
        }



        void HandleActions()
        {
            if (inputHandler.ShootInput)
            {
                Shoot();
            }

            if (inputHandler.ReloadInput)
            {
                Reload();
            }

            if (inputHandler.PickUpInput)
            {
                PickUp();
            }
        }

        void Shoot()
        {
            // Implement shooting logic
            gun.Shoot();
        }

        void Reload()
        {
            // Implement reloading logic
            gun.Reload();
        }

        void PickUp()
        {
            if (playerInteraction != null)
            {
                playerInteraction.PickUpItem();
            }
            else
            {
                Debug.LogWarning("No item to pick up.");
            }
        }

        // Example: Applying knockback when taking damage
        void OnDamaged(float damage, GameObject damageSource)
        {
            Debug.Log("Player Damaged: " + damage);
            gameFlowManager.OnPlayerHit();

            // Calculate knockback direction away from the damage source
            Vector3 direction = transform.position - damageSource.transform.position;
            float knockbackForce = 5f; // Adjust as needed
            float knockbackTime = 0.5f; // Adjust as needed

            ApplyKnockback(direction, knockbackForce, knockbackTime);
        }

        public void ApplyKnockback(Vector3 direction, float speed, float duration)
        {
            knockbackDirection = direction.normalized;
            knockbackSpeed = speed;
            knockbackDuration = duration;
            knockbackTimer = 0f;
            isKnockedBack = true;
        }


        void OnDie()
        {
            Debug.Log("Player Died!");

            // Implement player death logic
        }
    }
}