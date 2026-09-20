using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace HiddenNepal.Player
{
    public class CameraController : MonoBehaviour
    {
        [Header("Target Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 targetOffset = new Vector3(0f, 1.5f, 0f);

        [Header("Orbit Settings")]
        [SerializeField] private float distance = 5f;
        [SerializeField] private float minDistance = 2f;
        [SerializeField] private float maxDistance = 10f;
        [SerializeField] private float sensitivityX = 0.15f;
        [SerializeField] private float sensitivityY = 0.15f;
        [SerializeField] private float minPitch = -20f;
        [SerializeField] private float maxPitch = 70f;

        [Header("Collision Avoidance")]
        [SerializeField] private LayerMask collisionLayers;
        [SerializeField] private float cameraRadius = 0.2f;

        private float currentX = 0f;
        private float currentY = 20f;

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void LateUpdate()
        {
            if (target == null) return;

            float mouseX = 0f;
            float mouseY = 0f;
            float scrollDelta = 0f;

#if ENABLE_INPUT_SYSTEM
            if (Mouse.current != null)
            {
                Vector2 delta = Mouse.current.delta.ReadValue();
                mouseX = delta.x * sensitivityX;
                mouseY = delta.y * sensitivityY;
                scrollDelta = Mouse.current.scroll.ReadValue().y * 0.001f;
            }
#else
            mouseX = Input.GetAxis("Mouse X") * sensitivityX * 20f;
            mouseY = Input.GetAxis("Mouse Y") * sensitivityY * 20f;
            scrollDelta = Input.GetAxis("Mouse ScrollWheel");
#endif

            currentX += mouseX;
            currentY -= mouseY;
            currentY = Mathf.Clamp(currentY, minPitch, maxPitch);

            distance = Mathf.Clamp(distance - scrollDelta * 4f, minDistance, maxDistance);

            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0f);
            Vector3 focusPoint = target.position + targetOffset;
            Vector3 desiredPosition = focusPoint - (rotation * Vector3.forward * distance);

            float currentDistance = distance;
            if (Physics.SphereCast(focusPoint, cameraRadius, (desiredPosition - focusPoint).normalized, out RaycastHit hit, distance, collisionLayers))
            {
                currentDistance = Mathf.Clamp(hit.distance, minDistance, distance);
            }

            Vector3 finalPosition = focusPoint - (rotation * Vector3.forward * currentDistance);
            transform.position = finalPosition;
            transform.LookAt(focusPoint);
        }
    }
}
