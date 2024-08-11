/*
 * Author: Alfred Kang Jing Rui
 * Date Created: 24/06/2024
 * Date Modified: 25/06/2024
 * Description: Script handling button events and functions on main menu.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEditor;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles button events and functions on the main menu.
/// </summary>
public class MainMenuBtn : MonoBehaviour
{
    /// <summary>
    /// The description GameObject that appears on hover.
    /// </summary>
    public GameObject description;

    /// <summary>
    /// CrossFade Scene Transition
    /// </summary>
    public Animator crossFade;

    /// <summary>
    /// Changes the text style to underline and shows the description on hover enter.
    /// </summary>
    public void HoverEnter()
    {
        description.SetActive(true);
    }

    /// <summary>
    /// Reverts the text style to normal and hides the description on hover exit.
    /// </summary>
    public void HoverExit()
    {
        description.SetActive(false);
    }

    /// <summary>
    /// Plays Game
    /// </summary>
    public void Play()
    {
        crossFade.SetTrigger("Start");
        SceneManager.LoadScene(1);
    }

    /// <summary>
    /// Quits the application when the quit button is pressed.
    /// </summary>
    public void Quit()
    {
        SceneManager.LoadScene(0);
    }

    public void QuitApp()
    {
        Application.Quit();
    }
}
