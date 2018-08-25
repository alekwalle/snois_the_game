using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{

    [SerializeField] private float health = 100f;


    public void ApplyDOMAGE(float domage)
    {
        // Debug.Log("You fucked up son: " + domage);
        health -= domage;

        if(health <= 0f)
        {
            Destroy(gameObject);
        }
    }

}
