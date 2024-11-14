using System;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{
    [SerializeField] private float offset = 0.2f;
    public GameObject healthBarPrefab;
    List<HealthBar> healthBars = new List<HealthBar>();

    public void SpawnHealthBar(Health health)
    {
        var healthBar = Instantiate(healthBarPrefab, transform);
        healthBars.Add(healthBar.GetComponent<HealthBar>());
        healthBar.GetComponent<HealthBar>().Health = health;
        healthBar.SetActive(false);
    }

    void Update()
    {
        foreach (var healthBar in healthBars)
        {
            healthBar.transform.position = Camera.main.WorldToScreenPoint(healthBar.Health.transform.position + Vector3.up * offset);
        }
    }

    public void TurnOffHealthBar(Health health)
    {
        healthBars.Find(healthBar => healthBar.Health == health).gameObject.SetActive(false);
    }

    public void TurnOnHealthBar(Health health)
    {
        healthBars.Find(healthBar => healthBar.Health == health).gameObject.SetActive(true);
    }

    public void DestroyHealthBar(Health health)
    {
        Destroy(healthBars.Find(healthBar => healthBar.Health == health).gameObject);
        healthBars.Remove(healthBars.Find(healthBar => healthBar.Health == health));

    }
}
