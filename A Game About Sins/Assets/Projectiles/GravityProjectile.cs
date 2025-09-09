using UnityEngine;
using UnityEngine.UIElements;

public class GravityProjectile : ProjectileScript
{
    // Variables
    private Rigidbody2D rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        SetStraightVelocity();
        
    }

    void Update()
    {
        transform.right = rb.linearVelocity;

        if (transform.position.x > 10 || transform.position.y > 10 || transform.position.x < -10 || transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }

    private void SetStraightVelocity()
    {
        rb.linearVelocity = transform.right * movement;
    }
}
