using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehavior : MonoBehaviour
{
    [SerializeField] private GameObject enemyObject;
    [SerializeField] private float enemySpeed, bulletLife;
    [SerializeField] private BoxCollider FieldOfView;
    [SerializeField] private bool playerDetected = false;
    [SerializeField] private Rigidbody enemyBullet;
    public Transform player;
    public PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        enemySpeed = 10; // i think it has to be this quick :|
        bulletLife = 2;
    }

    // Update is called once per frame
    void Update()
    {
        /*if(playerDetected == true)
        {
            StartCoroutine(EnemyShoot());
        }
        else
        {
            StopCoroutine(EnemyShoot());
        }*/
    }
    IEnumerator EnemyShoot() //shoots the bullet
    {
        Rigidbody bullet;
        //Quaternion bulletRotation = new Quaternion(0, 90, 0, 0);
        while(true)
        {
            bullet = Instantiate(enemyBullet, enemyObject.transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().playerData = playerData;
            bullet.transform.LookAt(player);
            //bullet.transform.position += transform.forward * enemySpeed * Time.deltaTime;
            bullet.AddForce(bullet.transform.forward * enemySpeed, ForceMode.Impulse);
            Destroy(bullet.gameObject, bulletLife);
            yield return new WaitForSeconds(bulletLife);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //playerDetected = true;
            //FieldOfView.enabled = false;
            StartCoroutine(EnemyShoot());

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //playerDetected = true;
            //FieldOfView.enabled = false;
            Debug.Log("player out of view");
            StopAllCoroutines(); // stop coroutine didn't work so failsafe again
        }
    }
}
