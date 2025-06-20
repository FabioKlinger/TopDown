using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = System.Object;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField]private float startingHealth;
    public float currentHealth;
    public Animator anim;
    
    private bool isDead;
    
    
    private Collider2D[] colliders; // alle Collider die der Gegner hat

    private void Awake()
    {
        currentHealth = startingHealth;
        colliders = GetComponents<Collider2D>();
    }

    public void TakeDamage(float damage)
    {
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, startingHealth); //Reduziert die Leben des Gegners. Sorgt gleichzeitig dafür, das die Leben nicht unter 0 fallen.

        if (currentHealth > 0)
        { 
           //anim.SetTrigger("hit");
        }
        else
        {
            if(!isDead)
            {
                
                isDead = true;

                DisableColliders();
                //anim.SetTrigger("die");
                StartCoroutine(RemoveAfterDeath()); //Eine Coroutine ist eine Funktion, die über mehrere Frames hinweg ausgeführt wird. 
            }
        }
    }

    private void DisableColliders()
    {
        //deaktiviert alle Collider des Gegners auf ein Mal
        foreach (var collider in colliders) //Für jedes Element im Array colliders (was jeweils ein Collider2D ist) wird der Code innerhalb der Schleife ausgeführt, wobei das aktuelle Element als collider bezeichnet wird.
        {
            collider.enabled = false;
        }
    }
    private IEnumerator RemoveAfterDeath()
    {
        yield return new WaitForSeconds(1.5f); // Warten für 1,5 Sekunden
        Destroy(gameObject);
    }
    
    
}