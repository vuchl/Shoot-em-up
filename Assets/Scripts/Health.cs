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
    [HideInInspector] public float currentHealth;

    public Slider healthBarSlider;

    private IKillable killable;

    private void Start()
    {
        Reset();
    }

    private void OnEnable()
    {
        Reset();
    }

    public void TakeDamage(float amount)
    {
        if (!isServer)
            return;
        
        currentHealth -= amount;
        if (currentHealth <= 0)
        {
            print("Object dead");
            currentHealth = 0;
            Kill();
        }
    }

    void OnChangeHealth(float currentHealth)
    {
        healthBarSlider.value = currentHealth;
        this.currentHealth = currentHealth;
    }

    private void Reset()
    {
        currentHealth = maxHealth.Value;
        healthBarSlider.minValue = 0;
        healthBarSlider.maxValue = maxHealth;
        healthBarSlider.value = maxHealth;
    }

    private void Kill()
    {
        IKillable killable = gameObject.GetComponentInParent<IKillable>();
        if (killable != null)
        {
            killable.OnKilled();
        }
    }


}