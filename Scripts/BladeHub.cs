using JetBrains.Annotations;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnchancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.Touch;

public class BladeHub : MonoBehaviour
{
    private int touch_count = 0;
    private Vector2 touchPosition;
    private ARRaycastManager _arRaycastManager;
    private float scaleFactor = 1.1f; // Scale factor to increase the size by
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    [SerializeField]
    private GameObject[] gameObjectArray = new GameObject[4];
    private Dictionary<string, Vector3> originalScales;
    private bool isDragging = false;
    private Vector3 offset;
    private GameObject draggedObject;
    private Vector2 initialTouchDelta;
    private Vector2 currentTouchDelta;

    void Awake()
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();

        originalScales = new Dictionary<string, Vector3>();


        foreach (GameObject obj in gameObjectArray)
        {

            string tag = obj.tag;


            if (!originalScales.ContainsKey(tag))
            {

                originalScales[tag] = obj.transform.localScale;
            }
            else
            {
                Debug.LogWarning("Duplicate tag found: " + tag);
            }
        }
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            if (Input.touchCount == 1)
            {
                Touch touch = Input.GetTouch(0);

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        RaycastHit(touch.position);
                        break;
                    case TouchPhase.Moved:
                        if (isDragging)
                            OnTouchMoved(touch.position);
                        break;
                    case TouchPhase.Ended:
                        OnTouchEnded();
                        break;
                }
            }

            else if (Input.touchCount == 2)
            {
                RotateObjectWithTwoFingers();
            }

        }
    }
    private void RotateObjectWithTwoFingers()
    {
        Touch touch1 = Input.GetTouch(0);
        Touch touch2 = Input.GetTouch(1);

        if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
        {
            Vector2 initialPosition1 = touch1.position - touch1.deltaPosition;
            Vector2 initialPosition2 = touch2.position - touch2.deltaPosition;
            Vector2 currentPosition1 = touch1.position;
            Vector2 currentPosition2 = touch2.position;

            // Calculate the initial and current vectors
            Vector2 initialVector = initialPosition2 - initialPosition1;
            Vector2 currentVector = currentPosition2 - currentPosition1;

            // Calculate the angle between the initial and current vectors
            float initialAngle = Mathf.Atan2(initialVector.y, initialVector.x);
            float currentAngle = Mathf.Atan2(currentVector.y, currentVector.x);

            // Calculate the difference in angle
            float angleDifference = currentAngle - initialAngle;

            // Rotate the object
            draggedObject.transform.Rotate(Vector3.up, angleDifference * Mathf.Rad2Deg);
        }
    }

    void RaycastHit(Vector2 touchPosition)
    {
        Touch touch = Input.GetTouch(0);
        if (touch.phase == TouchPhase.Began)
        {
            touchPosition = touch.position;
        }
        Ray ray = Camera.main.ScreenPointToRay(touch.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            touch_count++;
            if (hit.collider != null)
            {

                GameObject hitObject = hit.collider.gameObject;
                if (touch_count == 1)
                {

                    if (hitObject.tag == "Motor" || hitObject.tag == "Screw" || hitObject.tag == "Bench" || hitObject.tag == "Pipe")
                    {

                        hitObject.transform.localScale *= scaleFactor;
                        draggedObject = hitObject;
                        isDragging = true;
                        offset = hitObject.transform.position - GetWorldPosition(touchPosition);
                    }
                    else
                    {
                        Debug.Log("Hit a different object.");
                    }
                }
                else
                {
                    if (hitObject.tag == "Motor" || hitObject.tag == "Screw" || hitObject.tag == "Bench" || hitObject.tag == "Pipe")
                    {
                        hitObject.transform.localScale = originalScales[hitObject.tag];
                        touch_count = 0;
                    }
                    else
                    {
                        Debug.Log("Hit a different object.");
                    }
                }
            }
            else
            {
                Debug.Log("Ray hit a feature point but not a GameObject.");
            }
        }
        else
        {
            Debug.Log("No raycast hit.");
        }
    }

    private void OnTouchMoved(Vector2 touchPosition)
    {
        Vector3 newPosition = GetWorldPosition(touchPosition) + offset;
        draggedObject.transform.position = newPosition;
    }

    private void OnTouchEnded()
    {
        isDragging = false;
        draggedObject = null;
    }

    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        Vector3 worldPosition = screenPosition;
        worldPosition.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(worldPosition);
    }
}