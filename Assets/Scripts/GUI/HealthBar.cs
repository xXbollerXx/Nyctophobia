using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Settings")]
    public float health;
    public float maxHealth;
    public Canvas healthBarUI;
    public Slider slider;
    Animator animator;
    public float DMGRate = 0.8f;
    private float DMGFire;

    public static HealthBar Instance;
    //public GameObject Gameover;

    private AudioSource playersource;
    public AudioClip DeathSound;
    public AudioClip DamageSound;
    void Start()
    {
        animator = GetComponent<Animator>();
        playersource = GetComponent<AudioSource>();
        health = maxHealth;
        slider.value = health / maxHealth;
    }

    void Update()
    {
        slider.value = health / maxHealth;

        if (health <= 0)
        {
            animator.SetBool("isDead", true);
            PlayerMovement.Instance.is_dead = true;
            SceneManager.LoadScene(1);
        }
        else if (health > maxHealth)
        {
            health = maxHealth;
        }
    }

    void OnControllerColliderHit(ControllerColliderHit collision)
    {
        if (collision.gameObject.name == "YSlime" && DMGFire <= Time.time)
        {
            DMGFire = Time.time + DMGRate;
            health = health - 10;
            animator.SetFloat("dmgMultiplier", 10f);
            playersource.PlayOneShot(DamageSound, 0.7f);
            animator.SetBool("isDMG", true);
        }
        if (collision.gameObject.name == "Red Slime" && DMGFire <= Time.time)
        {
            DMGFire = Time.time + DMGRate;
            health = health - 10;
            animator.SetFloat("dmgMultiplier", 10f);
            playersource.PlayOneShot(DamageSound, 0.7f);
            animator.SetBool("isDMG", true);
        }
        if (collision.gameObject.name == "Green Slime" && DMGFire <= Time.time)
        {
            DMGFire = Time.time + DMGRate;
            health = health - 10;
            animator.SetFloat("dmgMultiplier", 10f);
            playersource.PlayOneShot(DamageSound, 0.7f);
            animator.SetBool("isDMG", true);
        }
        if (collision.gameObject.name == "Monkey" && DMGFire <= Time.time)
        {
            DMGFire = Time.time + DMGRate;
            health = health - 10;
            animator.SetFloat("dmgMultiplier", 15f);
            playersource.PlayOneShot(DamageSound, 0.7f);
            animator.SetBool("isDMG", true);
        }
    }
    
    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Collider>().name == "BananaProjectile(Clone)")
        {
            health = health - 10;
            animator.SetFloat("dmgMultiplier", 20f);
            playersource.PlayOneShot(DamageSound, 0.7f);
            animator.SetBool("isDMG", true);
        }
    }

    void endDMG()
    {
        animator.SetBool("isDMG", false);
    }
}
