using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform targetPosition;
    float currentHealth = 0;
    float maxHealth = 100;
    [SerializeField] GameObject[] childs;
    private int counter = 0;
    [SerializeField] Transform spawnPoint;
    public Rigidbody bullet;
    public float bulletSpeed=10;
    bool chase = false;
    [SerializeField] float shootRate = 0.5f;
    float timerShoot;

    float timerTillNextShot = 0f;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        targetPosition = GameObject.FindGameObjectWithTag("Player").transform;        
    }

    private void Update()
    {
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
            if (timerShoot <= 0)
            {
                timerShoot = Random.Range(shootRate,(shootRate*2));
                Shoot();
            }
        }
        
    }

    void Shoot()
    {        
        Rigidbody bulletClone = (Rigidbody)Instantiate(bullet, spawnPoint.transform.position, spawnPoint.transform.rotation);
        bulletClone.gameObject.transform.LookAt(targetPosition.position);
        bulletClone.velocity = bulletClone.gameObject.transform.forward * bulletSpeed;
    }
    
    public void TakeDamage(float damage)
    {
        if (counter != 0)
        {
            currentHealth -= damage + (counter * 15);
            HideCrystals();
        }
        else
            currentHealth -= damage;
        

        if (currentHealth <= 0)
        {
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
            childs[counter].SetActive(true);
            counter++;
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
    
    
}
