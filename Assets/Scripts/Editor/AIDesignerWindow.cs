using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Unity.VisualScripting;
using UnityEditor.TerrainTools;
using static UnityEngine.GraphicsBuffer;
using UnityEditor.PackageManager.UI;
using Unity.EditorCoroutines.Editor;


    public class AIDesignerWindow : EditorWindow
    {
        //GeneralSettings generalSettings;
        public string key;
        private int currentPage = 1;

        [MenuItem("Window/AI Designer")]
        public static void OpenWindow()
        {
            AIDesignerWindow window = (AIDesignerWindow)GetWindow(typeof(AIDesignerWindow));
            window.minSize = new Vector2(600, 300);
            window.Show();
        }

        private void OnEnable()
        {
            //GeneralSettings generalSettings = GetWindow<GeneralSettings>();
        }
        void OnGUI()
        {
            // Draw a toolbar with buttons to switch between pages
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GUILayout.Button("Text Generator", EditorStyles.toolbarButton))
            {
                currentPage = 1;
            }
            if (GUILayout.Button("Image Generator", EditorStyles.toolbarButton))
            {
                currentPage = 2;
            }
            if (GUILayout.Button("General Settings", EditorStyles.toolbarButton))
            {
                currentPage = 3;
            }
        GUILayout.EndHorizontal();
            
            // Draw the GUI for the current page
            switch (currentPage)
            {
                case 1:
                TextGenerator.Draw();
                break;
                case 2:
                ImageGenerator.Draw();
                break;
                case 3:
                GeneralSettings.Draw();
                break;
            }
            
        }
    }



