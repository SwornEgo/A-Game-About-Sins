using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    //Variables
    public float movement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.Translate(new Vector3(0, -movement * Time.deltaTime, 0));

        if (transform.position.x > 10 || transform.position.y > 10 || transform.position.x < -10 || transform.position.y < -10)
        {
            Destroy(gameObject);
        }
    }
}
