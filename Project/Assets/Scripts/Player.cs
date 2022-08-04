using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    float rotateSpeedMultiplier = 15;
    float speedDivider = 10;
    float boostAmount = 80;
    float brakePower = 3;
    bool accelerating;
    bool steering;
    bool boosting;
    PlayerInput pi;
    Rigidbody2D rb;
    float dir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        pi = GetComponent<PlayerInput>();
        if (!pi.actions)
        {
            Debug.LogError("Player Input lost its Actions!!!");
        }
    }

    private void FixedUpdate()
    {
        if (accelerating)
        {
            var vel = (new Vector2(transform.up.x, transform.up.y) / speedDivider);

            if (boosting)
            {
                print("boosting");
                vel *= boostAmount;
                boosting = false;
            }

            rb.velocity += vel;
        }
        else if (boosting)
        {
            var vel = (new Vector2(transform.up.x, transform.up.y) / speedDivider);

            vel *= boostAmount;

            rb.velocity += vel;

            boosting = false;
        }

        if (steering)
        {
            Rotate();
        }
    }

    public void OnBoost()
    {
        boosting = true;
    }

    public void OnBrake(InputValue value)
    {
        if (value.Get<float>() == 0)
        {
            rb.drag = .5f;
        }
        else
        {
            rb.drag = brakePower;
        }
    }

    public void OnAccelerate(InputValue value)
    {
        if (value.Get<float>() == 0)
        {
            accelerating = false;
        }
        else
        {
            accelerating = true;
        }
    }

    public void OnSteer(InputValue value)
    {
        dir = value.Get<float>();

        if (dir == 0)
        {
            steering = false;
        }
        else
        {
            steering = true;
        }
    }

    void Rotate()
    {
        rb.angularVelocity += -dir * rotateSpeedMultiplier;
    }
}
