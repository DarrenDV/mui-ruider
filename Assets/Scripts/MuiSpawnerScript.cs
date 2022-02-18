using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuiSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] muien = new GameObject[3];
    public float worldLength;
    public int maxWorldZ;
    
    void Start()
    {
        for (int i = 0; i < worldLength; i++)
        {
            int randomMui = Random.Range(0,3);
            float randomXOffset = Random.Range(-2.0f, 3.0f);
            float randomZOffset = Random.Range(-maxWorldZ, maxWorldZ + 1);
            Instantiate(muien[randomMui], new Vector3((i * 10) + randomXOffset, 0.5f, randomZOffset), Quaternion.identity);
        }
    }
    private void FixedUpdate()
    {
        
    }
}
