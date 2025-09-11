using UnityEngine;

public class SpriteFollower : MonoBehaviour
{
    private MonoBehaviour leader;
    private enum Type { basic, lookAtPlayer };
    [SerializeField] private Type _type;

    // Only for lookAtPlayer
    private GameObject player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leader = transform.parent.GetComponentInChildren<MonoBehaviour>();

        // if (_type == Type.lookAtPlayer) {
        //     player = GameObject.Find("The Hero");
        // }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = leader.transform.position;

        // if (_type == Type.lookAtPlayer)
        // {
        //     if (player.transform.position.x > gameObject.transform.position.x + 0.5)
        //     {
        //         gameObject.transform.Rotate(0, -180, 0);
        //         print("right");
        //     }
        //     else if (player.transform.position.x < gameObject.transform.position.x - 0.5 &
        //              gameObject.transform.rotation.y == 0)
        //     {
        //         gameObject.transform.Rotate(0, 180, 0);
        //         print("left");
        //     }
        // }
    }
}
