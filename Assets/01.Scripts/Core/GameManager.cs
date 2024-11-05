using System;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [field:SerializeField] public ChessController ChessController {get; private set;}

    private void Awake()
    {
    }
}
