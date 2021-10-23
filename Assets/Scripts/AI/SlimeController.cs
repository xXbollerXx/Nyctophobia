using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonsterController
{
    private bool is_dead;
    protected override void Update()
    {
        if (is_dead)
        {
            return;
        }
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

            // LockOn
            if (range < playerRadius)
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
            is_dead = true;
            StopMove();
            animator.SetBool("isDead", true);

            if (gameObject.name == "YSlime")
            {
                PlayerMovement.Instance.gameObject.GetComponent<HealthBar>().health += 10;
            }
        }
    }
}
