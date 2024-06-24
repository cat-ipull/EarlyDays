using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera camera1;
    public Camera camera2;
    private bool isCamera1Active = false; // Set isCamera1Active to false initially
    private Vector2 startPos; // Declare startPos variable
    private float minSwipeDistance = 0.5f; // Minimum swipe distance in inches

    // Start is called before the first frame update
    void Start()
    {
        // Enable camera1 and disable camera2 at the start of the game
        camera1.enabled = true;
        camera2.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchCamera();
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                Vector2 swipeDelta = touch.position - startPos;
                float swipeDistance = swipeDelta.magnitude / Screen.dpi; // Convert swipe distance to inches
                if (swipeDistance >= minSwipeDistance)
                {
                    if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
                    {
                        if (swipeDelta.x > 0)
                        {
                            SwitchCamera();
                        }
                        else if (swipeDelta.x < 0)
                        {
                            SwitchCamera();
                        }
                    }
                }
            }
        }
    }

    void SwitchCamera()
    {
        if (isCamera1Active)
        {
            camera1.enabled = false;
            camera2.enabled = true;
            isCamera1Active = false;
        }
        else
        {
            camera1.enabled = true;
            camera2.enabled = false;
            isCamera1Active = true;
        }
    }
}
