using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodingSlimeController : MonsterController
{
    [Header("Exploding settings Settings")]

    public float explosionRadius;
    public AudioClip ExplodeSound;
    protected override void Update()
    {
        // Health
        slider.value = health / maxHealth;
        if (health < maxHealth)
        {
            healthBarUI.SetActive(true);
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        // Movement
        if (health > 0 && stop == false)
        {
            timer += Time.deltaTime;
            float range = Vector3.Distance(player.transform.position, agent.transform.position);

            if (range < explosionRadius)
            {
                animator.SetBool("isMoving", false);
                animator.SetBool("isExplo", true);
                stop = true;
            }

            // LockOn
            else if (range < playerRadius)
            {
                agent.SetDestination(player.transform.position);
                animator.SetBool("isMoving", true);
                timer = wanderTimer;
            }

            // Wander
            else if (timer >= wanderTimer)
            {
                animator.SetBool("isMoving", true);
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }

            // Idle
            else if (agent.remainingDistance == 0)
            {
                animator.SetBool("isMoving", false);
            }
        }
        // Stopping
        else if (health > 0 && stop == true)
        {
            StopMove();
        }
        // Death
        else
        {
            StopMove();
            animator.SetBool("isDead", true);
        }
    }

    public void Explode()
    {
        float range = Vector3.Distance(player.transform.position, agent.transform.position);
        if (range < explosionRadius)
        {
            PlayerMovement.Instance.gameObject.GetComponent<HealthBar>().health -= 45;
            DestroyObj();
        }
        else
        {
            DestroyObj();
        }
    }
    public void playSound()
    {
        audioSource.PlayOneShot(ExplodeSound, 0.7F);
    }
}