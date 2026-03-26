using UnityEngine;

public class InteractableButton : MonoBehaviour
{
    private Animator animator;

    void Awake() => animator = GetComponent<Animator>();

    public void Interact() => animator.SetTrigger("Press");
}