using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Buttons : MonoBehaviour
{
   

    public void Settings()
    {
        SceneManager.LoadScene("Settings");
    }
    public void Menu()
    {
        SceneManager.LoadScene("Menu");

    }
    public void Play()
    {
        SceneManager.LoadScene("Players");

    }
    public void Game()
    {
        SceneManager.LoadScene("Game");

    }
    public void Player()
    {
        SceneManager.LoadScene("Profile");

    }
}
