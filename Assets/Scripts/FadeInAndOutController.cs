using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInAndOutController : MonoBehaviour
{
    public Image BlackScreen;
    public static bool FadeInAndOut = false;
    public bool AIncreasing = false;
    public float Opacity = 0f;
    public float ChangeSpeedForScreen = 0.05f;


    void Update()
    {
        if (FadeInAndOut)
        {
            FadeInAndOut = false;
            AIncreasing = true;

        }

        if (Opacity < 1f && AIncreasing)
        {
            Opacity += ChangeSpeedForScreen;
        }
        else
        {
            if (AIncreasing)
            {
                AIncreasing = false;
            }

        }
        if (!AIncreasing && Opacity > 0f)
        {
            Opacity -= ChangeSpeedForScreen;
        }
        BlackScreen.color = new Color(0f, 0f, 0f, Opacity);

    }
}
