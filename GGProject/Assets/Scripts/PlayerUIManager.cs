using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum TutorialInfo
{
    NA,
    BasicMovement,
    Grapple,
    Glide
}

public class PlayerUIManager : MonoBehaviour
{
    public static PlayerUIManager local;

    public Text scoreText, healthText, livesText, clockText, buttonPressedText;

    public GameObject levelFinishedScreen;
    public Text finalScoreText, finalLivesText, finalTimeText;
    public Text levelTransitionText;
    private int levelTransitionSeconds = 10;

    public GameObject tutorialUISet;
    public Text tutorialTitle;
    public Text tutorialDescription;

    private bool gamePaused;
    public GameObject pauseMenu;

    public void Awake()
    {
        PlayerUIManager.local = this;
        levelFinishedScreen.SetActive(false);
        tutorialUISet.SetActive(false);
        gamePaused = false;
        pauseMenu.SetActive(false);
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
                        SceneManager.LoadScene("Level3");
                        break;
                    case "Level3":
                        SceneManager.LoadScene("Level4");
                        break;
                    case "Level4":
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

    public void SetTutorialInfo(bool active, TutorialInfo tutorialInfo = TutorialInfo.NA)
    {
        tutorialUISet.SetActive(active);

        if (active)
        {
            switch (tutorialInfo)
            {
                case TutorialInfo.BasicMovement:
                    tutorialTitle.text = "Basic Movement";
                    tutorialDescription.text = "Press WASD to move around. Press SPACE to jump. You may jump up to 3 times before landing on the ground again, resetting the count.";
                    break;
                case TutorialInfo.Grapple:
                    tutorialTitle.text = "Grapple Gun";
                    tutorialDescription.text = "Aim the reticle and hold the LEFT MOUSE BUTTON to grapple to objects. Releasing your grapple on an enemy will un-alive them.";
                    break;
                case TutorialInfo.Glide:
                    tutorialTitle.text = "Glider Satchel";
                    tutorialDescription.text = "Hold the RIGHT MOUSE BUTTON to glide, slowing your fall. This glider also acts an air brake by helping to decrease momentum.";
                    break;
            }
        }
    }

    public void TogglePauseState()
    {
        gamePaused = !gamePaused;

        pauseMenu.SetActive(gamePaused);

        if (gamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
}
