using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private float score = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        score = Time.realtimeSinceStartup;
        score = Mathf.Round(score * 10.0f) * 0.1f;
        scoreText.text = "Score:   " + score;
    }
}
