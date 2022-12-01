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
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player hit");
            playerData.Attacked();
            Destroy(this.gameObject);

        }
    }
}
