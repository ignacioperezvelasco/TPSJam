using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    float BulletDamage = 5;
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
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyScript>().TakeDamage(BulletDamage);
            Die();
        }
        else if (other.tag == "EnemyFollow")
        {
            other.GetComponent<EnemyAi>().TakeDamage(BulletDamage);
            other.GetComponent<Rigidbody>().AddForce(new Vector3(this.gameObject.transform.forward.x,0, this.gameObject.transform.forward.z)*10,ForceMode.VelocityChange);
            Die();
        }
        else if (other.tag == "Obstacle")
            Die();

    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
