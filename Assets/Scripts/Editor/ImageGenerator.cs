using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using UnityOpenAI;
using System.Threading.Tasks;
using Codice.Client.Common.GameUI;
using Codice.Client.BaseCommands.BranchExplorer;
using Unity.EditorCoroutines.Editor;
using System;

public class ImageGenerator : EditorWindow
{

    public static Rect Section;
    public static Rect imageSection;

    static Color emptyColor = new Color(100, 100, 1000);

    const string url = "https://api.openai.com/v1/images/generations";

    private static string prompt;
    public static string inputPrompt;
    private static string result;
    public static Texture2D inputResults;
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

        // Input Prompt
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = false;
        prompt = GUILayout.TextField("InputPrompt", style);
        GUI.enabled = true;
        inputPrompt = EditorGUILayout.TextField(inputPrompt);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUI.enabled = false;
        result = GUILayout.TextField("InputResult", style);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        //Picture
        EditorGUILayout.BeginHorizontal();

        GUILayout.Label("Picture:", EditorStyles.boldLabel);
        inputResults = (Texture2D)EditorGUILayout.ObjectField(inputResults, typeof(Texture2D), false, GUILayout.Width(100), GUILayout.Height(100));
        if (GUILayout.Button("Execute"))
        {
            Execute();
        }
        if (GUILayout.Button("Create and Save Texture"))
        {
            CreateAndSaveTexture();
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.EndArea();
    }
    /*
    private static IEnumerator DownloadAndLoadImage(string url, System.Action<Texture2D> callback)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Texture2D generatedImage = ((DownloadHandlerTexture)request.downloadHandler).texture;
            callback?.Invoke(generatedImage);
            //inputResults = generatedImage;
            Debug.Log($"Generated image: {inputResults.width}x{inputResults.height}");
        }

        isRunning = false;
    }*/



    public static async void Execute()
    {
        if (isRunning)
        {
            Debug.LogError("Already running");
            return;
        }
        isRunning = true;

        RequestImageData requestData = new RequestImageData()
        {
            prompt = inputPrompt,
            n = 1,
            size = "1024x1024"
        };

        string jsonData = JsonUtility.ToJson(requestData);

        byte[] postData = System.Text.Encoding.UTF8.GetBytes(jsonData);

        UnityWebRequest imageRequest = UnityWebRequest.Post(url, jsonData);
        imageRequest.uploadHandler = new UploadHandlerRaw(postData);
        imageRequest.downloadHandler = new DownloadHandlerBuffer();
        imageRequest.SetRequestHeader("Content-Type", "application/json");
        imageRequest.SetRequestHeader("Authorization", "Bearer " + GeneralSettings.authKey);

        var asyncOp = imageRequest.SendWebRequest();

        while (!asyncOp.isDone)
        {
            await Task.Delay(100);
        }

        if (imageRequest.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.LogError(imageRequest.error);
        }
        else
        {
            ResponseImageData responseData = JsonUtility.FromJson<ResponseImageData>(imageRequest.downloadHandler.text);
            Debug.Log(imageRequest.downloadHandler.text);

            UnityWebRequest imageDownloadRequest = UnityWebRequestTexture.GetTexture(responseData.data[0].url);
            var imageDownloadAsyncOp = imageDownloadRequest.SendWebRequest();

            while (!imageDownloadAsyncOp.isDone)
            {
                await Task.Delay(100);
            }

            if (imageDownloadRequest.result == UnityWebRequest.Result.ConnectionError || imageDownloadRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(imageDownloadRequest.error);
            }
            else
            {
                Texture2D generatedImage = ((DownloadHandlerTexture)imageDownloadRequest.downloadHandler).texture;
                    inputResults = generatedImage;
                
            }
        }

        isRunning = false;
    }



    private static void CreateAndSaveTexture()
    {
        if (inputResults == null)
        {
            Debug.LogError("No image to save.");
            return;
        }

        // Save the texture to disk
        byte[] pngData = inputResults.EncodeToPNG();
        string filePath = EditorUtility.SaveFilePanel("Save Texture", "", "new_texture.png", "png");
        if (filePath.Length > 0)
        {
            System.IO.File.WriteAllBytes(filePath, pngData);
            AssetDatabase.Refresh();
            Debug.Log("Texture saved to " + filePath);
        }
    }
}
