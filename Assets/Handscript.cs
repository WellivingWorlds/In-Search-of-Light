using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Handscript : MonoBehaviour
{
    public GameObject moth;  // Reference to the Moth object

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (moth != null)
        {
            // Get the current position of the Moth
            Vector3 mothPosition = moth.transform.position;

            // Update the position of the current object to match the Moth's z and x positions
            transform.position = new Vector3(mothPosition.x, transform.position.y, mothPosition.z);
        }
        else
        {
            Debug.LogWarning("Moth object is not assigned.");
        }
    }
}
