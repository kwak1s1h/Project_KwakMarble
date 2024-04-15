using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Charater : MonoBehaviour
{
    public event Action OnMoveEndComplete;

    private int _mapIdx;
    private void Start()
    {
        _mapIdx = 0;
        transform.position = Board.Instance.GetPlacePos(_mapIdx);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Move(10);
        }
    }
    public void Move(int diceValue)
    {
        int value = diceValue;
        _mapIdx = _mapIdx + 1;
        _mapIdx %= Board.Instance.BoardSize;
        transform.DOJump(Board.Instance.GetPlacePos(_mapIdx), 1f, 1, 0.1f).OnComplete(() =>
          {
              value--;
              if (value > 0)
                  ContinueJump(value);
              else
                  OnMoveEndComplete?.Invoke();

              Board.Instance.WavePlace(_mapIdx);
          });
    }
    private void ContinueJump(int value)
    {
        _mapIdx = _mapIdx + 1;
        _mapIdx %= Board.Instance.BoardSize;
        transform.DOJump(Board.Instance.GetPlacePos(_mapIdx), 1f, 1, 0.1f).OnComplete(() =>
        {
            value--;
            if (value > 0)
                ContinueJump(value);
            else
                OnMoveEndComplete?.Invoke();

            Board.Instance.WavePlace(_mapIdx);
        });
    }
}
