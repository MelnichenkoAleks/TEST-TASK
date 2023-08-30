using UnityEngine;
using DG.Tweening;

public class GoPlayer : MonoBehaviour
{
    private bool hasJumped = false; 

    public void GoButton()
    {
        if (!hasJumped)
        {            
            hasJumped = true;           
            transform.DOJump(new Vector3(0f, 1f, 20f), 2f, 10, 5f);
        }
    }
}
