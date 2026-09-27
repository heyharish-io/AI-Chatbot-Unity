using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChatUIStyle : MonoBehaviour
{
    [Header("Existing UI")]
    public RectTransform chatPanel;
    public RectTransform inputField;
    public RectTransform sendButton;
    public RectTransform responseBackground;
    public RectTransform responseText;
    public RectTransform title;

    [Header("Colors")]
    public Color panelColor = new Color(0.055f, 0.065f, 0.085f, 0.96f);
    public Color inputColor = new Color(0.12f, 0.14f, 0.18f, 1f);
    public Color responseColor = new Color(0.09f, 0.105f, 0.135f, 1f);
    public Color buttonColor = new Color(0.20f, 0.45f, 0.95f, 1f);
    public Color white = Color.white;

    private void Awake()
    {
        ApplyUI();
    }

    private void ApplyUI()
    {
        // Chat Panel
        if (chatPanel != null)
        {
            Image image = chatPanel.GetComponent<Image>();

            if (image != null)
                image.color = Color.white;
        }

        // Title
        if (title != null)
        {
            TMP_Text text = title.GetComponent<TMP_Text>();

            if (text != null)
            {
                text.text = "AI ASSISTANT";
                text.fontSize = 42;
                text.fontStyle = FontStyles.Bold;
                text.alignment = TextAlignmentOptions.Center;
                text.color = white;
            }
        }

        // Input Field
        if (inputField != null)
        {
            Image image = inputField.GetComponent<Image>();

            if (image != null)
                image.color = Color.white;

            TMP_InputField field =
                inputField.GetComponent<TMP_InputField>();

            if (field != null)
            {
                if (field.textComponent != null)
                {
                    field.textComponent.fontSize = 25;
                    field.textComponent.color = white;
                }

                if (field.placeholder != null)
                {
                    TMP_Text placeholder =
                        field.placeholder.GetComponent<TMP_Text>();

                    if (placeholder != null)
                    {
                        placeholder.text = "Ask me anything......";
                        placeholder.fontSize = 24;
                        placeholder.color =
                            new Color(0.65f, 0.68f, 0.72f, 1f);
                    }
                }
            }
        }

        // Send Button
        if (sendButton != null)
        {
            Image image = sendButton.GetComponent<Image>();

            if (image != null)
                image.color = buttonColor;

            Button button = sendButton.GetComponent<Button>();

            if (button != null)
            {
                ColorBlock colors = button.colors;

                colors.normalColor = buttonColor;
                colors.highlightedColor =
                    new Color(0.28f, 0.52f, 1f, 1f);
                colors.pressedColor =
                    new Color(0.15f, 0.35f, 0.8f, 1f);

                button.colors = colors;
            }

            TMP_Text buttonText =
                sendButton.GetComponentInChildren<TMP_Text>();

            if (buttonText != null)
            {
                buttonText.text = "ASK AI";
                buttonText.fontSize = 24;
                buttonText.fontStyle = FontStyles.Bold;
                buttonText.alignment = TextAlignmentOptions.Center;
                buttonText.color = white;
            }
        }

        // Response Background
        if (responseBackground != null)
        {
            Image image =
                responseBackground.GetComponent<Image>();

            if (image != null)
                image.color = Color.white;
        }

        // Response Text
        if (responseText != null)
        {
            TMP_Text text =
                responseText.GetComponent<TMP_Text>();

            if (text != null)
            {
                text.fontSize = 25;
                text.color = white;
                text.alignment = TextAlignmentOptions.TopLeft;
                text.enableWordWrapping = true;
                text.overflowMode = TextOverflowModes.Truncate;
            }
        }
    }
}