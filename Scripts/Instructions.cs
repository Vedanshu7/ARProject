using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    public Button buttonToChange;

    // Call this method when the button is clicked
    public void OnButtonClick()
    {
        if (buttonToChange != null)
        {
            Text buttonText = buttonToChange.GetComponentInChildren<Text>();
            if (buttonText != null)
            {
                buttonText.color = Color.blue;
            }
        }
    }
}
