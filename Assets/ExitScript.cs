using UnityEngine;


public class ExitScript : MonoBehaviour
{
    public WinPanelManager winPanelManager; // WinPanelManager script
    public GameObject keyText; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Player")) //check if its a player
        {
            KeyScript keyHandler = collision.GetComponent<KeyScript>();
            
            if (keyHandler != null && keyHandler.hasKey) //check if player has a key
            {
                
                if (winPanelManager != null)
                {
                    winPanelManager.ShowWinPopup(); //show win popup
                }
                
            }
            else
            {

                //show message that player needs a key to escape
                if (keyText != null)
                {
                    keyText.SetActive(true);
                    
                    //hide after 2 seconds
                    Invoke(nameof(HideKeyMessage), 2f);
                }
            }
        }
    }
    
    //hide the message
    void HideKeyMessage()
    {
        if (keyText != null)
        {
            keyText.SetActive(false);
        }
    }
}