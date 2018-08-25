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
    private AudioSource _AudioSource;
    public AudioClip shootSound;
    public ParticleSystem muzzleFlash;
    private bool isReloading;
    private bool shootInput;
    public float domage = 20f;

    public enum ShootMode { Auto, Semi}
    public ShootMode shootingMode;


    private Vector3 originalPosition;
    public Vector3 aimPosition;
    public float aodSpeed;

    [Header("WepansConfig")]
    public GameObject hitParticles;
    public GameObject bulletImapt;
    
	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
        _AudioSource = GetComponent<AudioSource>();
        currentBullets = bulletsPerMag;

        originalPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void Update () {

        switch(shootingMode)
        {
            case ShootMode.Auto:
                shootInput = Input.GetButton("Fire1");
            break;

            case ShootMode.Semi:
                shootInput = Input.GetButtonDown("Fire1");
            break;
        }
        if (shootInput)
        {
            if (currentBullets > 0)
                Fire(); // exec hvis du press elle hold knappen
            else if(bulletsLeft > 0)
                DoReload();
        }

        if (fireTimer < fireRate)
            fireTimer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
        {
            if(currentBullets < bulletsPerMag && bulletsLeft > 0)
            DoReload();
        }

        AimDownSights();
	}

    void FixedUpdate()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);
        isReloading = info.IsName("Reload");
        //if (info.IsName("Fire")) anim.SetBool("Fire", false);
    }

    private void AimDownSights()
    {
        if(Input.GetButton("Fire2") && !isReloading)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimPosition, Time.deltaTime * aodSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, originalPosition, Time.deltaTime * aodSpeed);
        }
    }


    private void Fire()
    {
        if (fireTimer < fireRate || currentBullets <= 0 || isReloading)
            return; 

        RaycastHit hit;
        if(Physics.Raycast(shootpoint.position, shootpoint.forward, out hit, weaponRange))
        {
            Debug.Log(hit.transform.name + " found ");
            GameObject hitParticleEffect = Instantiate(hitParticles, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
           // hitParticleEffect.transform.SetParent(hit.transform);
            GameObject bulletHole = Instantiate(bulletImapt, hit.point, Quaternion.FromToRotation(Vector3.forward, hit.normal));

            Destroy(hitParticleEffect, 1f);
            Destroy(bulletHole, 1f);

            if(hit.transform.GetComponent<HealthController>())
            {
                hit.transform.GetComponent<HealthController>().ApplyDOMAGE(domage);
            }
        }


        anim.CrossFadeInFixedTime("Fire", 0.01f); //Plays the fire animation
        
        currentBullets--;
        fireTimer = 0.0f; //resetter fireTimer
        PlayShootSound(); //Spiller av soundclip
        muzzleFlash.Play();


    }

    public void Reload()
    {
        if (bulletsLeft <= 0) return;

        int bulletsToLoad = bulletsPerMag-currentBullets; //Sjekker hvor mange kuler som addes til maget

        //                                IF                then      1st   else    2nd
        int bulletsToDeduct = (bulletsLeft >= bulletsToLoad) ? bulletsToLoad : bulletsLeft;

        bulletsLeft -= bulletsToDeduct;
        currentBullets += bulletsToDeduct;

    }

    private void DoReload()
    {
        AnimatorStateInfo info = anim.GetCurrentAnimatorStateInfo(0);

        if (isReloading) return;
        
        anim.CrossFadeInFixedTime("Reload", 0.01f);
    }


    private void PlayShootSound()
    {
        _AudioSource.PlayOneShot(shootSound);
        //_AudioSource.clip = shootSound;
        //_AudioSource.Play();
    }
}
