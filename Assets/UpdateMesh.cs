using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMesh : MonoBehaviour
{
    [SerializeField] private CelestialBodyGenerator[] generators;
    [SerializeField] private bool update;

    void Update()
    {
        foreach(CelestialBodyGenerator gen in generators)
        {
            if (update)
            {
                gen.SetLOD(1);
                gen.SetLOD(0);
               
            }
        }
        update = false;
    }

}
