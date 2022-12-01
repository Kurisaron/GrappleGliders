using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public static PlayerData local;
    
    [SerializeField] private GameObject player;
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
    void Awake()
    {
        PlayerData.local = this;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerUIManager.local.UpdateMainUI(playerScore, currentPlayerHealth, maxPlayerHealth, currentLives, Clock());
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
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        if (currentLives > 0)
        {
            --currentLives;
        }
        else
        {
            PlayerUIManager.local.LevelLost(playerScore, currentLives, Clock());
        }
    }

    public string Clock()
    {
        float timer = Time.time;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        return niceTime;
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Goal")
        {
            Debug.Log("player is on goal");
            PlayerUIManager.local.LevelWon(playerScore, currentLives, Clock());
        }

        if (other.gameObject.tag == "DeathFloor")
        {
            Debug.Log("Player has hit death floor");
            RestartLevel();
        }
    }

    

    public void AddCoin()
    {
        playerScore += scorePerCoin;
    }
}
