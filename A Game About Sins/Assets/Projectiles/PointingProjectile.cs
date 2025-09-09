using UnityEngine;
using UnityEngine.XR;

public class PointingProjectile : ProjectileScript
{
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.Find("The Hero");
        transform.right = player.transform.position - transform.position;
        transform.Rotate(new Vector3(0, 0, 90));
    }

    // Update is called once per frame
}
