using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinhataExplode : MonoBehaviour
{
    [SerializeField] private Animator animator;
    public PinhataAttack danho;

    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if(collision.gameObject.tag == "Player")
        {
            animator.SetBool("isDead", true); 
            StartCoroutine(kaboom());
        }
    }

    IEnumerator kaboom()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }
}
