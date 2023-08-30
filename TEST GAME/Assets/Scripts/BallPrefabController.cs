using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPrefabController : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
