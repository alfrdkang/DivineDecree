/*
 * Author: Alfred Kang Jing Rui
 * Date Created: 31/07/2024
 * Date Modified: 
 * Description: Dialogue Manager Script
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> sentences;
    private Queue<AudioClip> clips;
    private Queue<string> names;

    public bool diagActive = false;

    public CinemachineVirtualCamera diagVirtualCamera;
    public GameObject playerModel;
    public TextMeshProUGUI DiagText;
    public TextMeshProUGUI DiagName;
    public GameObject DiagUI;
    public TextMeshProUGUI interactText;
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        diagVirtualCamera.gameObject.SetActive(false);
        DiagUI.SetActive(false);
        sentences = new Queue<string>();
        clips = new Queue<AudioClip>();
        names = new Queue<string>();
    }

    /// <summary>
    /// Function to start dialogue
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(Dialogue dialogue)
    {
        playerModel.SetActive(false);
        diagVirtualCamera.gameObject.SetActive(true);
        DiagUI.SetActive(true);
        diagActive = true;
        interactText.enabled = false;

        sentences.Clear();
        clips.Clear();
        names.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        foreach (AudioClip clip in dialogue.clips)
        {
            clips.Enqueue(clip);
        }

        foreach (string name in dialogue.names)
        {
            names.Enqueue(name);
        }

        DisplayNextSentence();
    }

    /// <summary>
    /// Function to display next sentence and name when user clicks
    /// </summary>
    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        AudioClip clip = clips.Dequeue();
        string name = names.Dequeue();

        DiagName.text = name;
        DiagText.text = sentence;
        audioSource.clip = clip;
        audioSource.Play();
    }

    /// <summary>
    /// Function to end dialogue when queue is empty or player skips with Enter Key
    /// </summary>
    void EndDialogue()
    {
        sentences.Clear();
        clips.Clear();
        names.Clear();

        audioSource.Stop();

        diagVirtualCamera.gameObject.SetActive(false);
        diagActive = false;
        DiagUI.SetActive(false);
        playerModel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) | Input.GetMouseButtonDown(1) | Input.GetKeyDown(KeyCode.Space))
        {
            DisplayNextSentence();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EndDialogue();
        }
    }
}
