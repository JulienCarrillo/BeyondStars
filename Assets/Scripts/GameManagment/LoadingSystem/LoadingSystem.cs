using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoadingSystem : MonoBehaviour
{
    private GameObject cameraComponent;
    private CustomPostProcessing customPostProcessing;
    public GameObject Player;



    void Start()
    {
        cameraComponent = GameObject.FindGameObjectWithTag("MainCam");
        customPostProcessing = cameraComponent.GetComponent<CustomPostProcessing>();
        //customPostProcessing.effects[0] = null;
    }
    public void LoadLevel(int sceneIndex)
    {
        Destroy(Player);
        StartCoroutine(LoadAsynchronously(sceneIndex));
        Debug.Log("Finishjj");
    }
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        while (!operation.isDone)
        {
            float proggress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(proggress);
            yield return new WaitForSeconds(3);
        }
       
    }

    public void Exit()
    {
        Debug.Log("ExitPanel");
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        GameObject.FindGameObjectWithTag("LevelPicker").gameObject.SetActive(false);
    }
}
    
