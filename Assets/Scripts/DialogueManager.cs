/*
 * Author: Alfred Kang Jing Rui
 * Date Created: 18/05/2024
 * Date Modified: 19/05/2024
 * Description: NPC and Player Dialogue Script
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    /// <summary>
    /// Dialogue when player starts game
    /// </summary>
    public Dialogue tutorialDialogue;
    /// <summary>
    /// Dialogue when player talks to Annie BEFORE obtaining all artifacts
    /// </summary>
    public Dialogue talk;
    /// <summary>
    /// Dialogue when player talks to Annie AFTER obtaining all artifacts
    /// </summary>
    public Dialogue talk2;
    /// <summary>
    /// Dialogue when player picks up Strength Artifact Orb
    /// </summary>
    public Dialogue pickUpStrength;
    /// <summary>
    /// Dialogue when player picks up Jump Artifact Orb
    /// </summary>
    public Dialogue pickUpJump;
    /// <summary>
    /// Dialogue when player picks up Dash Artifact Orb
    /// </summary>
    public Dialogue pickUpDash;
    /// <summary>
    /// Dialogue when player picks up Updraft Artifact Orb
    /// </summary>
    public Dialogue pickUpUpdraft;
    /// <summary>
    /// Dialogue when player obtain all 4 Artifact Orbs
    /// </summary>
    public Dialogue artifactsObtained;
    /// <summary>
    /// Dialogue when player picks up Key
    /// </summary>
    public Dialogue pickUpKey;
    /// <summary>
    /// Dialogue when player obtains all coins
    /// </summary>
    public Dialogue allCoins;

    /// <summary>
    /// Dialogue UI Text Element
    /// </summary>
    public TextMeshProUGUI DiagText;
    /// <summary>
    /// Player/NPC Dialogue background Image
    /// </summary>
    public GameObject NPCDiagBG;

    /// <summary>
    /// Annie's Dialogue Background
    /// </summary>
    public Sprite AnnieBG;
    /// <summary>
    /// Arthur/Player's Dialogue Background
    /// </summary>
    public Sprite ArthurBG;

    /// <summary>
    /// Queue of sentences to be played during dialogue session
    /// </summary>
    private Queue<string> sentences;
    /// <summary>
    /// Queue of names to be played during dialogue session
    /// </summary>
    private Queue<string> names;

    /// <summary>
    /// Boolean to check if dialogue is ongoing
    /// </summary>
    public bool diagActive = false;

    // Start is called before the first frame update
    void Start()
    {
        DiagText = GameObject.Find("DiagText").GetComponent<TextMeshProUGUI>();
        NPCDiagBG = GameObject.Find("NPCDiagBG");
        sentences = new Queue<string>();
        names = new Queue<string>();
    }

    /// <summary>
    /// Function to start dialogue
    /// </summary>
    /// <param name="dialogue"></param>
    public void StartDialogue(Dialogue dialogue)
    {
        NPCDiagBG.SetActive(true);
        diagActive = true;

        sentences.Clear();
        names.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
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
        string name = names.Dequeue();

        if (name == "Annie")
        {
            NPCDiagBG.GetComponent<Image>().sprite = AnnieBG;
        } else
        {
            NPCDiagBG.GetComponent<Image>().sprite = ArthurBG;
        }
        DiagText.text = sentence;
    }

    /// <summary>
    /// Function to end dialogue when queue is empty or player skips with Enter Key
    /// </summary>
    void EndDialogue()
    {
        NPCDiagBG.SetActive(false);
        diagActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) | Input.GetMouseButtonDown(1))
        {
            DisplayNextSentence();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EndDialogue();
        }
    }
}
