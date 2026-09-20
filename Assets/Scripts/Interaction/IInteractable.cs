using UnityEngine;

namespace HiddenNepal.Interaction
{
    public interface IInteractable
    {
        string GetInteractPrompt();
        void Interact(GameObject instigator);
    }
}
