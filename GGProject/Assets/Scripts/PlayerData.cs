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
    public float currentPlayerHealth = 5;
    public float maxPlayerHealth = 5;
    public float playerInvincibilityCooldown = 5;
    public bool playerInvincible = false;
    public int scorePerCoin = 1;

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
    IEnumerator AttackingTime(float playerInvincibilityCooldown) // this controls the player taking damage
    {
        currentPlayerHealth--;
        for (float t = 0; t < playerInvincibilityCooldown; t += Time.deltaTime)
        {
            playerInvincible = true;
            player.GetComponent<MeshRenderer>().material.color = Color.cyan;
            yield return null;
        }
        playerInvincible = false;
        player.GetComponent<MeshRenderer>().material.color = Color.gray;
    }
    public void Attacked()
    {
        if(playerInvincible == false)
        {
            StartCoroutine(AttackingTime(playerInvincibilityCooldown));
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
