using UnityEngine;

public abstract class Shockwave : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected int stage = 0;
    
    public float speed;
    public float maxSpeed;
    public float deceleration;
    public float maxDeceleration;
    public float distance;
    protected Vector3 startingPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        startingPos = transform.position;
        speed = maxSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shock()
    {
        stage = 1;
    }
}
