/*
 * Author: Alfred Kang Jing Rui
 * Date Created: 22/06/2024
 * Date Modified: 25/06/2024
 * Description: Script for options on Options menu
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Controls the options menu functionality, including volume, quality settings, fullscreen mode, and resolution.
/// </summary>
public class OptionsMenu : MonoBehaviour
{
    public Toggle qualityLowToggle; // The audio mixer to control volume
    public Toggle qualityMediumToggle; // The audio mixer to control volume
    public Toggle qualityHighToggle; // The audio mixer to control volume

    public AudioMixer audioMixer; // The audio mixer to control volume

    public TMP_Dropdown resolutionDropdown; // Dropdown UI for resolution selection

    private Resolution[] resolutions; // Array of available screen resolutions

    private void Start()
    {
        resolutions = Screen.resolutions; // Get all available screen resolutions

        resolutionDropdown.ClearOptions(); // Clear existing dropdown options

        List<string> options = new List<string>(); // List to store resolution options

        int currentResolutionIndex = 0; // Index of the current screen resolution
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height; // Format resolution as string
            options.Add(option); // Add formatted resolution to options list

            // Check if this resolution matches the current screen resolution
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i; // Set current resolution index
            }
        }

        resolutionDropdown.AddOptions(options); // Add resolution options to dropdown
        resolutionDropdown.value = currentResolutionIndex; // Set dropdown value to current resolution index
        resolutionDropdown.RefreshShownValue(); // Refresh dropdown UI to show current value
    }

    /// <summary>
    /// Sets the volume level using the provided slider value.
    /// </summary>
    /// <param name="volume">The volume level to set.</param>
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume); // Set the volume level in the audio mixer
    }

    public void LowQuality()
    {
        if (qualityLowToggle.isOn)
        {
            QualitySettings.SetQualityLevel(0);
            qualityMediumToggle.isOn = false;
            qualityHighToggle.isOn = false;
        }
    }

    public void MediumQuality()
    {
        if (qualityMediumToggle.isOn)
        {
            QualitySettings.SetQualityLevel(1);
            qualityLowToggle.isOn = false;
            qualityHighToggle.isOn = false;
        }
    }

    public void HighQuality()
    {
        if (qualityHighToggle.isOn)
        {
            QualitySettings.SetQualityLevel(2);
            qualityMediumToggle.isOn = false;
            qualityLowToggle.isOn = false;
        }
    }

    /// <summary>
    /// Sets whether the game should run in fullscreen mode.
    /// </summary>
    /// <param name="isFullscreen">True to enable fullscreen mode, false otherwise.</param>
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen; // Set fullscreen mode
    }

    /// <summary>
    /// Sets the screen resolution based on the selected dropdown index.
    /// </summary>
    /// <param name="resolutionIndex">The index of the resolution to set.</param>
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex]; // Get selected resolution
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen); // Set screen resolution
    }

    public void OpenInstagramLink()
    {
        Application.OpenURL("https://www.instagram.com/sleepystudiosgame?igsh=MTcyZzB3dm5rd3FzZQ%3D%3D&utm_source=qr");
    }
}
