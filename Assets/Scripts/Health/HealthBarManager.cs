using System;
using UnityEngine;

public class HealthBarManager : MonoBehaviour
{

    public GameObject healthBarPrefab;


    public void SpawnHealthBar(Health health)
    {
        var healthBar = Instantiate(healthBarPrefab, transform);
        healthBar.GetComponent<HealthBar>().Health = health;



    }
}
