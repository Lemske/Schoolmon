using System;
using UnityEngine;

public class Health : MonoBehaviour
{

    private HealthBarManager healthBarManager;

    public event Action<int> OnHealthChanged;

    public int currentHealth;
    public int maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBarManager = FindFirstObjectByType<HealthBarManager>();
        healthBarManager.SpawnHealthBar(this);
    }


    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth);
    }



}
