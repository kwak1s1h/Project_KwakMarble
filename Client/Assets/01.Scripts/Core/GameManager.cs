using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private Board board;

    public Dictionary<int, Charater> charaterDic = new();

    private void Awake()
    {
        board = FindObjectOfType<Board>();
    }
    public void GameStart()
    {
        StartCoroutine(InitCor());
    }
    private IEnumerator InitCor()
    {
        yield return board.InitBoard();
        for (int i = 0; i < 1; i++)
        {
            charaterDic.Add(1, board.SpawnCharater());
            yield return new WaitForSeconds(0.2f);
        }

    }
}
