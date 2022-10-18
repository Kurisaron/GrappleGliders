using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    [SerializeField] private Text scoreText, healthText, clockText, buttonPressedText;
    public int playerScore = 0;
    public float currentPlayerHealth = 5;
    public float maxPlayerHealth = 5;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + playerScore.ToString();
        healthText.text = "Health: " + currentPlayerHealth.ToString() + "/" + maxPlayerHealth.ToString();

    }

    // Update is called once per frame
    void Update()
    {
        Clock();
        PrintButtonPressed();
    }
    void Clock()
    {
        float timer = Time.time;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        clockText.text = niceTime;
    }
    void PrintButtonPressed()
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
}
