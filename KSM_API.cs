using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class KSM_API : MonoBehaviour
{
    private string URL = "https://api.coingecko.com/api/v3/simple/price?ids=kusama&vs_currencies=usd&x_cg_demo_api_key=CG-DZppjbTLmu4WjejyLgjS6hrH";
    public float ksmPrice; // Make ksmPrice public
    public bool instantiatePrefab; // Add a public bool to control prefab instantiation

    public GameObject KSM_live_prefab; // Change the name of the GameObject

    public TextMeshProUGUI priceText; // Change the type to TextMeshProUGUI

    private GameObject floorObject;
    public Camera mainCamera; // Add a reference to the main camera

    void Awake()
    {
        floorObject = GameObject.FindGameObjectWithTag("floor");
        mainCamera = Camera.main; // Assign the main camera reference
        if (floorObject != null)
        {
            StartCoroutine(GetDatas());
        }
        else
        {
            Debug.LogError("Floor object not found!");
        }
    }

    IEnumerator GetDatas()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(URL))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            else
            {
                string json = request.downloadHandler.text;

                // Print the JSON string for debugging
                Debug.Log(json);

                // Deserialize the JSON string into a RootObject
                RootObject data = JsonUtility.FromJson<RootObject>(json);

                // Get the price value from the data
                ksmPrice = data.kusama.usd; // Assign the value to ksmPrice

                // Use the price to create Vector3 changes for game objects
                Vector3 change = new Vector3(34f, data.kusama.usd + 13.5f, 0f);

                if (instantiatePrefab) // Check the bool value before instantiating the prefab
                {
                    // Instantiate a new game object from the prefab
                    GameObject newObj = Instantiate(KSM_live_prefab, floorObject.transform.position + change, KSM_live_prefab.transform.rotation);

                    // Set the name of the new game object
                    newObj.name = "KSM_Price";
                }

                // Move the main camera up and down based on the price change
                mainCamera.transform.position += new Vector3(0f, data.kusama.usd, 0f);

                // Override any existing TextMeshPro text
                TextMeshProUGUI[] textMeshPros = FindObjectsOfType<TextMeshProUGUI>();
                foreach (TextMeshProUGUI textMeshPro in textMeshPros)
                {
                    priceText.text = "KSM" + " $" + ksmPrice.ToString() + " 31 Day Price Chart*Data Courtesy Coingecko*Just for Fun*Not to Scale*(DYOR)";
                }

                Debug.Log("Price Value: " + ksmPrice);
            }
        }
    }
}

[System.Serializable]
public class RootObject
{
    public KusamaData kusama;
}

[System.Serializable]
public class KusamaData
{
    public float usd;
}
