using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    public float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            Destroy(this.gameObject);
        }

        timer -= Time.deltaTime;
    }
}
