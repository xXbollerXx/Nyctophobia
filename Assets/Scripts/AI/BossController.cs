using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using System.Collections;

public class BossController : MonoBehaviour
{
    // Animation
    Animator animator;

    // Wander
    [Header("Wander Settings")]
    public float wanderRadius;
    public float wanderTimer;
    private float timer;

    // Lock On
    [Header("Targeting Settings")]
    public GameObject player;
    public float playerRadius;
    public float attackRadius;
    private NavMeshAgent agent;
    public float FireRate = 0.3f;
    private float NextFire = 0.0f;

    // Health
    [Header("Health Settings")]
    public float health;
    public float maxHealth;
    public float projectileDMG;
    public GameObject healthBarUI;
    public Slider slider;
    private bool stop;

    void OnEnable()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        timer = wanderTimer;
    }

    void Start()
    {
        health = maxHealth;
        slider.value = health / maxHealth;
        stop = false;
    }

    void Update()
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
            Vector3 facing = (player.transform.position - agent.transform.position).normalized;
            float dotProd = Vector3.Dot(facing, agent.transform.forward);

            // LockOn
            if (range < playerRadius)
            {
                agent.SetDestination(player.transform.position);
                animator.SetBool("isWalking", true);
                timer = wanderTimer;

                if (range < attackRadius && NextFire <= Time.time && dotProd > 0.99)
                {
                    attack();
                }
            }

            // Wander
            else if (timer >= wanderTimer)
            {
                animator.SetBool("isWalking", true);
                Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
                agent.SetDestination(newPos);
                timer = 0;
            }

            // Idle
            else if (agent.remainingDistance == 0)
            {
                animator.SetBool("isWalking", false);
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

    // Collision
    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<Collider>().name == "Projectile(Clone)")
        {
            health = health - projectileDMG;
            //animator.SetBool("isDMG", true);
            //stop = true;
        }
    }

    // Wander Calculation
    public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
    {
        Vector3 randDir = Random.insideUnitSphere * distance;
        randDir += origin;
        NavMeshHit navHit;
        NavMesh.SamplePosition(randDir, out navHit, distance, layermask);
        return navHit.position;
    }

    public void DestroyObj()
    {
        Destroy(gameObject);
    }

    //public void takeDMG()
    //{
        //animator.SetBool("isDMG", false);
        //stop = false;
    //}

    void StopMove()
    {
        agent.SetDestination(agent.transform.position);
    }

    void attack()
    {
        float num = Random.Range(0.0f, 10.0f);

        if (num <= 5.0f)
        {
            animator.SetBool("isSwing", true);
            stop = true;
        }
        else if (num >= 5.0f && num <= 10.0f)
        {
            animator.SetBool("isSwing2", true);
            stop = true;
        }
    }

    void attackend()
    {
        animator.SetBool("isSwing2", false);
        animator.SetBool("isSwing", false);
        stop = false;
    }
}

