using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int health = 100;

    public void Damage(int amount)
    {
        health -= 10;
    }
}
