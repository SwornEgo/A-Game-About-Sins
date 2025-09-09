using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Unity.UI;
using UnityEngine.UIElements;
using NUnit.Framework.Constraints;

public class PlayerScript : MonoBehaviour
{
    public enum State { overworld, combat, cutscene, death } 
    [SerializeField] private State _state;

    public Animator animator;

    // Direction Constants
    private const string UP = "Up";
    private const string DOWN = "Down";
    private const string LEFT = "Left";
    private const string RIGHT = "Right";


    // Public Variables
    public int speed;
    public int maxHealth;
    private float currHealth;
    public float spacing;
    public float dashTime;
    public string direction;
    public WeaponBehavior primaryWeapon;
    public WeaponBehavior secondaryWeapon;

    // Private Variables
    private float distance = 0;
    private float dashTimer = 0;
    private Vector3 movement;
    private Vector3 projectedPoint;
    private bool isDashing = false;

    // GameObjects
    private GameObject player;
    public GameObject tile;
    public Camera gameCam;
    [SerializeField] BarController healthBar;
    [SerializeField] BarController dashBar;

    // Awake is called before Start
    void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        player = gameObject;
        currHealth = maxHealth;
        healthBar.UpdateBar(currHealth, maxHealth);
        dashBar.UpdateBar(dashTimer, dashTime);
        direction = DOWN;
        dashTimer = dashTime;
    }

    // Update is called once per frame
    void Update()
    {
        stateCombat();
    }

    private void stateCombat()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            HealthUpdate(-10);
        }

        triggerReset();

        // Up
        if (Input.GetKeyDown(KeyCode.W) & distance == 0)
        {
            projectedPoint = new Vector3(player.transform.position.x,
                                         player.transform.position.y + spacing,
                                         player.transform.position.z
                                         );
            direction = UP;
            animator.SetTrigger(UP);

            if (checkTile(projectedPoint))
            {
                movement = new Vector3(0, speed, 0);
                distance = spacing;
            }

        }

        // Left
        else if (Input.GetKeyDown(KeyCode.A) & distance == 0)
        {
            projectedPoint = new Vector3(player.transform.position.x - spacing,
                                         player.transform.position.y,
                                         player.transform.position.z
                                         );
            direction = LEFT;
            animator.SetTrigger(LEFT);

            if (checkTile(projectedPoint))
            {
                movement = new Vector3(-speed, 0, 0);
                distance = spacing;
            }
        }

        // Down
        else if (Input.GetKeyDown(KeyCode.S) & distance == 0)
        {
            projectedPoint = new Vector3(player.transform.position.x,
                                         player.transform.position.y - spacing,
                                         player.transform.position.z
                                         );
            direction = DOWN;
            animator.SetTrigger(DOWN);

            if (checkTile(projectedPoint))
            {
                movement = new Vector3(0, -speed, 0);
                distance = spacing;
            }
        }

        // Right
        else if (Input.GetKeyDown(KeyCode.D) & distance == 0)
        {
            projectedPoint = new Vector3(player.transform.position.x + spacing,
                                         player.transform.position.y,
                                         player.transform.position.z
                                         );
            direction = RIGHT;
            animator.SetTrigger(RIGHT);

            if (checkTile(projectedPoint))
            {
                movement = new Vector3(speed, 0, 0);
                distance = spacing;
            }
        }

        // Dash
        if (Input.GetKey(KeyCode.LeftShift) & dashTimer >= dashTime & distance == spacing)
        {
            dash(direction);
            dashTimer = 0;
            isDashing = true;
            this.GetComponent<Collider2D>().enabled = false;
        }

        // Checks to see if the player has arrived at the correct tile
        if (distance > 0)
        {
            distance -= (Time.deltaTime * speed);
        }
        if (distance < 0)
        {
            movement = new Vector3(0, 0, 0);
            distance = 0;
            player.transform.position = projectedPoint;
            this.GetComponent<Collider2D>().enabled = true;
            isDashing = false;
        }

        // Movement
        player.transform.Translate(movement * Time.deltaTime);

        if (dashTimer < dashTime)
        {
            dashTimer += Time.deltaTime;
        }
        if (dashTimer > dashTime)
        {
            dashTimer = dashTime;
        }
        dashBar.UpdateBar(dashTimer, dashTime);

        // Combat
        if (Input.GetKeyDown(KeyCode.Space) & distance == 0)
        {
            WeaponBehavior weapon = Instantiate(primaryWeapon, transform.position, Quaternion.identity);
            weapon.GetComponent<WeaponBehavior>().direction = direction;
            weapon.Init(direction);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            WeaponBehavior tempWeapon = primaryWeapon;
            primaryWeapon = secondaryWeapon;
            secondaryWeapon = tempWeapon;
        }
    }

    // Getting Hit
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent("ProjectileScript") != null
        & isDashing == false)
        {
            HealthUpdate(-25);
        }
    }

    // Dashing
    private void dash(string direction)
    {
        Vector3 newPoint = projectedPoint;
        string DashAnimation = "";

        if (direction == UP)
        {
            newPoint = new Vector3(projectedPoint.x,
                                   projectedPoint.y + spacing,
                                   projectedPoint.z
                                         );
            DashAnimation = "DashU";
        }
        else if (direction == DOWN)
        {
            newPoint = new Vector3(projectedPoint.x,
                                   projectedPoint.y - spacing,
                                   projectedPoint.z
                                         );
            DashAnimation = "DashD";
        }
        else if (direction == LEFT)
        {
            newPoint = new Vector3(projectedPoint.x - spacing,
                                   projectedPoint.y,
                                   projectedPoint.z
                                         );
            DashAnimation = "DashL";
        }
        else if (direction == RIGHT)
        {
            newPoint = new Vector3(projectedPoint.x + spacing,
                                   projectedPoint.y,
                                   projectedPoint.z
                                         );
            DashAnimation = "DashR";
        }

        if (checkTile(newPoint))
        {
            projectedPoint = newPoint;
            movement = movement * 2;
            dashTimer = dashTime;
            animator.SetTrigger(DashAnimation);
        }
    }

    public void HealthUpdate(float amount)
    {
        currHealth += amount;
        print(currHealth);
        healthBar.UpdateBar(currHealth, maxHealth);
    }

    public bool checkTile(Vector2 position)
    {
        Collider2D[] colliderList = Physics2D.OverlapPointAll(position);
        foreach (Collider2D collider in colliderList)
        {
            if (collider != null)
            {
                try
                {
                    if (collider.GetComponentInParent<CombatScript>().GetType().ToString() == "CombatScript")
                    {
                        return true;
                    }
                }
                catch (Exception)
                {
                    print("Wrong Collider!");
                }
            }
        }


        return false;
    }

    public void triggerReset()
    {
        animator.ResetTrigger(UP);
        animator.ResetTrigger(DOWN);
        animator.ResetTrigger(LEFT);
        animator.ResetTrigger(RIGHT);
    }
}
