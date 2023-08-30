using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 5.0f;
    public LineRenderer lineRenderer;
    public float minScale = 0.3f;
    public float maxScale = 2.0f;
    public float scaleChangeRate = 0.05f;
    public GameObject road;

    public Color lowScaleColor = Color.red;

    private float currentScale;
    private bool isShooting = false;

    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    public AudioSource audioSource;
    public AudioClip winLevelclip;
    public AudioClip gameOverclip;

    public GameObject Particle_Death;

    private void Start()
    {
        Time.timeScale = 1f;
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        currentScale = transform.localScale.x;
    }

    private void Update()
    {
        if (!IsPointerOverUIObject())
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.transform.position.y;

            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(mousePosition);

            Vector3 direction = targetPosition - transform.position;
            direction.y = 0;
            direction.Normalize();

            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (Input.GetMouseButton(0))
            {
                lineRenderer.enabled = true;

                lineRenderer.SetPosition(0, transform.position);

                Vector3 endPosition = transform.position + direction * 200f;
                lineRenderer.SetPosition(1, endPosition);

                float newScale = Mathf.Clamp(currentScale + scaleChangeRate * Time.deltaTime, minScale, maxScale);
                currentScale = newScale;

                float playerScale = Mathf.Clamp(transform.localScale.x - scaleChangeRate * 0.2f * Time.deltaTime, minScale, maxScale);
                transform.localScale = new Vector3(playerScale, playerScale, playerScale);

                if (road != null)
                {
                    float roadScaleX = Mathf.Clamp(road.transform.localScale.x - scaleChangeRate * 0.2f * Time.deltaTime, minScale, maxScale);
                    road.transform.localScale = new Vector3(roadScaleX, road.transform.localScale.y, road.transform.localScale.z);
                }

                if (playerScale < 0.5f)
                {
                    audioSource.PlayOneShot(gameOverclip);
                    Destroy(gameObject);
                    Instantiate(Particle_Death, transform.position, Quaternion.identity);
                    gameOverPanel.SetActive(true);
                }
                else if (playerScale < 0.75f)
                {
                    Renderer playerRenderer = GetComponent<Renderer>();
                    playerRenderer.material.color = lowScaleColor;
                }
            }
            else
            {
                lineRenderer.enabled = false;
                currentScale = transform.localScale.x;
            }
        }
    }

    bool IsPointerOverUIObject()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return true;
        }

        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(i).fingerId))
                {
                    return true;
                }
            }
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Instantiate(Particle_Death, transform.position, Quaternion.identity);
            audioSource.PlayOneShot(gameOverclip);
            Destroy(gameObject);
            gameOverPanel.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Finish"))
        {
            victoryPanel.SetActive(true);
            audioSource.PlayOneShot(winLevelclip);
        }
    }
}
