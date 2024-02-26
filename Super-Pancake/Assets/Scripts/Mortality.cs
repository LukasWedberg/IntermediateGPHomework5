using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mortality : MonoBehaviour
{
    public float currentHealthPoints = 10;

    bool currentlyAlive = true;


    public void TakeDamage(float damageAmount)
    {
        currentHealthPoints -= damageAmount;

        if (currentHealthPoints <= 0)
        {
            currentlyAlive = false;
        }
    }

    
}
