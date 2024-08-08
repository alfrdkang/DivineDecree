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

/// <summary>
/// Handles button events and functions on the main menu.
/// </summary>
public class MainMenuBtn : MonoBehaviour
{
    /// <summary>
    /// The text component of the button.
    /// </summary>
    public TMP_Text text;

    /// <summary>
    /// The description GameObject that appears on hover.
    /// </summary>
    private GameObject description;

    /// <summary>
    /// Initializes the description GameObject.
    /// </summary>
    private void Awake()
    {
        description = gameObject.transform.GetChild(1).gameObject;
    }

    /// <summary>
    /// Changes the text style to underline and shows the description on hover enter.
    /// </summary>
    public void HoverEnter()
    {
        text.fontStyle = FontStyles.Underline;
        description.SetActive(true);
    }

    /// <summary>
    /// Reverts the text style to normal and hides the description on hover exit.
    /// </summary>
    public void HoverExit()
    {
        text.fontStyle = FontStyles.Normal;
        description.SetActive(false);
    }

    /// <summary>
    /// Quits the application when the quit button is pressed.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}
