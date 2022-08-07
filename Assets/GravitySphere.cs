using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravitySphere : MonoBehaviour
{
    public FauxGravityAttractor Attractor;
    void Start()
    {
        Attractor = GetComponentInParent<FauxGravityAttractor>();
    }
    private void OnTriggerEnter(Collider other)
    {
        FauxGravityBody Body = other.GetComponent<FauxGravityBody>();
        if(Body != null)
        {
            Body.attractor = Attractor;
        }
        
    }
}
