using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; 
    public float smoothSpeed = 0.125f;
    public Vector3 offset = new Vector3(0, 5, -10); 

    private void FixedUpdate()
    {
        if (player != null)
        {
            transform.position = player.position + offset;

            transform.LookAt(player);
        }
    }
}