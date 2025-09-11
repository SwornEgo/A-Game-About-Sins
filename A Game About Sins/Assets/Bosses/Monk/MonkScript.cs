using System;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UIElements;

public class MonkScript : MonoBehaviour
{
    // Constants
    public enum States { BETWEEN, SLAM, SHOCK, SPEW, BEAM };
    [SerializeField] private States _state;
    public Animator animatorController;

    // Public Variables
    public int test;
    public float test_speed, test_max_speed, test_timer;
    public GameObject player;
    public Camera gameCamera;
    public TextAsset textPatterns;

    /// Projectiles
    public GameObject targetProjectile, gravityProjectile, beam;

    // Private Variables
    public int stage = 1;
    public int glidePos;
    private float timer = 0, attackTimer = 0;
    private int counter = 0;
    private int direction = 1;
    private float distance = 0;
    private Vector2 startingPos;
    public Vector3 point = new Vector3(0, 0, 0);
    public Vector3 velocity;
    private GameObject boss;
    private Rigidbody2D RBody;
    private Shockmaker shockTrigger;

    // Arrays of points for the in between state
    private Vector3[] glidePatterns;

    void Awake()
    {
        string line = textPatterns.text;
        int lines = 0;

        while (line != "")
        {
            if (line[0] == '\n')
            {
                lines += 1;
            }

            line = line.Substring(1);
        }

        glidePatterns = new Vector3[lines];

        line = textPatterns.text;

        int xyzPos = 0;
        int patternPos = 0;
        String[] xyz = new String[3];

        while (line != "")
        {
            if (line[0] == ' ')
            {
                xyzPos++;
            }
            else if (line[0] == '\n')
            {
                xyzPos = 0;
                glidePatterns[patternPos] = new Vector3(float.Parse(xyz[0]), float.Parse(xyz[1]), float.Parse(xyz[2]));
                patternPos += 1;
                xyz = new String[3];
            }
            else
            {
                xyz[xyzPos] += line[0];
            }

            line = line.Substring(1);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        boss = gameObject;
        startingPos = transform.position;
        RBody = transform.GetComponentInChildren<Rigidbody2D>();
        shockTrigger = transform.parent.GetComponentInChildren<Shockmaker>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_state)
        {
            default:
                _state = States.BETWEEN;
                break;
            case States.BETWEEN:
                between_attacks();
                break;
            case States.SLAM:
                attack_SlamPlayer();
                break;
            case States.SHOCK:
                attack_ShockWaveLeft();
                break;
            case States.SPEW:
                attack_Spew();
                break;
            case States.BEAM:
                attack_Beams();
                break;
        }

        animate();
    }

    private void between_attacks()
    {
        switch (stage)
        {
            default:
                stage = 1;
                break;
            // Set a target and initial Velocity
            // Should add a way to randomize velocity and possible path
            case 1:
                velocity = new Vector3(0, 0, 0);
                glidePos = UnityEngine.Random.Range(0, 6);
                point = glidePatterns[glidePos];
                timer = 10;
                attackTimer = (float)1.5;
                stage += 1;
                break;

            // Finds the distance between the target and boss and accelerates towards it until it reaches a range around the point
            case 2:
                distance = Vector3.Distance(transform.position, point);
                velocity = glide(transform.position, point, velocity);
                transform.Translate(velocity * Time.deltaTime);

                if (distance < 1)
                {
                    if (glidePos != glidePatterns.Length - 1)
                    {
                        glidePos += 1;
                    }
                    else
                    {
                        glidePos = 0;
                    }
                }

                point = glidePatterns[glidePos];

                timer -= Time.deltaTime;
                attackTimer -= Time.deltaTime;

                if (attackTimer <= 0)
                {
                    attackTimer = (float)1.5;
                    Instantiate(targetProjectile, transform.position, Quaternion.identity);
                }

                if (timer <= 0)
                {
                    timer = 1;
                    stage += 1;
                }
                break;
            case 3:
                int randattack = UnityEngine.Random.Range(0, 5);

                if (randattack == 0)
                {
                    glidePos = UnityEngine.Random.Range(0, 6);
                    point = glidePatterns[glidePos];
                    timer = 5;
                    attackTimer = (float)1.5;
                    stage = 2;
                }
                else
                {
                    stage = 0;
                    _state = (States)randattack;
                }

                break;
        }
    }

    private void attack_SlamPlayer()
    {
        switch (stage)
        {
            default:
                stage = 1;
                break;
            case 1:
                point = new Vector3(player.transform.position.x, 3.5F, 0);
                distance = Vector3.Distance(transform.position, point);

                if (distance <= 10 * Time.deltaTime)
                {
                    transform.position = point;
                }
                else
                {
                    Vector2 direction = point - transform.position;
                    transform.rotation = Quaternion.FromToRotation(Vector3.up, direction);
                    transform.Translate(0, 10 * Time.deltaTime, 0);
                }

                if (boss.transform.position == point)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    stage = 2;
                    timer = 0.5F;
                }
                break;

            case 2:
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    stage = 3;
                }
                break;

            case 3:
                transform.Translate(new Vector3(0, -30, 0) * Time.deltaTime);

                if (transform.position.y < -10)
                {
                    transform.position = new Vector3(0, startingPos.y + 8, 0);
                    stage = 4;
                }
                break;
            case 4:
                transform.Translate(0, -25 * Time.deltaTime, 0);

