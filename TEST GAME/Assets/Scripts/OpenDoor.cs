using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    private Animator animator; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            animator = GetComponent<Animator>();
            animator.enabled= true;
        }
    }
}
