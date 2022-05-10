using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InputHandlerAI : IInputHandling
{
    [SerializeField] private GameObject[] targetsArray = new GameObject[2];
    [SerializeField] private Vector3 target;
    [SerializeField] private Slider DiffSlider;
    [SerializeField] private int difficulty;
    public float h = 25;
    public float gravity = -9.8f;
    private GameObject player;

    private void Start()
    {
        difficulty = 1;
        targetsArray = new GameObject[2];
        player = GameObject.FindGameObjectWithTag("Player");
        targetsArray[0] = Helpers.FindComponentInChildWithTag<Transform>(player, "bodyShot_hitpoint").gameObject;
        targetsArray[1] = Helpers.FindComponentInChildWithTag<Transform>(player, "headshot_hitpoint").gameObject;
    }

    public override Vector3 GetDirectionToTarget(Rigidbody2D rb)
    {
        SetTargetBasedOnDifficulty();
        Physics2D.gravity = Vector3.up * gravity;
        rb.gravityScale = 1.0f;
        return CalculateLaunchData(target, rb).initialVelocity;
    }

    public void SetTargetBasedOnDifficulty()
    {
        var prob = CalculateHitSuccess();
        if (prob <= 4)
        {
            target = player.transform.position + (Vector3.right * Random.Range(-15.0f, 8.0f));
        }
        else if (prob > 4 && prob <= 8f)
        {
            target = targetsArray[0].transform.position;
        }
        else
        {
            target = targetsArray[1].transform.position;
        }
    }

    public float CalculateHitSuccess()
    {
        float prob;
        if (difficulty < 3)
        { 
            prob = difficulty * Random.Range(1, 6);
        }
        else
        { 
            prob = difficulty * Random.Range(2, 7);
        }
        return prob;
    }

    public void UpdateDifficulty()
    {
        difficulty = (int)DiffSlider.value;
    }

    LaunchData CalculateLaunchData(Vector3 target, Rigidbody2D projectile) 
    {
        float displacementY = target.y - projectile.position.y;
        Vector3 displacementXZ = new Vector3 (target.x - projectile.position.x, 0);
        float time = Mathf.Sqrt(-2*h/gravity) + Mathf.Sqrt(2*(displacementY - h)/gravity);
        Vector3 velocityY = Vector3.up * Mathf.Sqrt (-2 * gravity * h);
        Vector3 velocityXZ = displacementXZ / time;

        return new LaunchData(velocityXZ + velocityY * -Mathf.Sign(gravity), time);
    }
}

struct LaunchData 
{
    public readonly Vector3 initialVelocity;
    public readonly float timeToTarget;

    public LaunchData (Vector3 initialVelocity, float timeToTarget)
    {
        this.initialVelocity = initialVelocity;
        this.timeToTarget = timeToTarget;
    }
		
}
