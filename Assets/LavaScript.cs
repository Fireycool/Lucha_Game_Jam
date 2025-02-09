using UnityEngine;

public class LavaScript : MonoBehaviour
{
    public float velocidadSubida = 0.5f;
    public float tiempoHastaSubida = 10f;
    public int damage = 1;
    private Base playerMovement; // El tipo debe coincidir con el script del jugador

    private void Start()
    {
        Invoke("EmpezarSubida", tiempoHastaSubida);

        // Busca el componente PlayerMovement al inicio.
        GameObject player = GameObject.FindGameObjectWithTag("Player"); // Usa tags
        if (player != null)
        {
            playerMovement = player.GetComponent<Base>(); // Reemplaza "Base" con el nombre real del script
            if (playerMovement == null)
            {
                Debug.LogError("No se encontró el componente Base en el jugador.");
            }
        }
        else
        {
            Debug.LogError("No se encontró el objeto jugador con la etiqueta 'Player'.");
        }
    }


    private void EmpezarSubida()
    {
        StartCoroutine(SubirLava());
    }

    private System.Collections.IEnumerator SubirLava()
    {
        while (true)
        {
            transform.Translate(Vector3.up * velocidadSubida * Time.deltaTime);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerMovement != null) // ¡Revisa si playerMovement es nulo!
            {
                playerMovement.TakeDamage(damage);
                Debug.Log("¡El jugador ha tocado la lava!");
            }
            else
            {
                Debug.LogError("playerMovement es nulo. No se puede aplicar daño.");
            }
        }
    }
}