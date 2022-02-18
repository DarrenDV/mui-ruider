using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuiSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject[] muien = new GameObject[3];
    [SerializeField] private GameObject muiHolder;
    [SerializeField] private GameObject endZone;
    public float worldLength;
    public int maxWorldX;
    private void Start()
    {
        CreateMuien();
        Instantiate(endZone);
        endZone.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + ((worldLength+1) * 10));
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
        Vector3 startPos = transform.position;
        for (int i = 0; i < worldLength; i++)
        {
            int randomMui = Random.Range(0, 3);
            float randomZOffset = Random.Range(-2.0f, 3.0f);
            float randomXOffset = Random.Range(-maxWorldX + 1, maxWorldX + 1);
            Instantiate(muien[randomMui], new Vector3(startPos.x + randomXOffset, 0.5f, startPos.z + (i * 10) + randomZOffset), Quaternion.identity, muiHolder.transform);
        }
    }
    public void DeleteMuien()
    {
        foreach(Transform child in muiHolder.transform)
        {
            Destroy(child.gameObject);
        }
        CreateMuien();
    }
}
