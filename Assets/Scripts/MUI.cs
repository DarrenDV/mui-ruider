using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MUI : MonoBehaviour
{
    public bool hasPlacedFlag = false;

    public void LeftMui()
    {
        if (!hasPlacedFlag)
        {
            GameObject.Find("LivesText").GetComponent<LivesTest>().RemoveLife();
        }
    }
}
