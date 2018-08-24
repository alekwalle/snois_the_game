using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wepon : MonoBehaviour {


    private Animator anim;
    public int bulletsPerMag = 30;
    public int bulletsLeft;
    public float fireRate = 0.1f;
    public float weaponRange = 100f;
    float fireTimer;
    public Transform shootpoint;
    public int currentBullets;
    
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        currentBullets = bulletsPerMag;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetButton("Fire1"))
        {
            Fire(); // exec hvis du press elle hold knappen
        }

        if (fireTimer < fireRate)
            fireTimer += Time.deltaTime;
	}




    private void Fire()
    {
        if (fireTimer < fireRate) return; 

        RaycastHit hit;
        if(Physics.Raycast(shootpoint.position, shootpoint.forward, out hit, weaponRange))
        {
            Debug.Log(hit.transform.name + " found ");
        }


        anim.SetBool("Fire", true);
        currentBullets--;
        fireTimer = 0.0f; //resetter fireTimer
    }
}
