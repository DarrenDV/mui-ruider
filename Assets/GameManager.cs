using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject BeachPrefab;
    [SerializeField] private float beachSize = 50f;

    private int currentBeach;

    private void Update()
    {
        if(currentBeach < 10)
        {
            Instantiate(BeachPrefab);
            BeachPrefab.transform.position = new Vector3(currentBeach * beachSize, 0, 0);
            currentBeach++;
        }
    }
}
