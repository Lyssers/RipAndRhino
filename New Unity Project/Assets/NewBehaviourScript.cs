using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    const int rotationRate = 130;
    const int movementRate = 150;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal") * Time.deltaTime * rotationRate;
        float verticalInput = Input.GetAxis("Vertical") * Time.deltaTime * movementRate;

        transform.Rotate(0, horizontalInput, 0);
        transform.Translate(0, 0, verticalInput);

    }
}
