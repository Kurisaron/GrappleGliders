using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [SerializeField] private GameObject homingEnemy;
    [SerializeField] private float enemySpeed = 10;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    void Move()
    {
        transform.position += new Vector3(0, 0, -1) * enemySpeed * Time.deltaTime;
    }
    
}
