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

    // Start is called before the first frame update
    void Start()
    {
        enemySpeed = 100; // i think it has to be this quick :|
        bulletLife = 3;
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
            bullet = Instantiate(enemyBullet, enemyObject.transform.position, enemyObject.transform.rotation);
            bullet.transform.LookAt(player);
            //bullet.transform.position += transform.forward * enemySpeed * Time.deltaTime;
            bullet.AddForce(transform.forward * enemySpeed, ForceMode.Impulse);
            Destroy(bullet.gameObject, bulletLife);
            yield return new WaitForSeconds(bulletLife);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            //playerDetected = true;
            //FieldOfView.enabled = false;
            StartCoroutine(EnemyShoot());

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //playerDetected = true;
            //FieldOfView.enabled = false;
            StopCoroutine(EnemyShoot());
        }
    }
}
