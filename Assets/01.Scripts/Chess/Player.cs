using Crogen.PowerfulInput;
using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class Player : Piece, IPointerClickHandler
{
    [SerializeField]
    private InputReader _inputReader;

    private bool _isSelected;

    private void Awake()
    {
        _inputReader.OnMouseMoveEvent += HandleOnMouseMoveEvent;
        _inputReader.OnLeftMouseClickEvent += HandleOnLeftMouseClickEvent;
    }

    private void HandleOnLeftMouseClickEvent(bool isClicked)
    {
        if (isClicked && _isSelected)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(_inputReader.MousePosition);
            Vector2Int ceiled = ((Vector2)mousePos).ConvertToInt(ConvertType.Ceil);
            if (IsOnMyMovement(ceiled))
            {
                Vector2Int boardPos = _board.WorldPointToBoardPoint(ceiled);
                if (_board.CanMove(boardPos))
                {
                    List<Vector2Int> movementList = GetMovementList();
                    foreach (Vector2Int move in movementList)
                    {
                        _board.SetTileColor(move, Color.white);
                    }
                    transform.DOJump(_board.GetSquare(boardPos).position, 2f, 1, 0.5f);
                    _isSelected = false;
                    _position = boardPos;
                }
            }
        }
    }

    private void HandleOnMouseMoveEvent(Vector3 mousePos)
    {
        if (!_isSelected) return;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2Int ceiled = ((Vector2)mousePos).ConvertToInt(ConvertType.Ceil);
        List<Vector2Int> movementList = GetMovementList();
        foreach (Vector2Int move in movementList)
        {
            _board.SetTileColor(move, new Color(1, 0, 0, 0.5f));
        }
        if (IsOnMyMovement(ceiled))
        {
            Vector2Int boardPos = _board.WorldPointToBoardPoint(ceiled);
            if (_board.CanMove(boardPos))
                _board.SetTileColor(boardPos, new Color(1, 0, 0, 0.9f));
        }
    }

    private bool IsOnMyMovement(Vector2Int position)
    {
        List<Vector2Int> movementList = GetMovementList();
        Vector2Int boardPos = _board.WorldPointToBoardPoint(position);
        foreach (Vector2Int move in movementList)
        {
            if (move == boardPos)
                return true;
        }
        return false;
    }

    private void SelectMovement()
    {
        if (!_isSelected) return;

    }

    private void Move(Vector2Int position)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _isSelected = !_isSelected;

        List<Vector2Int> movementList = GetMovementList();

        if (_isSelected)
        {
            foreach (Vector2Int move in movementList)
            {
                _board.SetTileColor(move, new Color(1, 0, 0, 0.5f));
            }
        }
        else
        {
            foreach (Vector2Int move in movementList)
            {
                _board.SetTileColor(move, Color.white);
            }
        }

    }
}
