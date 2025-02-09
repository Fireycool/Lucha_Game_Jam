using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GeneradorNivel : MonoBehaviour
{
    [SerializeField] private GameObject[] partesNivel;
    [SerializeField] private float distanciaMinima;
    [SerializeField] private Transform puntoFinal;
    [SerializeField] private int cantidadInicial;
    [SerializeField] private float distanciaDestruccion = 0f; // Adjusted value
    private Transform jugador;
    private Transform ultimoPuntoFinal;
    public Transform bg1;
    public Transform bg2;
    private float size;

    private void Start()
    {
        size = bg1.GetComponent<BoxCollider2D>().size.y;
        jugador = GameObject.FindGameObjectWithTag("Player").transform;

        for (int i = 0; i < cantidadInicial; i++)
        {
            GenerarParteNivel();
        }
        ultimoPuntoFinal = puntoFinal;
    }

    private void Update()
    {
        // Background update
        if (jugador.position.y >= bg2.position.y)
        {
            bg1.position = new Vector3(bg1.position.x, bg2.position.y + size, bg1.position.z);
            Switching();
        }

        if (ultimoPuntoFinal != null && Vector2.Distance(jugador.position, ultimoPuntoFinal.position) < distanciaMinima)
        {
            GenerarParteNivel();
        }
        DestruirSegmento();
    }

    private void Switching()
    {
        // Swap the backgrounds
        Transform temp = bg1;
        bg1 = bg2;
        bg2 = temp;
    }

    private void GenerarParteNivel()
    {
        int numeroAleatorio = Random.Range(0, partesNivel.Length);
        GameObject nivel = Instantiate(partesNivel[numeroAleatorio], puntoFinal.position, Quaternion.identity);
        puntoFinal = BuscarPuntoFinal(nivel, "PuntoFinal");

        if (puntoFinal != null)
        {
            ultimoPuntoFinal = puntoFinal;
        }
    }

    private Transform BuscarPuntoFinal(GameObject parteNivel, string etiqueta)
    {
        Transform punto = null;

        foreach (Transform ubicacion in parteNivel.transform)
        {
            if (ubicacion.CompareTag(etiqueta))
            {
                punto = ubicacion;
                break;
            }
        }
        return punto;
    }

    private void DestruirSegmento()
    {
        GameObject[] segmentos = GameObject.FindGameObjectsWithTag("Segmento");

        foreach (GameObject segmento in segmentos)
        {
            // Calculate the vertical difference between the player and the segment
            float yDifference = jugador.position.y - segmento.transform.position.y;

            // Get the height of the segment
            float segmentoHeight = segmento.GetComponent<Renderer>().bounds.size.y;

            // Add a buffer to ensure segments are not destroyed too soon
            float buffer = segmentoHeight * 1.5f;

            // Check if the segment is sufficiently below the player
            if (yDifference > (segmentoHeight + buffer))
            {
                // Destroy the segment since it's more than one segment height below the player
                Destroy(segmento);
            }
        }
    }
}