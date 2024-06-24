using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionalLightRotation : MonoBehaviour
{
    public Light directionalLight;
    public float rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        RotateDirectionalLight();
    }

    void RotateDirectionalLight()
    {
        directionalLight.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
