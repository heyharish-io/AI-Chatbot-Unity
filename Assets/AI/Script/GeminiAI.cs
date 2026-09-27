using System;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class GeminiAI : MonoBehaviour
{
    [Header("Gemini API")]
    [SerializeField] private string apiKey = "";
    [SerializeField] private string model = "gemini-3.5-flash-lite";

    [Header("Existing UI")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text responseText;

    private const string BaseURL =
        "https://generativelanguage.googleapis.com/v1beta/models/";

    public void AskGemini()
    {
        if (inputField == null || responseText == null)
        {
            Debug.LogError("GeminiAI: UI references are missing.");
            return;
        }

        string question = inputField.text.Trim();

        if (string.IsNullOrEmpty(question))
        {
            responseText.text = "Please enter a question.";
            return;
        }

        if (string.IsNullOrWhiteSpace(apiKey))
        {
            responseText.text = "API key is missing.";
            Debug.LogError("GeminiAI: API key is empty.");
            return;
        }

        // Show immediately when button is pressed
        responseText.text = "Thinking...";

        StartCoroutine(SendRequest(question));
    }

    private IEnumerator SendRequest(string question)
    {
        string url =
            BaseURL +
            model +
            ":generateContent?key=" +
            UnityWebRequest.EscapeURL(apiKey);

        GeminiRequest data = new GeminiRequest
        {
            contents = new Content[]
            {
                new Content
                {
                    parts = new Part[]
                    {
                        new Part
                        {
                            text = question
                        }
                    }
                }
            }
        };

        string json = JsonUtility.ToJson(data);

        byte[] body = Encoding.UTF8.GetBytes(json);

        using (UnityWebRequest request =
               new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(body);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader(
                "Content-Type",
                "application/json"
            );

            yield return request.SendWebRequest();

            string result =
                request.downloadHandler != null
                ? request.downloadHandler.text
                : "";

            if (request.result != UnityWebRequest.Result.Success)
            {
                responseText.text = "AI connection failed.";

                Debug.LogError(
                    "Gemini Error: " +
                    request.responseCode +
                    "\n" +
                    request.error +
                    "\n" +
                    result
                );

                yield break;
            }

            GeminiResponse response = null;

            try
            {
                response =
                    JsonUtility.FromJson<GeminiResponse>(result);
            }
            catch (Exception error)
            {
                responseText.text = "Invalid AI response.";

                Debug.LogError(
                    "Gemini JSON error: " +
                    error.Message +
                    "\n" +
                    result
                );

                yield break;
            }

            if (response != null &&
                response.candidates != null &&
                response.candidates.Length > 0 &&
                response.candidates[0] != null &&
                response.candidates[0].content != null &&
                response.candidates[0].content.parts != null &&
                response.candidates[0].content.parts.Length > 0)
            {
                string answer =
                    response.candidates[0]
                    .content
                    .parts[0]
                    .text;

                if (string.IsNullOrWhiteSpace(answer))
                {
                    responseText.text = "No response received.";
                }
                else
                {
                    responseText.text = answer;
                }
            }
            else
            {
                responseText.text = "No response received.";

                Debug.LogError(
                    "Gemini returned no usable response:\n" +
                    result
                );
            }
        }
    }

    // =========================
    // REQUEST CLASSES
    // =========================

    [Serializable]
    private class GeminiRequest
    {
        public Content[] contents;
    }

    [Serializable]
    private class Content
    {
        public Part[] parts;
    }

    [Serializable]
    private class Part
    {
        public string text;
    }

    // =========================
    // RESPONSE CLASSES
    // =========================

    [Serializable]
    private class GeminiResponse
    {
        public Candidate[] candidates;
    }

    [Serializable]
    private class Candidate
    {
        public Content content;
    }
}