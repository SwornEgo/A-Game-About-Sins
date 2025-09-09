using UnityEngine;

public class SpriteFollower : MonoBehaviour
{
    private MonoBehaviour leader;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        leader = transform.parent.GetComponentInChildren<MonoBehaviour>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = leader.transform.position;
    }
}
