using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    
    public float smoothSpeed = 0.125f; //camera speed
    
    public Vector3 offset; //offset from the player
    
    public Transform target; 


    void LateUpdate()
    {
        
        if (target is not null)
        {
            
            Vector3 desiredPosition = target.position + offset;
            
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            
            transform.position = smoothedPosition;
            
            
        }
        
    }
    
}
