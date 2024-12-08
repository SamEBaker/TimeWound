using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using UnityEngine.Pool;
//https://github.com/SamEBaker/M02_DV03
//GPT helped with editing navmesh 
public class EnemyBehavior : MonoBehaviour
{
    public List<GameObject> players;       
    public float detectionRadius = 15f;    
    public float attackRange = 10f;         
    public float patrolSpeed = 10f;         
    public float chaseSpeed = 15f;         
    public float fleeSpeed = 12f;          
    public float shootInterval = 1f;      
    public GameObject Bullet;              
    public Transform shootPoint;          
    public float projectileSpeed = 15f;    
    private NavMeshAgent navAgent;         
    private float timeSinceLastShot = 0f;  
    private Transform currentTarget;      
    public int enemyHealth = 30;
    public float patrolAreaRadius = 500f;   
    public GameManager gm;
    public Animator anim;

    public AudioSource audio;
    public AudioClip Damaged;
    public GameObject BulletSpawn;
    public IObjectPool<EnemyBehavior> Pool { get; set; }

    private bool isFleeing = false;  
    private float fleeDuration = 3f; 
    private float fleeTimer = 0f;    

    public float rotationSpeed = 350f; 

    public void Start()
    {
        gm = FindObjectOfType<GameManager>();
        navAgent = GetComponent<NavMeshAgent>();
        navAgent.speed = patrolSpeed;  
        navAgent.angularSpeed = rotationSpeed;
        anim.SetBool("IsRunning", true);
        enemyHealth = 30;
        if (players.Count == 0)
        {
            // Find all players if not assigned
            players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
        }
        Patrol();
    }

    void Update()
    {
        if (isFleeing)
        {
            fleeTimer += Time.deltaTime;
            if (fleeTimer >= fleeDuration)
            {
                
                isFleeing = false;
                fleeTimer = 0f;
                Patrol(); //or back to chase
            }
            return; 
        }


        Transform closestPlayer = FindClosestPlayer();

        if (closestPlayer != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, closestPlayer.position);

            // If the closest player is within the detection radius, start chasing
            if (distanceToPlayer <= detectionRadius)
            {
                currentTarget = closestPlayer;
                ChasePlayer(closestPlayer);

                // If within attack range, shoot the player
                if (distanceToPlayer <= attackRange)
                {
                    AttackPlayer();
                }
            }
            else
            {
                // If the player is out of detection range, continue patrolling
                Patrol();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            audio.clip = Damaged;
            audio.Play();
            TakingDamage();
        }
    }

    Transform FindClosestPlayer()
    {
        Transform closestPlayer = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject player in players)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);
            if (distanceToPlayer < closestDistance)
            {
                closestDistance = distanceToPlayer;
                closestPlayer = player.transform;
            }
        }

        return closestPlayer;
    }

    void ChasePlayer(Transform player)
    {
        navAgent.speed = chaseSpeed; 
        navAgent.SetDestination(player.position); 
    }

    void AttackPlayer()
    {
        timeSinceLastShot += Time.deltaTime;

        if (timeSinceLastShot >= shootInterval)
        {
            timeSinceLastShot = 0f;
            ShootAtPlayer(currentTarget);
        }
    }

    // Shoot a projectile at the player
    void ShootAtPlayer(Transform player)
    {
        Rigidbody rb = Instantiate(Bullet, BulletSpawn.transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        Vector3 direction = (player.position - shootPoint.position).normalized;
        //rb.velocity = direction * projectileSpeed;
        rb.AddForce(direction * 15f, ForceMode.Impulse);
    }


    void Patrol()
    {
        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance)
        {
            Vector3 randomDirection = Random.insideUnitSphere * patrolAreaRadius;
            randomDirection += transform.position;

            // Find a valid point on the NavMesh to move to
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomDirection, out hit, patrolAreaRadius, NavMesh.AllAreas))
            {
                navAgent.SetDestination(hit.position); 
            }
            else
            {
                // If no valid NavMesh position was found, try again
                Patrol();
            }
        }
    }

    public void TakingDamage()
    {
        if (enemyHealth > 0)
        {
            enemyHealth -= 10;
            audio.clip = Damaged;
            audio.Play();

            FleeFromPlayer();
        }
        else
        {
            gm.TotalAddGold(5); 
            audio.clip = Damaged;
            audio.Play();
            ReturnToPool(); 
        }
    }

    void FleeFromPlayer()
    {
        isFleeing = true; 
        fleeTimer = 0f;   
        navAgent.speed = fleeSpeed; 


        Transform closestPlayer = FindClosestPlayer();
        if (closestPlayer != null)
        {
            Vector3 directionAwayFromPlayer = transform.position - closestPlayer.position;
            Vector3 fleePosition = transform.position + directionAwayFromPlayer.normalized * patrolAreaRadius;


            NavMeshHit hit;
            if (NavMesh.SamplePosition(fleePosition, out hit, patrolAreaRadius, NavMesh.AllAreas))
            {
                navAgent.SetDestination(hit.position);
            }
            else
            {
                // If no valid position is found, just move to a random point within the patrol area
                Patrol();
            }
        }
    }

    private void ReturnToPool()
    {
        Pool.Release(this);
    }
}
