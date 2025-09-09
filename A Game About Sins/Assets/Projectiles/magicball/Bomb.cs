using Unity.Mathematics;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float timer;
    public float xspeed;
    public float yspeed;
    public float slowDown;
    public GameObject explosion;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(xspeed * Time.deltaTime, yspeed * Time.deltaTime, 0));

        if (math.abs(xspeed) < slowDown * Time.deltaTime)
        {
            xspeed = 0;
        }
        else if (xspeed > 0)
        {
            xspeed -= (float)slowDown * Time.deltaTime;
        }
        else if (xspeed < 0)
        {
            xspeed += (float)slowDown * Time.deltaTime;
        }

        if (math.abs(yspeed) < slowDown * Time.deltaTime)
        {
            yspeed = 0;
        }
        else if (yspeed > 0)
        {
            yspeed -= (float)slowDown * Time.deltaTime;
        }
        else if (yspeed < 0)
        {
            yspeed += (float)slowDown * Time.deltaTime;
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            Instantiate(explosion, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
    }
}
