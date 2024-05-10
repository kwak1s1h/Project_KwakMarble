using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Charater : MonoBehaviour
{
    public event Action OnMoveEndComplete;

    public TextMeshPro tmp;

    private int _mapIdx;
    public int MapIdx => _mapIdx;
    private void Start()
    {
        _mapIdx = 0;
        transform.position = Board.Instance.GetPlacePos(_mapIdx);

        OnMoveEndComplete += () =>
        {
            UIManager.Instance.GetUI<CreateBuildingUI>();
        };
    }
    public void Init(int uuid)
    {
        tmp.text = uuid.ToString();
    }

    public void Move(int dest)
    {
        Sequence seq = DOTween.Sequence();
        while(_mapIdx != dest)
        {
            _mapIdx = _mapIdx + 1;
            _mapIdx %= Board.Instance.BoardSize;
            int idx = _mapIdx;
            seq.Append(
            transform.DOJump(Board.Instance.GetPlacePos(idx), 1f, 1, 0.1f).OnComplete(() =>
              {
                  //value--;
                  //ContinueJump(value);
                  Board.Instance.WavePlace(idx);
              }));
        }
        seq.OnComplete(() => OnMoveEndComplete?.Invoke());
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
