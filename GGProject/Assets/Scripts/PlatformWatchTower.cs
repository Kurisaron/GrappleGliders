using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlatformWatchTower : MonoBehaviour
{
    [SerializeField] private GameObject levelFinishedScreen;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /*private void OnCollisionEnter(Collision other)
    {

        if (other.gameObject.tag == "Player")
        {
            Debug.Log("player is on platform");
            levelFinishedScreen.SetActive(true);

        }
    }*/
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player is on platform");
            levelFinishedScreen.SetActive(true);

        }
    }
    public void OKButtonClicked()
    {
        Debug.Log("button clicked"); // insanity check
        levelFinishedScreen.SetActive(false);
    }
}
