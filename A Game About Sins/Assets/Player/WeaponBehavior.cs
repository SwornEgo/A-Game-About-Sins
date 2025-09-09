using UnityEngine;

public class WeaponBehavior : MonoBehaviour
{
    // Direction Constants
    private string UP = "Up";
    private string DOWN = "Down";
    private string LEFT = "Left";
    private string RIGHT = "Right";

    public float momentum;
    public float deceleration;
    public string direction;
    public float decay;
    public float offset;
    public float delay;
    public float decelDelay;

    private bool decaying = false;
    private float speed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(new Vector3(0, speed * Time.deltaTime, 0));

        if (decaying == true)
        {
            decay -= Time.deltaTime;

            if (decay <= 0)
            {
                destroy();
            }
        }

        if (speed <= 0 && decaying == false)
        {
            speed = 0;
            decaying = true;
            print("decay");
        }
        else if (decelDelay <= 0 && decaying == false)
        {
            speed -= deceleration * Time.deltaTime;
            print("decel");
        }
        else
        {
            decelDelay -= Time.deltaTime;
            print("delay");
        }
    }

    public void destroy()
    {
        Destroy(gameObject);
    }

    // Is invoked by the player after initialization
    public void Init(string playerDirection)
    {
        speed = momentum;
        direction = playerDirection;

        if (direction.Equals(UP) & speed > 0)
        {
            transform.Rotate(new Vector3(0, 0, 0));
        }

        else if (direction.Equals(DOWN))
        {
            transform.Rotate(new Vector3(0, 0, 180));
        }

        else if (direction.Equals(LEFT))
        {
            transform.Rotate(new Vector3(0, 0, 90));
        }

        else if (direction.Equals(RIGHT))
        {
            transform.Rotate(new Vector3(0, 0, -90));
        }
        
        transform.Translate(new Vector3(0, offset, 0));
    }
}
