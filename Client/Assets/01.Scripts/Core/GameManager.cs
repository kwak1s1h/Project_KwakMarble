using Server.Packet.Client;
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
    public void GameStart(List<int> users)
    {
        StartCoroutine(InitCor(users));
    }
    private IEnumerator InitCor(List<int> users)
    {
        yield return board.InitBoard();
        for (int i = 0; i < users.Count; i++)
        {
            charaterDic.Add(users[i], board.SpawnCharater());
            yield return new WaitForSeconds(0.2f);
        }

        C_GameLoaded loaded = new C_GameLoaded();
        NetworkManager.Instance.Send(loaded.Write());
    }
}
