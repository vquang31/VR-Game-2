using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TriggerAnimation : NewMonoBehaviour
{
    [SerializeField] private Animator animator;

    [SerializeField] private string animationName = "";

    public void OnSelectEntered()
    {
        Debug.Log("SELECT");
    }

    public void Trigger()
    {
        animator.SetTrigger(animationName);
        Debug.Log("Triggering animation: " + animationName);

    }
}
