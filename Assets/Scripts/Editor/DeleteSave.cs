using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DeleteSave : EditorWindow
{
    [MenuItem("Game Settings/Delete All Save", false, 2)]
    static void ShowWindow()
    {
        PlayerPrefs.DeleteKey("Save");
    }
}
