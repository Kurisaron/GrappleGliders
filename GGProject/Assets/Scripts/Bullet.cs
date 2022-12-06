using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //public GameObject player;
    private PlayerData playerData;
    private float lifetime, age;

    
    public void Init(PlayerData pD, float life)
    {
        playerData = pD;
        lifetime = life;
        age = 0.0f;

        Debug.Log("Bullet spawned");
    }

    private void Update()
    {
        if (age >= lifetime)
        {
            Destroy(this.gameObject);
        }
        else
        {
            age += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                Debug.Log("Player hit");
                playerData.Attacked();
                Destroy(this.gameObject);
                break;
            case "Enemy":
                break;
            default:
                Debug.Log("Environment hit");
                Destroy(this.gameObject);
                break;
        }
    }
}
