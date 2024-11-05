using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine.TextCore.Text;

public class ChessMoveAreaWindowEditor : EditorWindow
{
	private static string[] _output;
	private static int _rangeAmount = 3;

	private readonly float _btnScale = 50f;

	private static List<List<int>> _curSelectedPos;

	private readonly string _path = "/Resources/PieceMoveRanges";
	private static string _fileName = "New Piece";
	[MenuItem("Tools/ChessMoveAreaWindow")]
    private static void ShowWindow()
    {
        var window = GetWindow(typeof(ChessMoveAreaWindowEditor));
        window.position = new Rect(0, 0, 800, 600);
        window.titleContent = new GUIContent("ChessMoveAreaWindow");
        window.Show();

		_curSelectedPos = new List<List<int>>();

        for (int i = 0; i < _rangeAmount; i++)
			_curSelectedPos.Add(new List<int>(new int[_rangeAmount]));
	}

	private Vector2 _scrollVec;

	private int resetCount = 0;

	private void OnGUI()
	{
        float offset = 0.1f;
        float finalScale = (_btnScale * _rangeAmount) + (offset * _rangeAmount);

		//인스펙터
		GUILayout.BeginArea(new Rect(0, 0, 200, position.height), GUI.skin.window);
		{
			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("Grid 크기");

				Mathf.Clamp(3, EditorGUILayout.IntField(_rangeAmount), 64);
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(25);

			if(GUILayout.Button("초기화"))
			{
				if(EditorUtility.DisplayDialog("경고", "정말로 초기화하시겠습니까?", "Yes", "No"))
				{
					_fileName = "New Piece";
					_rangeAmount = 3;
					_curSelectedPos = new List<List<int>>();

					for (int i = 0; i < _rangeAmount; i++)
						_curSelectedPos.Add(new List<int>(new int[_rangeAmount]));

					RefreshGrid();
				}
			}

			GUILayout.Space(25);

			GUILayout.BeginHorizontal();
			{
				GUILayout.Label("파일 이름");

				_fileName = EditorGUILayout.DelayedTextField(_fileName);
			}
			GUILayout.EndHorizontal();
			GUILayout.Space(25);

			GUILayout.BeginHorizontal();
			{
				if (GUILayout.Button("Load!"))
				{
					OnLoad();
				}
			}
			GUILayout.EndHorizontal();

			GUILayout.Space(25);

			GUILayout.BeginHorizontal();
			{
				if(GUILayout.Button("Bake!"))
				{
					OnBake();
				}
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndArea();

		//View
		_scrollVec = GUI.BeginScrollView(new Rect(200, 0, position.width - 200, position.height), _scrollVec, new Rect(0, 0, finalScale, finalScale));
		{
			DrawGridView(offset);
			DrawAddGridBtn(finalScale);
			if (_rangeAmount > 3)
			{
				DrawRemoveGridBtn(finalScale);
			}
		}
		GUI.EndScrollView();
	}

	private void OnLoad()
	{
		string path = $"{Application.dataPath}{_path}/{_fileName}.txt";

		string allText = File.ReadAllText(path);

		string[] lines = allText.Split('\n');

		_rangeAmount = lines.Length-1;
		_curSelectedPos = new List<List<int>>();

		for (int i = 0; i < _rangeAmount; i++)
			_curSelectedPos.Add(new List<int>(new int[_rangeAmount]));

		for (int i = 0; i < _rangeAmount; i++)
		{
			for (int j = 0; j < _rangeAmount; j++)
			{
				_curSelectedPos[i][j] = lines[i][j] - 48;
			}
		}

		RefreshGrid();
	}
	private void OnBake()
	{
		string str = string.Empty;

		for (int i = 0; i < _curSelectedPos.Count; i++)
		{
			for (int j = 0; j < _curSelectedPos[0].Count; j++)
			{
				str += _curSelectedPos[i][j];
			}
			str += '\n';
		}

		string path = $"{Application.dataPath}{_path}";
		DirectoryInfo directoryInfo = new DirectoryInfo(path);
		if(directoryInfo.Exists == false)
			directoryInfo.Create();

		using (StreamWriter writer = File.CreateText($"{path}/{_fileName}.txt"))
		{
			writer.Write(str);
		}

		AssetDatabase.Refresh();
		AssetDatabase.SaveAssets();
	}

	private void DrawGridView(float offset)
	{
		Vector2 pos = Vector2.zero;

		for (int i = 0; i < _rangeAmount; i++)
		{
			for (int j = 0; j < _rangeAmount; j++)
			{
				if (i == _rangeAmount / 2 && j == _rangeAmount / 2)
				{
					_curSelectedPos[i][j] = 2;
				}

				if (_curSelectedPos[i][j] == 0)
				{
					GUI.color = Color.white;
				}
				else if (_curSelectedPos[i][j] % 2 == 1)
				{
					GUI.color = Color.cyan;
				}
				else if (_curSelectedPos[i][j] % 2 + 1 == 2)
				{
					GUI.color = Color.red;
				}

				if (_curSelectedPos[i][j] == 2)
					GUI.color = Color.red;
				if (GUI.Button(new Rect(pos.x, pos.y, _btnScale, _btnScale),
					_curSelectedPos[i][j].ToString(), GUI.skin.window))
				{
					if (Event.current.alt)
					{
						_curSelectedPos[i][j] = 0;
					}
					else
					{
						_curSelectedPos[i][j]++;
						_curSelectedPos[i][j] %= 10;
					}
				}
				pos.x += offset + _btnScale;

				GUI.color = Color.white;
			}
			pos.x = 0f;
			pos.y += offset + _btnScale;
		}

	}
	private void DrawAddGridBtn(float finalScale)
	{
		GUI.color = Color.green;
		//칸 늘리기
		if (GUI.Button(new Rect(finalScale + _btnScale / 2, 0, _btnScale / 2, finalScale), "+"))
		{
			AddGrid();
		}
		GUI.color = Color.white;
	}
	private void DrawRemoveGridBtn(float finalScale)
	{
		GUI.color = Color.red;
		//칸 줄이기
		if (GUI.Button(new Rect(0, finalScale + _btnScale / 2, finalScale, _btnScale / 2), "-"))
		{
			RemoveGrid();
		}
		GUI.color = Color.white;
	}

	private void AddGrid()
	{
		_curSelectedPos.Insert(0, new List<int>(new int[_rangeAmount]));
		_curSelectedPos.Add(new List<int>(new int[_rangeAmount]));

		_rangeAmount += 2;

		for (int i = 0; i < _rangeAmount; i++)
		{
			_curSelectedPos[i].Insert(0, 0);
			_curSelectedPos[i].Add(0);
		}


		RefreshGrid();
	}
	private void RemoveGrid()
	{
		_curSelectedPos.RemoveAt(0);
		_curSelectedPos.RemoveAt(_curSelectedPos.Count-1);

		_rangeAmount -= 2;

		for (int i = 0; i < _rangeAmount; i++)
		{
			_curSelectedPos[i].RemoveAt(0);
			_curSelectedPos[i].RemoveAt(_curSelectedPos[i].Count-1);
		}

		RefreshGrid();
	}

	private void RefreshGrid()
	{
		for (int i = 0; i < _rangeAmount; i++)
		{
			for (int j = 0; j < _rangeAmount; j++)
			{
				if (_curSelectedPos[i][j] == 2)
					_curSelectedPos[i][j] = 0;
			}
		}
	}
}
