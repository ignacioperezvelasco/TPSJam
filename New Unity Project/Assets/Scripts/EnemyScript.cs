﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    public AudioSource shotS;
    public AudioSource hit;
    public AudioSource hitBubble;
    public Transform targetPosition;
    public LayerMask whatIsObstacle;
    public Slider myHealth;
    float currentHealth = 0;
    float maxHealth = 100;
    [SerializeField] GameObject[] childs;
    private int counter = 0;
    [SerializeField] Transform spawnPoint;
    public Rigidbody bullet;
    public Rigidbody enemyP;
    public float bulletSpeed=10;
    bool chase = false;
    [SerializeField] float shootRate = 0.5f;
    float timerShoot;

    // Start is called before the first frame update

    [SerializeField] float spawnRate = 5;
    private float timerSpawn = 0;

    void Start()
    {
        currentHealth = maxHealth;
        targetPosition = GameObject.FindGameObjectWithTag("Player").transform;        
    }

    private void Update()
    {
        myHealth.value = currentHealth / maxHealth;
        if (((this.transform.position - targetPosition.position).magnitude < 28) && chase == false)
        {
            chase = true;
        }
        else if ((this.transform.position - targetPosition.position).magnitude > 28)
        {
            chase = false;
        }
        else if (chase)
        {
            timerShoot -= Time.deltaTime;
            timerSpawn -= Time.deltaTime;
            if (timerShoot <= 0)
            {
                timerShoot = Random.Range(shootRate,(shootRate*2));
                if(!Physics.CheckCapsule(transform.position, targetPosition.position, 0.5f, whatIsObstacle))
                    Shoot();
            }
            if (timerSpawn <= 0)
            {
                timerSpawn = Random.Range(spawnRate, (spawnRate * 2));
                SpawnEnemy();
            }
        }        
    }

    void SpawnEnemy()
    {
        Rigidbody enemyClone = (Rigidbody)Instantiate(enemyP, spawnPoint.transform.position, spawnPoint.transform.rotation);
        
    }

    void Shoot()
    {
        shotS.pitch = Random.Range(0.7f, 1);
        shotS.Play();
        Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
        bulletClone.gameObject.transform.LookAt(targetPosition.position);
        bulletClone.velocity = bulletClone.gameObject.transform.forward * bulletSpeed;
    }
    
    public void TakeDamage(float damage)
    {
        if (counter != 0)
        {
            hitBubble.Play();
            currentHealth -= damage + (counter * 15);
            HideCrystals();
        }
        else
        {
            hit.Play();
            currentHealth -= damage;
        }

        if (currentHealth <= 0)
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().AddMinute();
            Die();
        }
    }

    void HideCrystals()
    {
        for (int i = 0; i < counter; i++)
        {
            childs[i].SetActive(false);
            //Debug.Log("entro");
        }
        counter = 0;
    }

    public void AwakeCrystal()
    {
        if (counter < 3)
        {
            hit.Play();
            childs[counter].SetActive(true);
            counter++;
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
    
    
}
