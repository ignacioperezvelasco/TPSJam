using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject LoreControls;
    public GameObject Other;

    public void Play()
    {
        SceneManager.LoadScene("RbIgna");
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void LoreAndControls()
    {
        LoreControls.SetActive(true);
        Other.SetActive(false);
    }
    public void Return()
    {
        LoreControls.SetActive(false);
        Other.SetActive(true);
    }
}
