using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyConfetti : MonoBehaviour
{

    public float bottomLimit = 15f;
    public float timerDelay = 0.01f;

    // Update is called once per frame
    void Update()
    {

        //Destroy confetti delay five seconds regardless of how fast it falls


        Destroy(gameObject, timerDelay);

        // Destroy confetti if y position less than bottomm limit
        if (transform.position.y < bottomLimit)
        {
            Destroy(gameObject);
        }

        

    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
        
    }

}