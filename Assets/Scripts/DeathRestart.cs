using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.SceneManagement;

public class DeathRestart : MonoBehaviour
{
    [SerializeField] public static int backTo;

    public static void deathRestart()
    {
        SceneManager.LoadScene(backTo);
    }

    public void toManu()
    {
        SceneManager.LoadScene(GameJump.selectIndex);
    }

    public void toStart()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }


}
