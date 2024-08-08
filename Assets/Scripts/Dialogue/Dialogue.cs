/*
 * Author: Alfred Kang Jing Rui
 * Date Created: 18/04/2024
 * Date Modified: 19/05/2024
 * Description: Dialogue Class
 */

using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

[CreateAssetMenu]
public class Dialogue : ScriptableObject
{
    /// <summary>
    /// Stores Dialogue Sentences
    /// </summary>
    [TextArea(3, 10)]
    public string[] sentences;

    /// <summary>
    /// Stores Dialogue Audio
    /// </summary>
    public AudioClip[] clips;

    /// <summary>
    /// Stores Dialogue Names
    /// </summary>
    [TextArea(3, 10)]
    public string[] names;
}
