using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    float moveSpeed = 5f;

    public GameObject oempaloempa;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = transform.position += transform.forward * moveSpeed * Time.deltaTime;

        if (Input.GetKeyDown("space"))
        {
            //Instantiate oempaloempa
            Instantiate(oempaloempa, transform.position, transform.rotation, transform.parent = null);
        }
    }
}
