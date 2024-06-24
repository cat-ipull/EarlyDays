using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moon_Spawner : MonoBehaviour
{
    public GameObject moonPrefab;
    public float moveDuration = 15f;
    public Vector3 startPosition = new Vector3(-111f, 40f, -6f);
    public Vector3 endPosition = new Vector3(-3f, 40f, 6f);
    public float arcHeight = 13f;
    public float rotationSpeed = 10f;
    public Camera mainCamera;
    public Vector3 cameraOffset = new Vector3(0f, 10f, 0f);

    private GameObject moonInstance;
    private Quaternion originalRotation;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        SpawnMoon();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= moveDuration)
        {
            ResetMoon();
        }
        else
        {
            float t = timer / moveDuration;
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight;
            moonInstance.transform.position = transform.TransformPoint(Vector3.Lerp(startPosition, endPosition, t) + new Vector3(0f, height, 0f));

            // Rotate the moon prefab around the y axis
            moonInstance.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

            // Update camera position
            Vector3 cameraPosition = moonInstance.transform.position + cameraOffset;
            mainCamera.transform.position = cameraPosition;
        }
    }

    void SpawnMoon()
    {
        moonInstance = Instantiate(moonPrefab, transform.TransformPoint(startPosition), Quaternion.identity);
        moonInstance.transform.localScale = new Vector3(5f, 5f, 5f);
        moonInstance.transform.rotation = Quaternion.Euler(-14f, 90f, -28f);
        originalRotation = moonInstance.transform.rotation;
    }

    void ResetMoon()
    {
        timer = 0f;
        moonInstance.transform.position = transform.TransformPoint(startPosition);
        moonInstance.transform.rotation = originalRotation;
    }
}
