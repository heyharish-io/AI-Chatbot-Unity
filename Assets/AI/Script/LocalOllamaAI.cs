using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class LocalOllamaAI : MonoBehaviour
{
    [Header("Ollama Settings")]
    public string ollamaURL = "http://localhost:11434/api/generate";
    public string modelName = "Ollama3.2";

    [Header("UI")]
    public TMP_InputField userInput;
    public TMP_Text responseText;

    [Header("Prompt")]
    [TextArea(3, 8)]
    public string systemPrompt =
    "You are an AI assistant inside a Unity AR vehicle application. " +
    "You are helping the user learn about the vehicle shown in the AR experience. " +
    "The current vehicle is a tow truck. " +
    "Answer questions about the vehicle clearly, briefly, and in a user-friendly way. " +
    "You can explain general information such as the vehicle's purpose, typical features, components, usage, safety, and how a tow truck works. " +
    "Do not claim that you can see the camera, image, or 3D model unless that information is explicitly provided to you. " +
    "If the user asks something unrelated to the vehicle, politely say that you are designed to answer questions about the vehicle.";
    public void AskAI()
    {
        if (string.IsNullOrWhiteSpace(userInput.text))
        {
            responseText.text = "Please enter a question.";
            return;
        }

        responseText.text = "Thinking...";
        StartCoroutine(SendToOllama(userInput.text));
    }

    IEnumerator SendToOllama(string question)
    {
        OllamaRequest requestData = new OllamaRequest
        {
            model = modelName,
            prompt = systemPrompt + "\n\nUser: " + question,
            stream = false
        };

        string json = JsonUtility.ToJson(requestData);

        using (UnityWebRequest request =
               new UnityWebRequest(ollamaURL, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                OllamaResponse response =
                    JsonUtility.FromJson<OllamaResponse>(
                        request.downloadHandler.text
                    );

                responseText.text = response.response;
            }
            else
            {
                responseText.text =
                    "Ollama connection failed:\n" +
                    request.error;
            }
        }
    }

    [System.Serializable]
    public class OllamaRequest
    {
        public string model;
        public string prompt;
        public bool stream;
    }

    [System.Serializable]
    public class OllamaResponse
    {
        public string response;
    }
}