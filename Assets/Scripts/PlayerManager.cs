using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static bool gameOver;
    public GameObject gameOverPanel;

    public static bool isGameStarted;
    public GameObject tapToStartText ;
    void Start()
    {
        Time.timeScale = 1;
        gameOver = false;
        isGameStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameOver)
        {
            Time.timeScale = 0;
            gameOverPanel.SetActive(true);
        }

        if(SwipeManager.tap)
        {
            isGameStarted = true;
            Destroy(tapToStartText);
        }
    }
}
