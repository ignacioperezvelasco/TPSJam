﻿
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyAi : MonoBehaviour
{
    private GameManager myGM;
    public AudioSource shotS;
    public GameObject[] mySpheres;
    private int counter = 0;
    public NavMeshAgent agent;
    public Slider myHealth;
    public Transform player;
    public LayerMask whatIsGround, whatIsPlayer, whatIsObstacle;
    public AudioSource hit;
    public AudioSource hitBubble;
    //patroling 
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    bool canShoot=false;
    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;
    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, PlayerInAttackRange;

    public float currentHealth;
    public float maxHealth=50;

    [SerializeField] float bulletSpeed=15;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = this.GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
        myGM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void Update()
    {
        myHealth.value = currentHealth / maxHealth;
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        PlayerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !PlayerInAttackRange)
            Patroling();
        if (playerInSightRange && !PlayerInAttackRange)
            ChasePlayer();
        if (playerInSightRange && PlayerInAttackRange)
        {
            canShoot = !Physics.CheckCapsule(transform.position, player.position,0.5f, whatIsObstacle);
            if (canShoot)
            {
                AttackPlayer();
            }
            else
            {
                ChasePlayer();
            }
        } 

    }

    private void Patroling()
    {        
        if (!walkPointSet)
            SearchWalkPoint();
        else
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //walkPointReached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void SearchWalkPoint()
    {
        //calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);
        transform.LookAt(new Vector3(player.position.x,this.transform.position.y,player.position.z));

        if (!alreadyAttacked)
        {
            shotS.pitch = Random.Range(0.7f, 1);
            shotS.Play();
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * bulletSpeed, ForceMode.VelocityChange);
            //AttackCode Here
            alreadyAttacked = true;
            timeBetweenAttacks = Random.Range(0.25f,1.5f);
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage + (counter*15);
        if (counter > 0)
        {
            hitBubble.Play();
            counter = 0;
            for (int i = 0; i < 3; i++)
            {
                mySpheres[i].SetActive(false);
            }
        }
        else
            hit.Play();
        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (myGM.numEnemies == 1)
        {
            myGM.WinPanel();
        }
        else
            Destroy(this.gameObject);
    }

    public void AwakeSphere()
    {
        if (counter < 3)
        {
            hit.Play();
            mySpheres[counter].SetActive(true);
            counter++;
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
