using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public PlayerData playerData;
    public PlayerMovement playerMovement;

    public void Start()
    {
        playerData = PlayerData.local;
        playerMovement = playerData.gameObject.GetComponent<PlayerMovement>();
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
