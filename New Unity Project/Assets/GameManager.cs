using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject pause, screenWin, ScreenLose;
    public rvMovementPers player;
    public int numEnemies = 1;
    // Start is called before the first frame update
    void Start()
    {
        numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length + GameObject.FindGameObjectsWithTag("EnemyFollow").Length;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<rvMovementPers>();
    }

    // Update is called once per frame
    void Update()
    {
        numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length + GameObject.FindGameObjectsWithTag("EnemyFollow").Length;
        
        if (Input.GetKeyDown(KeyCode.P))
        {
            pause.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
        }
        if (player.dead)
        {
            ScreenLose.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
        }
    }

    public void WinPanel()
    {
        screenWin.SetActive(true);
        Cursor.visible = true;
        Time.timeScale = 0;
    }

    public void unPauseGame()
    {
        pause.SetActive(false);
        Time.timeScale = 1;
    }

    public void ResetScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("RbIgna");
    }
    public void MenuScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }
}
