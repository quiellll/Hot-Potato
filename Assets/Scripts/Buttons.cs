using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Buttons : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
