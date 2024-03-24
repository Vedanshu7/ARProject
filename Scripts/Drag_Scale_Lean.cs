using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_Scale_Lean : MonoBehaviour
{
    private Touch touch;

    [SerializeField]
    private float speedModifier;

    private void Start()
    {
        speedModifier = 0.001f;
    }

    private void Update()
    {
        if(Input.touchCount >0) 
        {
            touch = Input.GetTouch(0);
            if(touch.phase == TouchPhase.Moved)
            {
                transform.position = new Vector3(transform.position.x + touch.deltaPosition.x * speedModifier, transform.position.y, transform.position.z + touch.deltaPosition.y * speedModifier);
            }
        }
    }

}
