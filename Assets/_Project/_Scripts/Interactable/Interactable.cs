using UnityEngine;

namespace Project.InteractableSystem
{
    public abstract class Interactable : MonoBehaviour
    {
        public delegate void OnInteractionFinishedEventHandler();
        public static event OnInteractionFinishedEventHandler OnInteractionFinished;

        public abstract void Interact(Transform interactor);

        protected void InteractionFinished()
        {
            OnInteractionFinished?.Invoke();
        }
    }
}
