using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private Text scoreText, healthText, clockText, buttonPressedText;
    public Text finalScoreText, finalLivesText, finalTimeText;
    [SerializeField] private GameObject player, levelFinishedScreen;
    public int playerScore = 0;
    public int scorePerCoin = 1;

    public int currentPlayerHealth = 5;
    public int maxPlayerHealth = 5;
    public int currentLives = 3;
    public int maxLives = 3;

    public float playerInvincibilityCooldown = 5;
    public bool playerInvincible = false;

    public GameObject restartPoint;

    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + playerScore.ToString();
        healthText.text = "Health: " + currentPlayerHealth.ToString() + "/" + maxPlayerHealth.ToString();

        clockText.text = Clock();
        PrintButtonPressed();
    }

    public void Attacked()
    {
        if (playerInvincible == false)
        {
            StartCoroutine(AttackingTime(playerInvincibilityCooldown));
        }
    }

    IEnumerator AttackingTime(float playerInvincibilityCooldown) // this controls the player taking damage
    {
        currentPlayerHealth--;

        if (currentPlayerHealth <= 0)
        {
            RestartLevel();
        }

        for (float t = 0; t < playerInvincibilityCooldown; t += Time.deltaTime)
        {
            playerInvincible = true;
            player.GetComponent<MeshRenderer>().material.color = Color.cyan;
            yield return null;
        }
        playerInvincible = false;
        player.GetComponent<MeshRenderer>().material.color = Color.gray;
    }

    public void RestartLevel()
    {
        currentPlayerHealth = maxPlayerHealth;

        transform.position = restartPoint.transform.position;
        transform.rotation = restartPoint.transform.rotation;

        if (currentLives > 0)
        {
            --currentLives;
        }
        else
        {
            levelFinishedScreen.SetActive(true);
            DisplayLevelFinish();
        }
    }

    string Clock()
    {
        float timer = Time.time;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        return niceTime;
    }

    void PrintButtonPressed() // prints buttons pressed on the keyboard, space doesn't work
    {
        if (Input.anyKeyDown)
        {
            //print(Input.inputString);
            buttonPressedText.text = "Button Right Now: " + Input.inputString;

        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            buttonPressedText.text = "Button Right Now: " + KeyCode.Space;
        }
        /*foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(kcode))
                buttonPressedText.text = "Button Right Now: " + kcode;
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Goal")
        {
            Debug.Log("player is on goal");
            levelFinishedScreen.SetActive(true);
            DisplayLevelFinish();
        }

        if (other.gameObject.tag == "DeathFloor")
        {
            Debug.Log("Player has hit death floor");
            RestartLevel();
        }
    }

    void DisplayLevelFinish()
    {
        finalScoreText.text = "Score: " + playerScore.ToString();
        finalLivesText.text = "Health: " + currentPlayerHealth.ToString() + "/" + maxPlayerHealth.ToString();
        finalTimeText.text = "Time: " + Clock();
    }

    public void AddCoin()
    {
        playerScore += scorePerCoin;
    }
}
