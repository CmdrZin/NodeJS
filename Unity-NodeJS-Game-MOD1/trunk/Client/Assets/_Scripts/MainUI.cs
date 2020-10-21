using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Required when Using UI elements.

public class MainUI : MonoBehaviour
{
    public Text nameBox;

    public void SetNameBoxText(string text)
    {
        nameBox.text = text;
    }

    public void SetNameBoxColor(Color color)
    {
        nameBox.color = color;
    }

    /* Provide methods to show Player Health and other Statistics. */

}
