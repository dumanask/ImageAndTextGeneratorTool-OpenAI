using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class GeneralSettings : EditorWindow
{
    private static string authName = "Key";
    public static string authKey;
    private static string authOrg = "Organization";
    private static string authOrganization;

    private const string AuthKeyPrefKey = "AuthKey";
    private const string AuthOrgPrefKey = "AuthOrg";

    private static void LoadPreferences()
    {
        authKey = PlayerPrefs.GetString(AuthKeyPrefKey, "");
        authOrganization = PlayerPrefs.GetString(AuthOrgPrefKey, "");
    }

    private static void SavePreferences()
    {
        PlayerPrefs.SetString(AuthKeyPrefKey, authKey);
        PlayerPrefs.SetString(AuthOrgPrefKey, authOrganization);
        PlayerPrefs.Save();
    }

    public static void Draw()
    {
        LoadPreferences();

        GUILayout.Label("Settings");

        // Key
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(authName, GUILayout.Width(100));
        authKey = EditorGUILayout.TextField(authKey);
        EditorGUILayout.EndHorizontal();

        // Organization
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label(authOrg, GUILayout.Width(100));
        authOrganization = EditorGUILayout.TextField(authOrganization);
        EditorGUILayout.EndHorizontal();

        SavePreferences();
    }
}
