using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class rotate : MonoBehaviour
{
    [SerializeField]
    private float roatate_speed;
    [SerializeField]
    private bool enable_rotate;

    private void Update()
    {
        roatate_object();
    }

    private void roatate_object()
    {
        // If Not rotate
        if (!enable_rotate)
        {
            // Return
            return;
        }
        else
        {
            transform.Rotate(Vector3.forward * roatate_speed * Time.deltaTime);

        }
    }
}
