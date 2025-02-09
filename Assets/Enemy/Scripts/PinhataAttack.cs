using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinhataAttack : MonoBehaviour
{
    public int damage = 1;
    public Base playerMovement;
    // Start is called before the first frame update
    private void OnCollisionEnter2D(Collision2D collision)
    { 
        if(collision.gameObject.tag == "Player")
        {
            playerMovement.KBCounter = playerMovement.KBTime;
            if(collision.transform.position.x <= transform.position.x)
            {
                playerMovement.KnockFromRight = true;
            }
            else
            {
                playerMovement.KnockFromRight = false;
            }
            playerMovement.TakeDamage(damage);
        }
    }
}
