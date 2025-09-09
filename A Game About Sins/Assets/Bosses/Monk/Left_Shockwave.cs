using UnityEngine;

public class Left_Shockwave : Shockwave
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {   
        switch (stage)
        {
            // Accelerates towards the playe area
            case 1:
                transform.Translate(speed * Time.deltaTime, 0, 0);

                if (transform.position.x >= -distance)
                {
                    stage = 2;
                }
                break;

            // Deceleration
            case 2:
                // Check if at max Deceleration
                if (-speed > maxDeceleration)
                {
                    speed = -maxDeceleration;
                }
                else
                {
                    speed -= deceleration * Time.deltaTime;
                }

                // Check if reached Original Position
                if (transform.position.x + speed * Time.deltaTime < startingPos.x)
                {
                    transform.position = startingPos;
                    speed = maxSpeed;
                    stage = 0;
                }
                else
                {
                    transform.Translate(speed * Time.deltaTime, 0, 0);
                }
                break;

        }

    }
}
