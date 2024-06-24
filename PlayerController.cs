using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float boundaryX = 300f;
    public float boundaryY = 100f;
    public float boundaryZ = 300f;

    // Start is called before the first frame update
    void Start()
    {
        // Set the starting position for the camera
        transform.position = new Vector3(-45f, 37f, -180f);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float forwardInput = Input.GetAxis("Forward");
        float strafeInput = Input.GetAxis("Horizontal");

        // Mouse free look
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the player based on mouse movement
        transform.Rotate(Vector3.up, mouseX);

        // Rotate the camera based on mouse movement
        Camera.main.transform.Rotate(Vector3.right, -mouseY);

        Vector3 movement = new Vector3(strafeInput, 0f, forwardInput) * moveSpeed * Time.deltaTime;
        Vector3 newPosition = transform.position + movement;

        newPosition.x = Mathf.Clamp(newPosition.x, -boundaryX, boundaryX);
        newPosition.y = Mathf.Clamp(newPosition.y, -boundaryY, boundaryY);
        newPosition.z = Mathf.Clamp(newPosition.z, -boundaryZ, boundaryZ);

        transform.position = newPosition;
    }
}
