using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StageMapGenerator.StageMapGenerator))]
public class StageMapGeneratorEditor : Editor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		GUILayout.Space(25);

		if(GUILayout.Button("GenerateMap"))
		{
			if (Application.isPlaying == false)
			{
				Debug.LogWarning("Play Mode老 锭 角青秦林技夸.");
				return;
			}

			(target as StageMapGenerator.StageMapGenerator).GenerateMap();
		}
	}
}
