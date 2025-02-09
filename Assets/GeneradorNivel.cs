using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneradorNivel : MonoBehaviour
{
    [SerializeField] private GameObject[] partesNivel;
    [SerializeField] private float distanciaMinima;
    [SerializeField] private Transform puntoFinal;
    [SerializeField] private int cantidadInicial;
    [SerializeField] private float distanciaDestruccion = 4000f;
    private Transform jugador;
    private Transform ultimoPuntoFinal;

    private void Start(){
        jugador = GameObject.FindGameObjectWithTag("Player").transform;

        for(int i= 0; i< cantidadInicial; i++){
            GenerarParteNivel();
        }
        ultimoPuntoFinal = puntoFinal;
    }

    private void Update(){
        if(ultimoPuntoFinal!= null && Vector2.Distance(jugador.position, ultimoPuntoFinal.position)< distanciaMinima){
             GenerarParteNivel();
        }
        DestruirSegmento();
    }   

    private void GenerarParteNivel(){
        int numeroAleatorio= Random.Range(0, partesNivel.Length);
        GameObject nivel= Instantiate(partesNivel[numeroAleatorio], puntoFinal.position, Quaternion.identity);
        puntoFinal= BuscarPuntoFinal(nivel, "PuntoFinal");
        
        if(puntoFinal!= null){
            ultimoPuntoFinal= puntoFinal;
        }
    }

    private Transform BuscarPuntoFinal(GameObject parteNivel, string etiqueta){
        Transform punto = null;

        foreach(Transform ubicacion in parteNivel.transform){
            if(ubicacion.CompareTag(etiqueta)){
                punto = ubicacion;
                break;
            }
        }
        return punto;
    }

    private void DestruirSegmento(){
        GameObject[] segmentos = GameObject.FindGameObjectsWithTag("Segmento");

        foreach(GameObject segmento in segmentos){
            float distancia = Vector2.Distance(jugador.position, segmento.transform.position);

            if(distancia> distanciaDestruccion){
                Destroy(segmento);
            }
        }
    }
}