                if (transform.position.y <= startingPos.y)
                {
                    transform.position = startingPos;
                    timer = 2;
                    stage = 5;
                }
                break;
            case 5:
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    _state = States.BETWEEN;
                    stage = 0;
                }
                break;

        }
    }

    private void attack_ShockWaveLeft()
    {
        switch (stage)
        {
            default:
                transform.position = startingPos;
                stage = 1;
                break;

            // Starts the timer
            case 1:
                timer = 2;
                stage += 1;
                break;

            // Chooses a Direction
            case 2:
                distance = UnityEngine.Random.Range(0, 2);
                if (distance == 0)
                {
                    distance = -1;
                }

                stage += 1;
                break;

            // Indication of attack
            case 3:
                if (transform.position.x > startingPos.x + 0.25)
                {
                    direction = -1;
                }
                else if (transform.position.x < startingPos.x - 0.25)
                {
                    direction = 1;
                }

                transform.Translate(new Vector3(3 * direction * Time.deltaTime, 0));

                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    stage += 1;
                }

                break;

            // Charges in specified direction
            case 4:
                transform.Translate(20 * distance * Time.deltaTime, 0, 0);

                if ((distance == 1 & transform.position.x > 9)
                || (distance == -1 & transform.position.x < -9))
                {
                    stage += 1;
                }

                break;

            case 5:
                if (distance == 1)
                {
                    shockTrigger.shockActivator("right");
                }
                else
                {
                    shockTrigger.shockActivator("left");
                }

                stage += 1;
                break;
            case 6:
                _state = States.BETWEEN;
                stage = 0;
                break;
        }
    }

    private void attack_Spew()
    {
        switch (stage)
        {
            default:
                transform.position = startingPos;
                stage = 1;
                break;
            case 1:
                for (int i = 0; i < 5; i++)
                {
                    GameObject spear = Instantiate(gravityProjectile, transform.position, Quaternion.identity);
                    spear.transform.Rotate(new Vector3(0, 0, UnityEngine.Random.Range(70, 110)));
                }

                counter += 1;
                if (counter == test)
                {
                    stage += 2;
                }
                else
                {
                    timer = 1;
                    stage += 1;
                }

                break;
            case 2:
                timer -= Time.deltaTime;

                if (timer <= 0)
                {
                    timer = 0;
                    stage -= 1;
                }
                break;

            case 3:
                _state = States.BETWEEN;
                stage = 0;
                timer = 0;
                counter = 0;
                break;
        }
    }

    private void attack_Beams()
    {
        switch (stage)
        {
            default:
                transform.position = startingPos;
                stage = 1;
                timer = 1;
                break;
            case 1:
                if (timer <= 0)
                {
                    Instantiate(beam, transform.position, Quaternion.identity);
                    counter += 1;
                    timer = 2;
                }
                if (counter == 3)
                {
                    stage += 1;
                    timer = 2;
                }

                timer -= Time.deltaTime;
                break;

            case 2:
                if (timer <= 0)
                {
                    _state = States.BETWEEN;
                    stage = 0;
                    timer = 0;
                    counter = 0;
                }
                timer -= Time.deltaTime;
                break;
        }
    }

    private void release()
    {
        // Releases projectiles in a circling pattern around itself
    }

    private Vector3 glide(Vector3 objectPos, Vector3 targetPos, Vector3 velocity)
    {
        // Creates a smooth "gliding" acceleration towards a given point
        /// Finds the total distance, and the percent of x and y distances towards the target
        Vector3 targetdistance = targetPos - objectPos;
        float xPercent, yPercent;

        float total = Math.Abs(targetdistance.x) + Math.Abs(targetdistance.y);

        if (targetdistance.x != 0)
        {
            xPercent = targetdistance.x / total;
        }
        else
        {
            xPercent = 0;
        }

        if (targetdistance.y != 0)
        {
            yPercent = targetdistance.y / total;
        }
        else
        {
            yPercent = 0;
        }

        float xTarget = (float)7.5 * xPercent;
        float yTarget = (float)7.5 * yPercent;

        /// Adds the percent of x and y distance to the velocity,
        /// while ensuring it doesn't exceed the target speeds
        if (xTarget > velocity.x)
        {
            velocity.x += Math.Abs(xPercent * 15 * Time.deltaTime);
        }
        else
        {
            velocity.x -= Math.Abs(xPercent * 15 * Time.deltaTime);
        }


        if (yTarget > velocity.y)
        {
            velocity.y += Math.Abs(yPercent * 15 * Time.deltaTime);
        }
        else
        {
            velocity.y -= Math.Abs(yPercent * 15 * Time.deltaTime);
        }

        return velocity;
    }

    private void animate()
    {

        if (player.transform.position.x > gameObject.transform.position.x + 0.25)
        {
            animatorController.SetTrigger("IdleR");
        }
        else if (player.transform.position.x < gameObject.transform.position.x - 0.25)
        {
            animatorController.SetTrigger("IdleL");
        }

        if (_state == States.BETWEEN)
        {

        }
    }
    
    public void triggerReset()
    {
        animatorController.ResetTrigger("IdleR");
        animatorController.ResetTrigger("IdleL");
        animatorController.ResetTrigger("AttackR");
        animatorController.ResetTrigger("AttackL");
    }
}
