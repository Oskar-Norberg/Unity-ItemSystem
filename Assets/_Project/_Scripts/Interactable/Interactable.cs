using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public int Priority => priority;
    [SerializeField] private int priority;
    
    public abstract void Interact(Transform interactor);
}
