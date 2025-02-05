using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool GameIsPaused = false;
    public static bool GameIsTransitioning = false;

    public GameObject PauseMenuUI;
    public GameObject HelpMenuUI;

    void Start() {
        GameIsTransitioning = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (GameIsPaused) {
                Resume();
            }
            else if(GameIsTransitioning) {
                //Do nothing lol
            }
            else {
                Pause();
            }
        }
    }

    public void ManualUpdate() {
        if (GameIsPaused){
            Resume();
        }
        else {
            Pause();
        }
    }

    public void ManualTransitionUpdate() {
        if (GameIsPaused){
            GameIsTransitioning = true;
            Resume();
        }
        else {
            Pause();
        }
    }

    void Resume() {
        GameIsPaused = false;
        PauseMenuUI.SetActive(false);
        HelpMenuUI.SetActive(false);
        Time.timeScale = 1f;
    }

    void Pause() {
        GameIsPaused = true;
        PauseMenuUI.SetActive(true);
        HelpMenuUI.SetActive(false);
        Time.timeScale = 0f;
    }
}
