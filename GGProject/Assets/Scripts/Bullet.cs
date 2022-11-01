using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject player;
    public PlayerData playerData;
    // Start is called before the first frame update
    void Start()
    {
        playerData = player.GetComponentInChildren<PlayerData>(); // this isn't working, you have to attach to the inspector but it won't allow me :/
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player hit");
            //playerData.Attacked();
            Destroy(this.gameObject);

        }
    }
}
