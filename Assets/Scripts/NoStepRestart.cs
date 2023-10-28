using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;
using UnityEngine.SceneManagement;

public class NoStepRestart : MonoBehaviour
{
    [SerializeField] public static int backToOrigin;

    public static void noStepRestart()
    {
        SceneManager.LoadScene(backToOrigin);
    }

    public void toManu()
    {
        SceneManager.LoadScene(0);
    }


}
