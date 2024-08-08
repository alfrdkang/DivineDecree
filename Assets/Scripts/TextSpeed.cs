using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSpeed : MonoBehaviour
{
    // Reference to the TextMeshPro component
    public TextMeshProUGUI textComponent;

    // The full text that you want to display
    public string fullText;

    // The speed at which the text will appear (in seconds per character)
    public float textSpeed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        // Start the coroutine to display the text
        StartCoroutine(DisplayText());
    }

    // Coroutine to display the text one character at a time
    IEnumerator DisplayText()
    {
        textComponent.text = "";  // Clear any existing text

        for (int i = 0; i < fullText.Length; i++)
        {
            textComponent.text += fullText[i];  // Add the next character
            yield return new WaitForSeconds(textSpeed);  // Wait for the specified time
        }
    }
}
