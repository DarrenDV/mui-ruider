using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LivesTest : MonoBehaviour
{

    [SerializeField] private Text livesText;

    [SerializeField] private int lives = 3;

    private void Start()
    {
        livesText = GetComponent<Text>();
        livesText.text = "Strikes left: " + lives;
    }

    public void RemoveLife()
    {
        lives--;
        livesText.text = "Strikes left: " + lives;

        if (lives < 0)
        {
            GameOver();
        }
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
}
