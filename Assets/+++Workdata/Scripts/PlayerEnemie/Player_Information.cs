using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerInformation : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 5;
    [SerializeField] private int currentHealth;

    [Header("Attack Settings")] 
    [SerializeField] private int attackDmg;
    
    [Header("Ui")]
    [SerializeField] private Image healthBar;

    [SerializeField] private GameObject deathMenu;

    [SerializeField] private float healthBarSpeed = .3f;
    
    [Header("Events")] 
    [SerializeField] UnityEvent OnDeath;
    [SerializeField] UnityEvent OnHeal;

    public UiManager ui;

    public Button firstDeath;
    //private Player

    //private ColorSpriteSetter[] _colorSpriteSetter;
    
    private void Start()
    {
        //_colorSpriteSetter = GetComponentsInChildren<ColorSpriteSetter>();
        currentHealth = maxHealth;
    }

    public void GetDamage(int dmg)
    {
        if (currentHealth <= 0) return;

       /* for (int i = 0; i < _colorSpriteSetter.Length; i++)
        {
            _colorSpriteSetter[i].ColorObject();
        }*/
        
        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            OnDeath?.Invoke();
            currentHealth = 0;
            
            if (deathMenu != null)
            {
                deathMenu.SetActive(true);
                Time.timeScale = 0f;
                ui.SelectButton(firstDeath);
            }
        }

        float targetFillAmount = (float)currentHealth / maxHealth;
        //healthBar.fillAmount = (float)currentHealth / maxHealth;
        StartCoroutine(UpdateHealthbar(targetFillAmount));
    }

    private IEnumerator UpdateHealthbar(float targetFillAmount) // 0.6
    {
        float elapsed = 0;
        float currentFillAmount = healthBar.fillAmount; // 0,8

        while (elapsed < healthBarSpeed)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / healthBarSpeed;
            float currentFill = Mathf.Lerp(currentFillAmount, targetFillAmount, t); // x
            healthBar.fillAmount = currentFill;
            yield return null;
        }

        healthBar.fillAmount = targetFillAmount;
    }

    public void Heal(int heal)
    {
        //if (currentHealth == maxHealth) return; //optional
        
        currentHealth += heal;

        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        
        OnHeal?.Invoke();
    }

    /*public void DamageEnemy(GameObject obj)
    {
        obj.GetComponent<EnemyInformations>().GetDamage(attackDmg);
    }*/
    
}