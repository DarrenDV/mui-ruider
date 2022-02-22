using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 0;
    public GameObject startButton;
    public GameObject oempaloempa;
    [SerializeField] private GameObject player;

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
            Instantiate(oempaloempa, player.transform.position, player.transform.rotation, transform.parent = null);
        }
    }
    public void StartGame()
    {
        moveSpeed = 5;
        startButton.SetActive(false);
    }
}
