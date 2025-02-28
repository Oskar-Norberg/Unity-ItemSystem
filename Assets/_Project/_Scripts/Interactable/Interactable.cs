using UnityEngine;

namespace Project.InteractableSystem
{
    public abstract class Interactable : MonoBehaviour
    {
        // TODO: Consider removing this, I don't think it's necessary. Just sort by closest to viewport center instead.
        public int Priority => priority;
        [SerializeField] private int priority;

        public delegate void OnInteractionFinishedEventHandler();
        public static event OnInteractionFinishedEventHandler OnInteractionFinished;

        public abstract void Interact(Transform interactor);

        protected void InteractionFinished()
        {
            OnInteractionFinished?.Invoke();
        }
    }
}
