using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public int SceneIndex;
    public LoadingSystem loadingSystem;
    
    public void EnterScene(int SceneIndex)
    {
        loadingSystem.LoadLevel(SceneIndex);
    }
}
