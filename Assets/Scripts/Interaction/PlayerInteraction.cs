using UnityEngine;

namespace HiddenNepal.Interaction
{
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("Interaction Settings")]
        [SerializeField] private float interactRange = 3f;
        [SerializeField] private LayerMask interactableMask = ~0;
        [SerializeField] private KeyCode interactKey = KeyCode.E;

        private IInteractable currentInteractable;

        private void Update()
        {
            DetectInteractable();

            if (currentInteractable != null && Input.GetKeyDown(interactKey))
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

            // Fallback sphere overlap near player forward position
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
