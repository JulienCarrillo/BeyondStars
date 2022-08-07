using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class LevelController : MonoBehaviour
{
    public void Exit()
    {
        Debug.Log("ExitPanel");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        GameObject.FindGameObjectWithTag("LevelPicker").gameObject.SetActive(false);
    }
}
