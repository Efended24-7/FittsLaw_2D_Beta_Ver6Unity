using UnityEngine;
using UnityEngine.UI;

// MoveButtonRight:
// Purpose of this script is to move the button to the right as soon as it is clicked.

public class OnClick : MonoBehaviour
{
    public Button myButton;        // Assign your UI Button in the Inspector
    public float moveAmount = 10f; // How many pixels to move per click

    private RectTransform buttonRect;

    void Start()
    {
        if (myButton != null)
        {
            buttonRect = myButton.GetComponent<RectTransform>();
            myButton.onClick.AddListener(MoveButton);
        }
        else
        {
            Debug.LogError("Button not assigned in the Inspector!");
        }
    }

    void MoveButton()
    {
        // Move the button to the right
        buttonRect.anchoredPosition += new Vector2(moveAmount, 0);

        // Print message to the Console
        Debug.Log("Button moved to the right!");
    }
}
