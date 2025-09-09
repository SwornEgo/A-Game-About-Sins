using UnityEngine;

public class Shockmaker : MonoBehaviour
{
    const string L = "left";
    const string R = "right";

    public int num_of_shocks;
    public Right_Shockwave rightShock;
    public Left_Shockwave leftShock;

    private Shockwave[] rightShockList;
    private Shockwave[] leftShockList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rightShockList = shockConstructor(R);
        leftShockList = shockConstructor(L);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private Shockwave[] shockConstructor(string L_or_R)
    {
        // Creates the shockwaves on either side of the screen for use later
        int direction = 1;
        Shockwave shock;

        if (L_or_R == R)
        {
            shock = rightShock;
            direction = -1;
        }
        else
        {
            shock = leftShock;
            direction = 1;
        }

        Shockwave[] list = new Shockwave[num_of_shocks * 2 + 1];
        list[0] = Instantiate(shock, new Vector3(-13 * direction, transform.position.y, 0), Quaternion.identity);

        int index = 1;
        int position = 1;

        while (index < num_of_shocks)
        {
            list[index] = Instantiate(shock, new Vector3((-13 - position) * direction, transform.position.y + position * 0.6f, 0), Quaternion.identity);
            list[index + 1] = Instantiate(shock, new Vector3((-13 - position) * direction, transform.position.y - position * 0.6f, 0), Quaternion.identity);
            index += 2;
            position++;
        }

        return list;
    }

    public bool shockActivator(string direction)
    {
        // Activates the shockwaves 
        if (direction == L)
        {
            for (int i = 0; i < num_of_shocks; i++)
            {
                leftShockList[i].Shock();
            }
        }

        else if (direction == R)
        {
            for (int i = 0; i < num_of_shocks; i++)
            {
                rightShockList[i].Shock();
            }
        }

        return true;
    }
}