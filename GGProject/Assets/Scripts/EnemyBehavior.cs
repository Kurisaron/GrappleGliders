using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private float enemySpeed, enemyRange1, enemyRange2;
    [SerializeField] private BoxCollider FieldOfView, enemyHitBox;
    [SerializeField] private Rigidbody playerRigidbody;
    public PlayerData playerData;
    public Transform player;
    private Vector3 move = new Vector3(1, 0, 0);
    [SerializeField] private bool playerDetected = false;

    // Start is called before the first frame update
    void Awake()
    {
        enemySpeed = 3;
        enemyRange1 = transform.position.x + 5;
        enemyRange2 = transform.position.x - 5;
    }

    // Update is called once per frame
    void Update()
    {
        float moveRadius = 5f;
        /*if (playerDetected == false)
        {
            EnemyRangeMove();
        }
        else
        {
            EnemyAction();
        }*/
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= moveRadius)
        {
            EnemyAction();
        }
        else
        {
            EnemyRangeMove();
        }
    }
    public void EnemyRangeMove() // the range the enemy moves in while not detecting the player
    {
        transform.position += move * enemySpeed * Time.deltaTime;
        if (enemyRange2 >= transform.position.x)
        {
            //Debug.Log(enemyRange);
            transform.Rotate(0, 180, 0);
            move.x *= -1;
        }
        if (enemyRange1 <= transform.position.x)
        {
            //Debug.Log(enemyRange);
            transform.Rotate(0, 180, 0);
            move.x *= -1;
        }
    }
    public void EnemyAction() // enemy homing movement
    {
        transform.LookAt(player);
        transform.position += transform.forward * enemySpeed * Time.deltaTime;
        //Debug.Log("player detected");
        float distance = Vector3.Distance(player.transform.position, this.transform.position);
        /*if(distance > FieldOfView.size.z)
        {
            FieldOfView.enabled = true;
        }*/
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player hit");
            playerData.Attacked();
            playerRigidbody.AddForce((player.transform.position - this.transform.position).normalized * 6, ForceMode.Impulse);

        }
    }
    /*private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player hit");
            playerData.Attacked();


        }
    }*/
}
