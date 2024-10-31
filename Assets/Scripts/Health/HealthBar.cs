using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{

	[SerializeField] private Health health;

	public Slider slider;
	public Gradient gradient;
	public Image fill;

	public Health Health
	{
		get => health; set
		{
			if (health != null)
			{
				health.OnHealthChanged -= UpdateHealthBar;
			}

			health = value;
			slider.maxValue = Health.maxHealth;
			health.OnHealthChanged += UpdateHealthBar;
			UpdateHealthBar(health.currentHealth);
		}
	}


	public void UpdateHealthBar(int health)
	{
		slider.value = health;
		fill.color = gradient.Evaluate(slider.normalizedValue);
	}

}