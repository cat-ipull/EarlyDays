using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class KSM_Hist_Mountains : MonoBehaviour
{
    private string URL = "https://api.coingecko.com/api/v3/coins/kusama/market_chart?vs_currency=usd&days=31&interval=daily&x_cg_demo_api_key=CG-DZppjbTLmu4WjejyLgjS6hrH";
    public LineRenderer lineRenderer;
    private GameObject floorObject;
    public Material mountains;

    public float[] ksm_x; // Public x position for KSM historical data, represents time
    public float[] ksm_y; // Public y position for KSM historical data, represents price
    public float scalingFactor = .00000000005f; // Public scaling factor

    void Start()
    {
        // Find the floor object in the scene, floor object is used as a reference for positioning the line renderer
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

                // Define the spacing between points, this helps space the points after scaling factor is applied
                float spacing = 2f;

                // Initialize the arrays
                ksm_x = new float[pricesArray.Count + 2];
                ksm_y = new float[pricesArray.Count + 2];

                // Add the first and last points
                ksm_x[0] = floorObject.transform.position.x;
                ksm_y[0] = floorObject.transform.position.y;


                // Iterate through the prices array and populate the arrays
                for (int i = 0; i < pricesArray.Count; i++)
                {
                    // Get the x and y values from the prices array
                    ksm_x[i + 1] = pricesArray[i][0].AsFloat;
                    ksm_y[i + 1] = pricesArray[i][1].AsFloat;

                    // Calculate the position of the line point
                    Vector3 position = floorObject.transform.position + new Vector3((ksm_x[i + 1] * scalingFactor) + 38f + ((i + 1) * spacing), ksm_y[i + 1] - 3, 0f);

                    // Set the position of the line point
                    lineRenderer.positionCount = i + 2;
                    lineRenderer.SetPosition(i + 1, position);
                }
              

                // Add the new point at the end
                ksm_x[ksm_x.Length - 2] = 100f;
                ksm_y[ksm_y.Length - 2] = 0f;

                // Calculate the position of the new point
                Vector3 newPosition = floorObject.transform.position + new Vector3(400f, -100f, 0f);

                //the following code is what's generating inside the scene, for good or ill

                // Set the position of the new point
                lineRenderer.positionCount = ksm_x.Length;
                lineRenderer.SetPosition(ksm_x.Length - 2, newPosition);

                // Create a mesh to fill the loop of lineRenderer
                Mesh mesh = new Mesh();
                lineRenderer.BakeMesh(mesh);
                
                // Assign the mountains material to the mesh
                MeshRenderer meshRenderer = lineRenderer.gameObject.AddComponent<MeshRenderer>();
                meshRenderer.sharedMaterial = mountains;

                // Assign the mesh to the mesh filter
                MeshFilter meshFilter = lineRenderer.gameObject.AddComponent<MeshFilter>();
                meshFilter.mesh = mesh;
            }
        }
    }
}
