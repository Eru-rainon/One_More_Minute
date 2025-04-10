using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class menumanager : MonoBehaviour
{
    public void play(){
        SceneManager.LoadScene(1);
    }
    public void quitGame(){
        Application.Quit();
    }
}
