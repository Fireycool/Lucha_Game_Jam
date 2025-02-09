using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinhataFreaky : MonoBehaviour
{

    public Transform playerTransform;
    public float moveSpeed = 2;

    // Update is called once per frame
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
