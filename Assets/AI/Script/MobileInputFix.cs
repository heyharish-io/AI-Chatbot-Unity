using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MobileInputFix : MonoBehaviour, IPointerDownHandler
{
    private TMP_InputField inputField;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (inputField == null)
            return;

        inputField.Select();
        inputField.ActivateInputField();

#if UNITY_ANDROID || UNITY_IOS
        TouchScreenKeyboard.Open(
            inputField.text,
            TouchScreenKeyboardType.Default,
            false,
            false,
            false,
            false,
            "Ask anything..."
        );
#endif
    }
}