
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

public class CameraSwitch : MonoBehaviour
{

    [SerializeField] bool isGodView = true;
    [SerializeField] Object godView;
    [SerializeField] Object characterView;

    private void Start()
    {
        godView.GetComponent<Camera>().enabled = isGodView;
        godView.GetComponent<AudioListener>().enabled = isGodView;
        characterView.GetComponent<Camera>().enabled = !isGodView;
        characterView.GetComponent<AudioListener>().enabled = !isGodView;
    }


    public void changeView()
    {
        this.isGodView = !isGodView;
        godView.GetComponent<Camera>().enabled = isGodView;
        godView.GetComponent<AudioListener>().enabled = isGodView;
        characterView.GetComponent<Camera>().enabled = !isGodView;
        characterView.GetComponent<AudioListener>().enabled = !isGodView;
    }
}
