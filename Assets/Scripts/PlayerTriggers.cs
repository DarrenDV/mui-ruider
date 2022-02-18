using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggers : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private MuiSpawnerScript muiSpawner;
    [SerializeField] private GameObject pion;

    Vector3 playerStartPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("PlayerHolder");
        muiSpawner = GameObject.Find("MuiSpawner").GetComponent<MuiSpawnerScript>();

        playerStartPos = player.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "EndZone")
        {
            player.transform.position = playerStartPos;
            muiSpawner.DeleteMuien();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (other.tag == "mui")
            {
                other.GetComponent<MUI>().hasPlacedFlag = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "mui")
        {
            other.GetComponent<MUI>().LeftMui();
        }
    }
}
