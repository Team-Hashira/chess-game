using UnityEngine;
using UnityEditor;

public class ChessMoveAreaWindowEditor : EditorWindow
{
    [MenuItem("Tools/ChessMoveAreaWindow")]
    private static void ShowWindow()
    {
        var window = GetWindow(typeof(ChessMoveAreaWindowEditor));
        window.position = new Rect(0, 0, 800, 600);
        window.titleContent = new GUIContent("ChessMoveAreaWindow");
        window.Show();
    }

	private void OnGUI()
	{
		
	}
}
