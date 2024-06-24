using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiSpawnMangers : MonoBehaviour
{
    public GameObject[] confettiPrefabs;

    public float spawnLimitXLeft = -55;
    public float spawnLimitXRight = 58;
    public float spawnPosY = 38;
    public float spawnLimitZup = 19;
    public float spawnLimitZdown = -19;

    private float startDelay = 0f;
    public float spawnInterval = 0.0002f;

    private List<GameObject> confettiPool = new List<GameObject>();

    private bool isConfettiSpawn = false;

    void Start()
    {
        for (int i = 0; i < confettiPrefabs.Length; i++)
        {
            GameObject confetti = Instantiate(confettiPrefabs[i]);
            confetti.SetActive(false);
            confettiPool.Add(confetti);
        }
    }

    void SpawnConfetti()
    {
        if (!isConfettiSpawn)
            return;

        int confettiIndex = Random.Range(0, confettiPrefabs.Length);

        GameObject confetti = confettiPool.Find(c => c != null && !c.activeInHierarchy);

        if (confetti == null)
        {
            confetti = Instantiate(confettiPrefabs[confettiIndex]);
            confettiPool.Add(confetti);
        }

        Vector3 spawnPos = new Vector3(Random.Range(spawnLimitXLeft, spawnLimitXRight), spawnPosY, Random.Range(spawnLimitZup, spawnLimitZdown));

        confetti.transform.position = spawnPos;
        confetti.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isConfettiSpawn = !isConfettiSpawn;

            if (isConfettiSpawn)
            {
                Debug.Log("Confetti spawning is enabled");
                InvokeRepeating("SpawnConfetti", startDelay, spawnInterval);
            }
            else
            {
                Debug.Log("Confetti spawning is disabled");
                CancelInvoke("SpawnConfetti");
            }
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began && touch.tapCount == 1)
            {
                isConfettiSpawn = !isConfettiSpawn;

                if (isConfettiSpawn)
                {
                    isConfettiSpawn = true;
                    Debug.Log("Touch spawn is enabled");
                    InvokeRepeating("SpawnConfetti", startDelay, spawnInterval);
                }
                else
                {
                    isConfettiSpawn = false;
                    Debug.Log("Touch spawn is disabled");
                    CancelInvoke("SpawnConfetti");
                }
            }
        }
                
            }
        }
