using UnityEngine;

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
        [SerializeField] private float sensitivityX = 3f;
        [SerializeField] private float sensitivityY = 2f;
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

            // Mouse Look Input
            currentX += Input.GetAxis("Mouse X") * sensitivityX * 100f * Time.deltaTime;
            currentY -= Input.GetAxis("Mouse Y") * sensitivityY * 100f * Time.deltaTime;
            currentY = Mathf.Clamp(currentY, minPitch, maxPitch);

            // Scroll Zoom
            float scroll = Input.GetAxis("Mouse ScrollWheel");
            distance = Mathf.Clamp(distance - scroll * 4f, minDistance, maxDistance);

            // Calculate Rotation & Target Position
            Quaternion rotation = Quaternion.Euler(currentY, currentX, 0f);
            Vector3 focusPoint = target.position + targetOffset;
            Vector3 desiredPosition = focusPoint - (rotation * Vector3.forward * distance);

            // Collision Check
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
