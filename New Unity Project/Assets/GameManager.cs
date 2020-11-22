using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject pause, screenWin, ScreenLose;
    public Text timerText;
    public rvMovementPers player;
    public int numEnemies = 1;
    float timerTotal = 65;
    int min, secs;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length + GameObject.FindGameObjectsWithTag("EnemyFollow").Length;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<rvMovementPers>();
    }

    // Update is called once per frame
    void Update()
    {
        numEnemies = GameObject.FindGameObjectsWithTag("Enemy").Length + GameObject.FindGameObjectsWithTag("EnemyFollow").Length;

        timerTotal -= Time.deltaTime;
        min = ((int)timerTotal / 60);
        secs = ((int)timerTotal % 60);
        if(secs<10)
            timerText.text = "0"+min.ToString()+ ":0" + secs.ToString();
        else
            timerText.text = "0" + min.ToString() + ":" + secs.ToString();

        if (Input.GetKeyDown(KeyCode.P))
        {
            pause.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
        }
        if (player.dead || (timerTotal<=0))
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

    public void AddMinute()
    {
        timerTotal += 60;
    }
}
