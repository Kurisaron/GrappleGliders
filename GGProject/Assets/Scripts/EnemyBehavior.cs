using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private float enemySpeed, enemyRange1, enemyRange2;
    [SerializeField] private BoxCollider FieldOfView, enemyHitBox;
    [SerializeField] private Rigidbody enemyRigidbody, playerRigidbody;
    public PlayerData playerData;
    public Transform player;
    private bool canRandomMove;
    [SerializeField] private bool playerDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerData.local;
        playerRigidbody = playerData.gameObject.GetComponent<Rigidbody>();
        player = playerData.gameObject.transform;
        
        if (playerData == null)
        {
            Debug.Log("Player data could not be found to store in enemy.");
        }

        canRandomMove = true;
        enemyRigidbody = GetComponent<Rigidbody>();
        enemySpeed = 100;
        enemyRange1 = transform.position.x + 5;
        enemyRange2 = transform.position.x - 5;
    }

    // Update is called once per frame
    void Update()
    {
        float moveRadius = 10.0f;
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
            playerDetected = false;
            if (canRandomMove)
            {
                StartCoroutine(EnemyRandomMove());
            }
        }
    }

    

    public IEnumerator EnemyRandomMove() // the range the enemy moves in while not detecting the player
    {
        enemyRigidbody.velocity = new Vector3(0, 0, 0);
        canRandomMove = false;

        float randomMoveInterval = 2.0f;
        float forceInterval = randomMoveInterval / 10.0f;

        Vector3 moveDirection = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));

        for (float currentIntervalTime = randomMoveInterval; currentIntervalTime > 0; currentIntervalTime -= forceInterval)
        {
            enemyRigidbody.AddForce(moveDirection.normalized * enemySpeed * forceInterval);

            if (playerDetected)
            {
                currentIntervalTime = 0;
                
            }

            yield return new WaitForSeconds(forceInterval);
        }

        enemyRigidbody.velocity = new Vector3(0, 0, 0);
        canRandomMove = true;
    }

    public void EnemyAction() // enemy homing movement
    {
        playerDetected = true;
        transform.LookAt(player);
        Vector3 moveDirection = new Vector3(transform.forward.x, 0, transform.forward.z);
        enemyRigidbody.AddForce(moveDirection.normalized * enemySpeed * Time.deltaTime);
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
            playerRigidbody.AddForce((player.position - this.transform.position).normalized * 6, ForceMode.Impulse);

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
