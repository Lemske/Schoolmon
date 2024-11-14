using System;
using UnityEngine;

public class Health : MonoBehaviour
{

    private HealthBarManager healthBarManager;

    public event Action<int> OnHealthChanged;
    private Monster monster;

    public int currentHealth;
    public int maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBarManager = FindFirstObjectByType<HealthBarManager>();
        healthBarManager.SpawnHealthBar(this);
        monster = GetComponent<Monster>();
    }

    public void TurnOffHealthBar()
    {
        healthBarManager.TurnOffHealthBar(this);
    }

    public void TurnOnHealthBar()
    {
        healthBarManager.TurnOnHealthBar(this);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        OnHealthChanged?.Invoke(currentHealth);
        if (currentHealth <= 0 && monster != null)
        {
            monster.EndGame(); //Todo: Weird implementation for this code
        }
    }
}
