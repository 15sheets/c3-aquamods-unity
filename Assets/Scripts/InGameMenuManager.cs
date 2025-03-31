using UnityEngine;

public class InGameMenuManager : MonoBehaviour
{

    // add stuff about pause menu here as well, later
    public GameObject deathMenu;


    // Update is called once per frame
    void Update()
    {
        
    }

    public void resumeGame()
    {
        Time.timeScale = 1;
        ControllerSettings.cs.getPlayer(1).ActivateInput();
        ControllerSettings.cs.getPlayer(2).ActivateInput();
    }

    public void pauseGame()
    {
        Time.timeScale = 0;
        ControllerSettings.cs.getPlayer(1).DeactivateInput();
        ControllerSettings.cs.getPlayer(2).DeactivateInput();
    }
}
