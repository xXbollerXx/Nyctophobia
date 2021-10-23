using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShoot : MonoBehaviour
{
    Animator animator;
    public Transform ShotStart;
    public GameObject ProjectilePrefab;
    public float ProjectileSpeed;
    private float FireRate = 0.5f;
    private float NextFire;

    // Audio
    public AudioClip shoot;
    AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = this.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (NextFire <= Time.time)
            {
                animator.SetFloat("shootMultiplier", 2.0f);
                animator.SetBool("isShooting", true);
            }
        }
    }

    public void ShootEnd()
    {
        animator.SetBool("isShooting", false);
    }

    public void castProjectile()
    {
        audioSource.PlayOneShot(shoot, 0.7F);
        NextFire = Time.time + FireRate;
        GameObject Temp = Instantiate(ProjectilePrefab, ShotStart.position, ShotStart.rotation);
        Temp.GetComponent<Rigidbody>().velocity = Temp.transform.forward * ProjectileSpeed;
    }
}
