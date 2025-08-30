using System;
using UnityEngine;

public class S_CarHover : MonoBehaviour
{
    [SerializeField] private Transform[] hoverPoints;
    [SerializeField] private float length, strength, dampening, maxRotation;

    
    private Rigidbody rb;
    private float lastHitDist;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (!rb)
        {
            Debug.LogError("No rigidbody attached");
        }
        
        if (hoverPoints.Length == 0)
        {
            SpringPhysic(transform);
        }
        else
        {
            foreach (var point in hoverPoints)
            {
                SpringPhysic(point);
            }
        }
        
        ClampRotation();
    }

    private void ClampRotation()
    {
        Vector3 eulerAngle = transform.eulerAngles;

        // Convert from 0–360 to -180–180 for proper clamping
        float x = (eulerAngle.x > 180) ? eulerAngle.x - 360 : eulerAngle.x;
        float z = (eulerAngle.z > 180) ? eulerAngle.z - 360 : eulerAngle.z;

        // Clamp
        x = Mathf.Clamp(x, -maxRotation, maxRotation);
        z = Mathf.Clamp(z, -maxRotation, maxRotation);

        // Assign back, but convert to 0–360 again
        eulerAngle.x = x;
        eulerAngle.z = z;

        transform.eulerAngles = eulerAngle;
    }


    private void SpringPhysic(Transform t)
    {
        RaycastHit hit;

        if (Physics.Raycast(t.position, t.TransformDirection(-Vector3.up), out hit, length))
        {
            float forceAmount = HooksLawDampen(hit.distance);
            
            rb.AddForceAtPosition(t.up * forceAmount, t.position);
        }
        else
        {
            lastHitDist = length * 1.1f;
        }
    }

    private float HooksLawDampen(float distance)
    {
        float forceAmount = strength * (length - distance) + (dampening * (lastHitDist - distance));
        forceAmount = Mathf.Max(0f, forceAmount);
        lastHitDist = distance;
        
        return forceAmount;
    }
}
