using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private float enemySpeed, enemyRange;
    [SerializeField] private BoxCollider FieldOfView;
    public Transform player;
    private Vector3 move = new Vector3(1, 0, 0);
    [SerializeField] private bool playerDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        enemySpeed = 7;
        enemyRange = 10;

    }

    // Update is called once per frame
    void Update()
    {

        if (playerDetected == false)
        {
            EnemyRangeMove();
        }
        else
        {
            EnemyAction();
        }
    }
    public void EnemyRangeMove()
    {
        transform.position += move * enemySpeed * Time.deltaTime;
        if (-enemyRange >= transform.position.x)
        {
            transform.Rotate(0, 180, 0);
            move.x *= -1;
        }
        if (enemyRange <= transform.position.x)
        {
            transform.Rotate(0, 180, 0);
            move.x *= -1;
        }
    }
    public void EnemyAction()
    {
        transform.LookAt(player);
        transform.position += transform.forward * enemySpeed * Time.deltaTime;
        Debug.Log("player detected");
        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        /*if(distance > FieldOfView.size.z)
        {
            FieldOfView.enabled = true;
        }*/
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            playerDetected = true;
            FieldOfView.enabled = false;
            
        }
    }
}
