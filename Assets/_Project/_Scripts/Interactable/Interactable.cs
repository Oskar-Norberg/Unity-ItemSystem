using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public int Priority => priority;
    [SerializeField] private int priority;
    
    public delegate void OnInteractionFinishedEventHandler();
    public static event OnInteractionFinishedEventHandler OnInteractionFinished;
    
    public abstract void Interact(Transform interactor);
}
