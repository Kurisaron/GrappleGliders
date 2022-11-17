using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PlayerData playerData;
    public PlayerMovement playerMovement;

    public void Awake()
    {
        GameObject[] playerObjs = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject playerObj in playerObjs)
        {
            if (playerObj.GetComponent<PlayerData>() != null)
            {
                playerData = playerObj.GetComponent<PlayerData>();
                playerMovement = playerObj.GetComponent<PlayerMovement>();
            }
        }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && playerData != null)
        {
            PickUp();
            GameObject.Destroy(gameObject);
        }
    }

    protected virtual void PickUp()
    {

    }
}
