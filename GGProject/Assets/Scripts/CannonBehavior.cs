using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehavior : MonoBehaviour
{
    [SerializeField] private GameObject enemyObject;
    private float enemySpeed, bulletLife, playerRadius, shootInterval;
    [SerializeField] private bool playerDetected = false;
    [SerializeField] private GameObject enemyBullet;
    private GameObject player;
    private PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        playerData = PlayerData.local;
        player = playerData.gameObject;
        
        enemySpeed = 10; // i think it has to be this quick :|
        bulletLife = 2;
        playerRadius = 30;
        shootInterval = 1;
    }

    private void Update()
    {
        if (Vector3.Distance(gameObject.transform.position, player.transform.position) <= playerRadius)
        {
            if (!playerDetected)
            {
                playerDetected = true;
                StartCoroutine(EnemyShoot());
            }
            
            
        }
        else
        {
            if (playerDetected)
            {
                playerDetected = false;
            }

        }
    }

    private IEnumerator EnemyShoot() //shoots the bullet
    {
        GameObject bullet;
        //Quaternion bulletRotation = new Quaternion(0, 90, 0, 0);
        while(playerDetected)
        {
            bullet = Instantiate(enemyBullet, enemyObject.transform.position, transform.rotation);
            bullet.GetComponent<Bullet>().Init(playerData, bulletLife);
            bullet.transform.LookAt(player.transform);
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * enemySpeed, ForceMode.Impulse);

            yield return new WaitForSeconds(shootInterval);

        }
    }
}
