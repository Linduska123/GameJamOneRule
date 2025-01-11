using UnityEngine;

public class GameStartScript : MonoBehaviour
{
    
    public GameObject player; 
    
    public GameObject startPanel; 

    void Start()
    {
        
        //in the beginning disable the player until the game starts
        if (player != null)
        {
            player.SetActive(false); 
        }
        
    }

    public void StartGame()
    {
        //hide the panel and start the game
        startPanel.SetActive(false);
        
        if (player != null)
        {
            player.SetActive(true);
        }
        
    }
}