using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using RoboRyanTron.Unite2017.Variables;

public class Health : NetworkBehaviour
{
    [SerializeField]
    public FloatReference maxHealth;

    [SyncVar(hook = "OnChangeHealth")]
    public float currentHealth;

    public RectTransform healthBar;

    private void OnEnable()
    {
        currentHealth = maxHealth.Value;
    }

    public void TakeDamage(float amount)
    {
        if (!isServer)
            return;

        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
    }

    void OnChangeHealth(float health)
    {
        healthBar.sizeDelta = new Vector2(health, healthBar.sizeDelta.y);
    }
}