using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    public void StartTutorialButtonPressed()
    {
        SceneManager.LoadScene("TutorialLevel");
    }

    public void StartLevel1ButtonPressed()
    {
        SceneManager.LoadScene("Level1");
    }

    public void QuitButtonPressed()
    {
        Application.Quit();
    }
}
