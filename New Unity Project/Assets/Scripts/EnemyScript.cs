using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform targetPosition;
    float currentHealth = 0;
    float maxHealth = 100;
    public List<GameObject> childs;
    private int counter = 0;
    Transform spawnPoint;
    public Rigidbody bullet;
    public float bulletspeed=60;
    bool chase = false;

    float timerTillNextShot = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoint = this.gameObject.GetComponentInParent<Transform>();
        currentHealth = maxHealth;
        targetPosition = GameObject.FindGameObjectWithTag("Player").transform;
        for (var i = 0; i < transform.childCount; i++)
        {
            childs.Add(transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {       
        if ((this.transform.position - targetPosition.position).magnitude<100)
        {
            chase = true;
        }
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

       // Debug.Log("CurrentHealth : " + currentHealth);

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
        if (counter < this.transform.childCount)
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
