using UnityEngine;

public class WinPanelManager : MonoBehaviour
{
    public GameObject winPanel;  
    public MazeGenerator mazeGenerator;
    

    //method for regenerating the maze when restarting the game
    public void RetryGame()
    {
        Time.timeScale = 1; //resume the game
        winPanel.SetActive(false); //hide popup

        if (mazeGenerator != null)
        {
            
            mazeGenerator.ResetMaze(); //regenerate the maze
            
        }
        
        //reset the status of the key
        KeyScript playerKeyHandler = FindObjectOfType<KeyScript>();
        if (playerKeyHandler != null)
        {
            playerKeyHandler.ResetKeyStatus();
        }

    }
    
    
    
    // method displays the winning popup
    public void ShowWinPopup()
    {
        winPanel.SetActive(true); 
        Time.timeScale = 0; //pause the game
    }
    
    //exit the game 
    public void ExitGame()
    {
        Application.Quit(); //quit application
    }
}