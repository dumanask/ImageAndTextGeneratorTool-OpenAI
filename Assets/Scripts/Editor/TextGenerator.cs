using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityOpenAI;

public class TextGenerator : EditorWindow
{
    public static Rect Section;
    const string url = "https://api.openai.com/v1/completions";

    private static string model;
    public static string modelName = "text-davinci-003";
    private static string prompt;
    public static string inputPrompt;
    private static string result;
    public static string inputResults;

    static bool isRunning = false;

    public static void Draw()
    {
        Section.x = 0;
        Section.y = 30;
        Section.width = Screen.width;
        Section.height = 150;

        GUIStyle style = new GUIStyle(GUI.skin.textField);
        style.fixedWidth = 100;

        GUILayout.BeginArea(Section);

        GUILayout.Label("Settings");

        // Model Name
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = false;
        model = GUILayout.TextField("ModelName", style);
        GUI.enabled = true;
        modelName = EditorGUILayout.TextField(modelName);
        EditorGUILayout.EndHorizontal();

        // Input Prompt
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = false;
        prompt = GUILayout.TextField("InputPrompt", style);
        GUI.enabled = true;
        inputPrompt = EditorGUILayout.TextField(inputPrompt);
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Execute"))
        {
            Execute();
        }

        // Input Results
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = false;
        result = GUILayout.TextField("InputResult", style);
        GUI.enabled = true;
        inputResults = EditorGUILayout.TextField(inputResults);
        EditorGUILayout.EndHorizontal();

        GUILayout.EndArea();
    }

    public static void Execute()
    {
        if (isRunning)
        {
            Debug.LogError("Already running");
            return;
        }
        isRunning = true;

        RequestCompletionsData requestData = new RequestCompletionsData()
        {
            model = modelName,
            prompt = inputPrompt,
            temperature = 0.7f,
            max_tokens = 256,
            top_p = 1,
            frequency_penalty = 0,
            presence_penalty = 0
        };

        string jsonData = JsonUtility.ToJson(requestData);

        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest request = UnityWebRequest.Post(url, jsonData);
        request.uploadHandler = new UploadHandlerRaw(postData);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Authorization", "Bearer " + GeneralSettings.authKey);
        UnityWebRequestAsyncOperation async = request.SendWebRequest();
        async.completed += (op) =>
        {
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.LogError(request.error);
            }
            else
            {

                CompletionsOpenAIAPI responseData = JsonUtility.FromJson<CompletionsOpenAIAPI>(request.downloadHandler.text);
                string generatedText = responseData.choices[0].text.TrimStart('\n').TrimStart('\n');
                inputResults = generatedText;
            }
            isRunning = false;
        };
    }



}
