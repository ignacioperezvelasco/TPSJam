﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalScript : MonoBehaviour
{
    Vector3 initialPosition;
    public float distanceCanReach = 40;

    private void Start()
    {
        initialPosition = this.transform.position;
    }

    private void Update()
    {
        Vector3 distanceV = (this.transform.position - initialPosition);
        if (distanceV.magnitude >= distanceCanReach)
            Die();        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            other.GetComponent<EnemyScript>().AwakeCrystal();
            Die();
        }
        else if (other.tag == "EnemyFollow")
        {
            other.GetComponent<EnemyAi>().AwakeSphere();
            Die();
        }
        else if(other.tag == "Obstacle")
            Die();
        
    }

    private void Die()
    {
        Destroy(this.gameObject);
    }
}
