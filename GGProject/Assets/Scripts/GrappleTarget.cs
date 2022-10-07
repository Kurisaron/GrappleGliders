using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleTarget : MonoBehaviour
{
    public GameObject target;
    
    void Awake()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit");
        target.GetComponent<MeshRenderer>().material.color = Color.green;
    }

    void OnTriggerExit(Collider other)
    {
        target.GetComponent<MeshRenderer>().material.color = Color.gray;
    }
    
}
