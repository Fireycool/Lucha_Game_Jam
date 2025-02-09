using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinhataPatrol : MonoBehaviour
{
    public Transform[] puntosPatrol;
    public float moveSpeed = 2;
    public int Destino;

    // Update is called once per frame
    void Update()
    {
        if (Destino == 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, puntosPatrol[0].position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, puntosPatrol[0].position) < .5f)
            {
                Destino = 1;
                transform.localScale = new Vector3(-2, 2, 1);
            }
        }

        if(Destino == 1)  
        {
            transform.position = Vector2.MoveTowards(transform.position, puntosPatrol[1].position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, puntosPatrol[1].position) < .5f)
            {
                Destino = 0;
                transform.localScale = new Vector3(2, 2, 1);
            }
        }
    }
}
