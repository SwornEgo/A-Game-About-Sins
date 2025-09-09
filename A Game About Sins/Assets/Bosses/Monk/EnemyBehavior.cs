using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public float maxHealth;
    public float enemyHealth;
    [SerializeField] BarController healthBar;

    void Awake()
    {
        healthBar = GetComponentInChildren<BarController>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent("WeaponBehavior") != null)
        {
            print("hit!");
            HealthUpdate(-25);
            collision.gameObject.GetComponentInParent<WeaponBehavior>().destroy();
        }
    }

    public void HealthUpdate(float amount)
    {
        enemyHealth += amount;
        print(enemyHealth);
        healthBar.UpdateBar(enemyHealth, maxHealth);
    }
}
