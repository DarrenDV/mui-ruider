using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuiSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] muien = new GameObject[3];
    [SerializeField] private GameObject muiHolder;
    public float worldLength;
    public int maxWorldZ;
    private void Start()
    {
        CreateMuien();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            DeleteMuien();
        }
    }
    private void CreateMuien()
    {
        for (int i = 0; i < worldLength; i++)
        {
            int randomMui = Random.Range(0, 3);
            float randomXOffset = Random.Range(-2.0f, 3.0f);
            float randomZOffset = Random.Range(-maxWorldZ, maxWorldZ + 1);
            Instantiate(muien[randomMui], new Vector3((i * 10) + randomXOffset, 0.5f, randomZOffset), Quaternion.identity, muiHolder.transform);
        }
    }
    private void DeleteMuien()
    {
        foreach(Transform child in muiHolder.transform)
        {
            Destroy(child.gameObject);
        }
        CreateMuien();
    }
}
