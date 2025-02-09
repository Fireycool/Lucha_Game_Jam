using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinhataFreaky : MonoBehaviour
{

    public Transform playerTransform;
    public float moveSpeed = 2;

    // Update is called once per frame

    private void Awake() // Use Awake() if other scripts might depend on these values in their Start()
    {
        // Find the player object and its components.  This is safer than GameObject.Find
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player"); // Use tags!

        if (playerObject != null)
        {
            playerTransform = playerObject.GetComponent<Transform>();

            if (playerTransform == null)
            {
                Debug.LogError("Player Transform not found on Player GameObject!");
            }
        }
        else
        {
            Debug.LogError("Player GameObject with tag 'Player' not found!");
        }
    }
    void Update()
    {
        if(Vector2.Distance(transform.position, playerTransform.position) < 10)
        {
            if (transform.position.x > playerTransform.position.x)
            {
                transform.position += Vector3.left * moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += Vector3.right * moveSpeed * Time.deltaTime;
            }

            if(transform.position.y > playerTransform.position.y)
            {
                transform.position += Vector3.down * moveSpeed * Time.deltaTime;
            }
            else
            {
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            }
        }
    }
}
