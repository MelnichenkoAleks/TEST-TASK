using System.Collections;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Color newObjectColor; 
    private Color originalColor; 
    private Renderer objectRenderer; 

    public AudioSource audioSource;
    public AudioClip fireclip;
    public AudioClip killCactusclip;

    public GameObject Particle_Death;

    private void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        originalColor = objectRenderer.material.color;
    }

    private void OnTriggerEnter(Collider other)
    {
        GameObject collidingObject = other.gameObject;
       
        if (collidingObject.CompareTag("Ball"))
        {            
            objectRenderer.material.color = newObjectColor;
            audioSource.PlayOneShot(fireclip);
            
            StartCoroutine(DestroyObjectAfterDelay(0.5f));
        }
    }

    private IEnumerator DestroyObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSource.PlayOneShot(killCactusclip);
        Instantiate(Particle_Death, transform.position, Quaternion.identity);
        Destroy(gameObject);        
    }
}
