using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShoot : MonoBehaviour
{
    public GameObject ballPrefab;
    public Transform shootPoint;
    public float shootForce = 10.0f;
    public float scaleIncreaseRate = 0.005f;
    public Color maxScaleColor = Color.red;
    public GameObject playerObject;

    private GameObject currentBall;
    private bool isShooting = false;

    public GameObject gameOverPanel;
    public AudioSource audioSource;
    public AudioClip gameOverClip;

    public GameObject Particle_Death;

    void Update()
    {
        if (!IsPointerOverUIObject())
        {
            if (Input.GetButtonDown("Fire1") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
            {
                StartShooting();
            }

            if ((Input.GetButton("Fire1") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)) && isShooting)
            {
                KeepAimingAndScaling();
            }

            if ((Input.GetButtonUp("Fire1") || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) && isShooting)
            {
                Shoot();
                isShooting = false;
            }

            if (currentBall != null && currentBall.transform.localScale.x > 2f)
            {
                Instantiate(Particle_Death, transform.position, Quaternion.identity);
                audioSource.PlayOneShot(gameOverClip);
                Destroy(currentBall);
                gameOverPanel.SetActive(true);

                if (playerObject != null)
                {
                    Instantiate(Particle_Death, playerObject.transform.position, Quaternion.identity);
                    Destroy(playerObject);
                }
            }
        }
    }

    void StartShooting()
    {
        currentBall = Instantiate(ballPrefab, shootPoint.position, Quaternion.identity);
        isShooting = true;
    }

    void KeepAimingAndScaling()
    {
        if (currentBall != null)
        {
            currentBall.transform.position = shootPoint.position;

            Vector3 newScale = currentBall.transform.localScale + Vector3.one * scaleIncreaseRate;
            currentBall.transform.localScale = newScale;

            if (newScale.x >= 1.5f)
            {
                Renderer ballRenderer = currentBall.GetComponent<Renderer>();
                ballRenderer.material.color = maxScaleColor;
            }
        }
    }

    void Shoot()
    {
        Rigidbody ballRb = currentBall.GetComponent<Rigidbody>();
        ballRb.AddForce(shootPoint.forward * shootForce, ForceMode.Impulse);

        currentBall = null;
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
}
