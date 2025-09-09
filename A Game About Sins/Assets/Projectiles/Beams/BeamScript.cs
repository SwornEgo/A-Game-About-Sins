using UnityEngine;

public class BeamScript : ProjectileScript
{
    private GameObject player;
    private Vector3 direction;
    private float timer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<Collider2D>().enabled = false;
        player = GameObject.Find("The Hero");
        timer = 0.625f;
        transform.right = player.transform.position - transform.position;
        transform.Rotate(new Vector3(0, 0, 90));


        if (UnityEngine.Random.Range(0, 2) == 1)
        {
            transform.Rotate(new Vector3(0, 0, -50));
            direction = new Vector3(0, 0, movement);

        }
        else
        {   
            transform.Rotate(new Vector3(0, 0, 50));
            direction = new Vector3(0, 0, -movement);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(direction * Time.deltaTime);

        if (timer <= -0.75)
        {
            this.GetComponent<Collider2D>().enabled = false;
        }
        else if (timer <= 0.2)
        {
            this.GetComponent<Collider2D>().enabled = true;
        }

        timer -= Time.deltaTime;
    }
}
