using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace HiddenNepal.Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactRange = 3f;
        [SerializeField] private LayerMask interactableMask = ~0;

        private IInteractable currentInteractable;

        private void Update()
        {
            DetectInteractable();

            bool interactPressed = false;

#if ENABLE_INPUT_SYSTEM
            if (Keyboard.current != null)
            {
                interactPressed = Keyboard.current.eKey.wasPressedThisFrame;
            }
#else
            interactPressed = Input.GetKeyDown(KeyCode.E);
#endif

            if (currentInteractable != null && interactPressed)
            {
                currentInteractable.Interact(gameObject);
            }
        }

        private void DetectInteractable()
        {
            Ray ray = new Ray(Camera.main != null ? Camera.main.transform.position : transform.position, 
                              Camera.main != null ? Camera.main.transform.forward : transform.forward);

            if (Physics.Raycast(ray, out RaycastHit hit, interactRange, interactableMask))
            {
                if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    currentInteractable = interactable;
                    return;
                }
            }

            Collider[] colliders = Physics.OverlapSphere(transform.position + transform.forward * 1f, 1.5f, interactableMask);
            foreach (var col in colliders)
            {
                if (col.TryGetComponent<IInteractable>(out var interactable))
                {
                    currentInteractable = interactable;
                    return;
                }
            }

            currentInteractable = null;
        }

        public IInteractable GetCurrentInteractable() => currentInteractable;
    }
}
