using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PortalController : MonoBehaviour
{
    public string NextScene;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<PlayerController>())
        {
            Debug.Log("Palyer Enter");
            PlayerController player = other.GetComponentInParent<PlayerController>();
            player.LandingText.GetComponent<Text>().text = "Go To " + NextScene + " (E)";
            player.NextScene = NextScene;
            player.CanChangeScene = true;
        }
    }
}
