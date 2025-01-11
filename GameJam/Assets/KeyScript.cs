using UnityEngine;

public class KeyScript : MonoBehaviour
{
    public bool hasKey = false; 

    
    //method resets the key when game restarts
    public void ResetKeyStatus() 
    {
        hasKey = false;
    }
    
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Key")) //make sure its a key 
        {
            
            hasKey = true; //player now has a key
            Destroy(collision.gameObject); //remove key from the game scene
            
        }
    }


    
}