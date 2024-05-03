using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatchScript : MonoBehaviour
{
    // this script is part of minigame #1a
    // it is attached to each damage and checks if the dirt covering the spots for patches are cleaned before they can be placed 

    [SerializeField] private GameObject dirt;
    
    public void CheckIfCleaned()
    {
        if (!dirt.activeSelf)
        {
            this.gameObject.GetComponent<Image>().raycastTarget = true;
        }
        else
        {
            this.gameObject.GetComponent<Image>().raycastTarget = false;
        }
    }
}
