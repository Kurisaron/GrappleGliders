using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialZoneController : MonoBehaviour
{
    public TutorialInfo tutorialInfo;
    private PlayerUIManager playerUIManager;

    private void Start()
    {
        playerUIManager = PlayerUIManager.local;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerUIManager.SetTutorialInfo(true, tutorialInfo);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerUIManager.SetTutorialInfo(false);
        }
    }
}
