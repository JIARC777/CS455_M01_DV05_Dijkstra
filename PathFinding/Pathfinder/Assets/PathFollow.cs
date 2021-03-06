﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrive
{
    public AbstractKinematic ai;
    public GameObject target;
    public float maxAcceleration = 25f;
    float targetRadius = 3f;
    float slowRadius = 10f;
    float timeToTarget = 0.1f;
    public float maxSpeed = 50f;

    public virtual SteeringOutput GetSteering()
    {
        SteeringOutput result = new SteeringOutput();
        result.linear = target.transform.position - ai.transform.position;
        float distance = result.linear.magnitude;
        float targetSpeed;
        Vector3 targetVelocity;
        // if (distance < targetRadius)
        //     return null;
        if (distance > slowRadius)
        {
            targetSpeed = maxSpeed;
        }
        else
        {
            targetSpeed = maxSpeed * (distance - targetRadius) / targetRadius;
        }
        targetVelocity = result.linear;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;


        result.linear = targetVelocity - ai.linearVelocity;
        result.linear /= timeToTarget;
        result.linear.y = 0;
        if (result.linear.magnitude > maxAcceleration)
        {
            result.linear.Normalize();
            result.linear *= maxAcceleration;
        }
        //Debug.Log(target);
        result.angular = 0;
        return result;

    }
}

public class PathFollow : Arrive
{
    public GameObject[] path;
    float targetRadius = 4f;
    int currentIndex;

    public override SteeringOutput GetSteering()
    {
        if (target == null)
        {
            currentIndex = 0;
            target = path[currentIndex];
        }

        float distToTarget = (target.transform.position - ai.transform.position).magnitude;
        if (distToTarget < targetRadius)
        {
            currentIndex++;
            if (currentIndex > path.Length - 1)
                currentIndex = 0;
        }
        target = path[currentIndex];
        return base.GetSteering();
    }
}