using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace HiddenNepal.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float runSpeed = 8.5f;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] private float jumpHeight = 1.2f;
        [SerializeField] private float gravity = -19.62f;

        [Header("Ground Check")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundDistance = 0.3f;
        [SerializeField] private LayerMask groundMask;

        [Header("Camera Reference")]
        [SerializeField] private Transform cameraTransform;

        // Internal State
        private CharacterController controller;
        private Vector3 velocity;
        private bool isGrounded;
        private float turnSmoothVelocity;

        public bool IsGrounded => isGrounded;
        public float CurrentSpeed { get; private set; }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (cameraTransform == null && Camera.main != null)
            {
                cameraTransform = Camera.main.transform;
            }
        }

        private void Update()
        {
            HandleGroundCheck();
            HandleMovement();
            HandleJumpAndGravity();
        }

        private void HandleGroundCheck()
        {
            if (groundCheck != null)
            {
                isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            }
            else
            {
                isGrounded = controller.isGrounded;
            }

            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2f;
            }
        }

        private void HandleMovement()
        {
            float horizontal = 0f;
            float vertical = 0f;
            bool isSprinting = false;

#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null)
            {
                if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) vertical += 1f;
                if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) vertical -= 1f;
                if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontal += 1f;
                if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontal -= 1f;

                isSprinting = Keyboard.current.leftShiftKey.isPressed || Keyboard.current.rightShiftKey.isPressed;
            }
#else
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
            isSprinting = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
#endif

            Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;
            float targetSpeed = isSprinting ? runSpeed : walkSpeed;

            if (direction.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
                if (cameraTransform != null)
                {
                    targetAngle += cameraTransform.eulerAngles.y;
                }

                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDir.normalized * (targetSpeed * Time.deltaTime));

                CurrentSpeed = targetSpeed;
            }
            else
            {
                CurrentSpeed = 0f;
            }
        }

        private void HandleJumpAndGravity()
        {
            bool jumpPressed = false;

#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null)
            {
                jumpPressed = Keyboard.current.spaceKey.wasPressedThisFrame;
            }
#else
            jumpPressed = Input.GetButtonDown("Jump");
#endif

            if (jumpPressed && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);
        }

        private void OnDrawGizmosSelected()
        {
            if (groundCheck != null)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(groundCheck.position, groundDistance);
            }
        }
    }
}
