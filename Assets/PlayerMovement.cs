using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    
    public Rigidbody2D rb;
    
    public float movementSpeed = 5f; //player speed
    
    private Vector2 _movement;
    

    void FixedUpdate()
    {
        
        //move player
        rb.MovePosition(rb.position + _movement * (movementSpeed * Time.fixedDeltaTime));
        
    }
    
    void Update()
    {
        
        //get player input
        _movement.x = Input.GetAxisRaw("Horizontal"); //horizontal movement (A/D or arrow keys)
        _movement.y = Input.GetAxisRaw("Vertical");   //vertical movement (W/S or arrow keys)
        
    }
    
}
