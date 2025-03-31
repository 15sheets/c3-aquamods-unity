using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenuManager : MonoBehaviour
{
    public GameObject depthText;

    // add stuff about pause menu here as well, later
    public GameObject pauseOverlay;
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

    public void activateDeathMenu()
    {
        // later move this so it only happens after death anim finishes
        depthText.SetActive(false);

        pauseOverlay.SetActive(true);
        deathMenu.SetActive(true);
    }

    public void playerDied()
    {
        pauseGame();
        
        // move this later
        activateDeathMenu();

        // ...when restart button is pressed, 
        // reload scene 
        // set controllersettings playerInputExists to false -- can i do this IN controllersettings somewhere? like a OnSceneReload or something?
    }

    public void reloadScene()
    {
        Time.timeScale = 1;

        string currSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currSceneName);
    }
}
