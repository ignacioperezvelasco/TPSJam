using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float BulletDamage = 5;
    public float distanceCanReach=40;
    Vector3 initialPosition;

    private void Start()
    {
        initialPosition = this.transform.position;
    }

    private void Update()
    {
        Vector3 distanceV = (this.transform.position - initialPosition);        
        if (distanceV.magnitude >= distanceCanReach)
        {
            Die();
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<rvMovementPers>().TakeDamage(BulletDamage);
            Die();
        }
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
