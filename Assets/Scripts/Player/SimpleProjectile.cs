using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleProjectile : MonoBehaviour
{
    public float Life = 5f;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, Life);
    }

 

    private void OnTriggerEnter(Collider collider)
    {
        Destroy(gameObject); 
    }
}
