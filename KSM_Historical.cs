using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class KSM_Historical : MonoBehaviour
{
    private string URL = "https://api.coingecko.com/api/v3/coins/kusama/market_chart?vs_currency=usd&days=31&interval=daily&x_cg_demo_api_key=CG-DZppjbTLmu4WjejyLgjS6hrH";
    public GameObject NS_prefab;
    private GameObject floorObject;

    public float[] ksm_x; // Public x position for KSM historical data, represents time
    public float[] ksm_y; // Public y position for KSM historical data, represents price
    public bool isMeshEnabled = false;

    void Start()
    {
        // Find the floor object in the scene, floor object is used as a reference for positioning the prefabs
        floorObject = GameObject.FindGameObjectWithTag("floor");
        if (floorObject != null)
        {
            // Start the coroutine to fetch KSM historical data
            StartCoroutine(GetKSMHistoricalData());
        }
        else
        {
            // Log an error if the floor object is not found
            Debug.LogError("Floor object not found!");
        }


        IEnumerator GetKSMHistoricalData()
        {
            using (UnityWebRequest request = UnityWebRequest.Get(URL))
            {
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.ConnectionError)
                {
                    // Log an error if there is a connection error
                    Debug.LogError(request.error);
                }
                else
                {
                    // Get the JSON response
                    string json = request.downloadHandler.text;
                    Debug.Log(json);

                    // Parse the JSON data
                    SimpleJSON.JSONNode data = SimpleJSON.JSON.Parse(json);
                    SimpleJSON.JSONArray pricesArray = data["prices"].AsArray;
                    Debug.Log(pricesArray);

                    // Define a scaling factor, this is used to scale the x value from Unix Epoch time to a smaller value
                    float scalingFactor = .00000000001f;

                    // Define the spacing between prefabs, this helps space the prefabs after scaling factor is applied
                    float spacing = 2f;

                    // Initialize the arrays
                    ksm_x = new float[pricesArray.Count];
                    ksm_y = new float[pricesArray.Count];

                    // Iterate through the prices array and populate the arrays
                    for (int i = 0; i < pricesArray.Count; i++)
                    {
                        // Get the x and y values from the prices array
                        ksm_x[i] = pricesArray[i][0].AsFloat;
                        ksm_y[i] = pricesArray[i][1].AsFloat;
                        Vector3 change = new Vector3(ksm_x[i], ksm_y[i], 0f);

                        // Calculate the position of the prefab
                        Vector3 position = floorObject.transform.position + new Vector3((ksm_x[i] * scalingFactor) - 44.5f + (i * spacing), ksm_y[i] - 12f, 0f);

                        // Instantiate a new prefab at the calculated position with mesh off
                        GameObject newObj = Instantiate(NS_prefab, position, Quaternion.identity);
                        newObj.name = "KSM_HistoricalPrice" + i;
                        newObj.GetComponent<MeshRenderer>().enabled = isMeshEnabled; // Turn off the mesh
                    }
                }
            }
        }
    }
}
