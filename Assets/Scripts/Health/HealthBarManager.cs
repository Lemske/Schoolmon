using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{

    public GameObject healthBarPrefab;
    List<HealthBar> healthBars = new List<HealthBar>();

    public void SpawnHealthBar(Health health)
    {
        var healthBar = Instantiate(healthBarPrefab, transform);
        healthBars.Add(healthBar.GetComponent<HealthBar>());
        healthBar.GetComponent<HealthBar>().Health = health;
    }

    void Update()
    {
        foreach (var healthBar in healthBars)
        {
            healthBar.transform.position = Camera.main.WorldToScreenPoint(healthBar.Health.transform.position + Vector3.up * 0.2f);
        }
    }

    public void DestroyHealthBar(Health health)
    {
        Destroy(healthBars.Find(healthBar => healthBar.Health == health).gameObject);
        healthBars.Remove(healthBars.Find(healthBar => healthBar.Health == health));

    }
}
