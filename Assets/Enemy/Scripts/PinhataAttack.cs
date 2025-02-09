using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinhataAttack : MonoBehaviour
{
    public int damage = 1;
    public Base playerMovement;
    public Transform playerTransform;

    // Start is called before the first frame update

    private void Awake() // Use Awake() if other scripts might depend on these values in their Start()
    {
        // Find the player object and its components.  This is safer than GameObject.Find
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); // Use tags!

        if (playerObject != null)
        {
            playerTransform = playerObject.GetComponent<Transform>();
            playerMovement = playerObject.GetComponent<Base>(); // Replace 'Base' with the *actual* type of your movement script (e.g., PlayerController).

            if (playerTransform == null)
            {
                Debug.LogError("Player Transform not found on Player GameObject!");
            }

            if (playerMovement == null)
            {
                Debug.LogError("Player Movement component not found on Player GameObject!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found!");
        }
    }
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

    void Update()
    {
        if(playerMovement.parried == true && Vector2.Distance(transform.position, playerTransform.position) <= 2)
        {
            Destroy(gameObject);
        }
    }
}
