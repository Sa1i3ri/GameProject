using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameJump : MonoBehaviour
{
    private int turtorial = 3;
    private int level_1 = 9;
    private int level_2 = 13;
    private int level_3 = 18;
    public static int deathMenuIndex = 24;
    public static int noStepIndex = 25;
    public static int selectIndex = 2;
    public void startGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void ToTorturial()
    {
        SceneManager.LoadScene(this.turtorial);
    }

    public void ToLevel_1()
    {
        SceneManager.LoadScene(level_1);
    }

    public void ToLevel_2()
    {
        SceneManager.LoadScene(level_2);
    }

    public void ToLevel_3()
    {
        SceneManager.LoadScene(level_3);
    }

    public void restart()
    {
        EnemyController.EnemyNum = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }



}
