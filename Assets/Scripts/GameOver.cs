using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    public void Restart() {
    	PlayerStats.ResetInstance();
    	UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void Quit() {
    	Application.Quit();
    }
}
