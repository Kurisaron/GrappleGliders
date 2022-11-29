using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager local;

    public Text scoreText, healthText, livesText, clockText, buttonPressedText;
    public GameObject levelFinishedScreen;
    public Text finalScoreText, finalLivesText, finalTimeText;
    public Text levelTransitionText;
    private int levelTransitionSeconds = 10;

    public void Awake()
    {
        PlayerUIManager.local = this;
        levelFinishedScreen.SetActive(false);
    }

    public void UpdateMainUI(int score, int currentHealth, int maxHealth, int lives, string clock)
    {
        scoreText.text = "Score: " + score.ToString();
        healthText.text = "Health: " + currentHealth.ToString() + "/" + maxHealth.ToString();
        livesText.text = "Lives left: " + lives.ToString();

        clockText.text = clock;
        PrintButtonPressed();
    }


    private void PrintButtonPressed() // prints buttons pressed on the keyboard, space doesn't work
    {
        if (Input.anyKeyDown)
        {
            buttonPressedText.text = "Button Right Now: " + Input.inputString;

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttonPressedText.text = "Button Right Now: " + KeyCode.Space;
        }

    }

    public void LevelWon(int score, int lives, string clock)
    {
        levelFinishedScreen.SetActive(true);

        finalScoreText.text = "Score: " + score.ToString();
        finalLivesText.text = "Lives left: " + lives.ToString();
        finalTimeText.text = "Time elapsed: " + clock;

        StartCoroutine(LevelTransition("win"));
    }

    public void LevelLost(int score, int lives, string clock)
    {
        levelFinishedScreen.SetActive(true);

        finalScoreText.text = "Score: " + score.ToString();
        finalLivesText.text = "Lives left: " + lives.ToString();
        finalTimeText.text = "Time elapsed: " + clock;

        StartCoroutine(LevelTransition("retry"));
    }

    public IEnumerator LevelTransition(string type)
    {
        string transitionText;
        switch (type)
        {
            case "retry":
                transitionText = "Retrying level in ";
                break;
            case "win":
                transitionText = "Next level in ";
                break;
            default:
                transitionText = "Error in ";
                break;
        }
        
        for (int timeLeft = levelTransitionSeconds; timeLeft > 0; timeLeft--)
        {
            levelTransitionText.text = transitionText + timeLeft.ToString() + " seconds..."; 
            
            yield return new WaitForSeconds(1);
        }

        Scene scene = SceneManager.GetActiveScene();

        switch (type)
        {
            case "retry":
                SceneManager.LoadScene(scene.name);
                break;
            case "win":
                switch (scene.name)
                {
                    case "TutorialLevel":
                        SceneManager.LoadScene("Level1");
                        break;
                    case "Level1":
                        SceneManager.LoadScene("Level2");
                        break;
                    case "Level2":
                        SceneManager.LoadScene("StartMenu");
                        break;
                    default:
                        Debug.Log("Given scene name does not correspond with any existing scene that should contain this script. Loading Tutorial");
                        SceneManager.LoadScene("TutorialLevel");
                        break;
                }
                break;
            default:
                Debug.Log("Given transition type did not match existing types");
                break;
        }
    }
}