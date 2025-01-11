using UnityEngine;

public class RulesPanelManager : MonoBehaviour
{
    public GameObject rulesPanel; 

    //show the rules to the player
    public void ShowRulesPanel()
    {
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(true); 
        }
    }

    //hide the rules panel
    public void HideRulesPanel()
    {
        if (rulesPanel != null)
        {
            rulesPanel.SetActive(false); 
        }
    }
}



