using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class Board : MonoSingleton<Board>
{
    [SerializeField] private List<Place> places = new List<Place>();
    [SerializeField] private Transform floor;

    [SerializeField] private Charater playerPref;

    [SerializeField] Animator[] dices = new Animator[2];


    public IEnumerator InitBoard()
    {
        WaitForSeconds w = new WaitForSeconds(0.1f);
        foreach (var p in places)
        {
            p.transform.DOMoveY(0, 0.3f);
            yield return w;
        }
        yield return new WaitForSeconds(0.5f);
        floor.DOMoveY(0, 0.5f);
    }

    public void RollDice(int[] result)
    {
        for (int i = 0; i < result.Length; i++)
        {
            dices[i].gameObject.SetActive(true);
            dices[i].SetInteger("Value", result[i]);
        }
        StartCoroutine(FalseDice());
    }
    private IEnumerator FalseDice()
    {
        yield return new WaitForSeconds(3f);
        for (int i = 0; i < dices.Length; i++)
        {
            dices[i].gameObject.SetActive(false);
            dices[i].SetInteger("Value", 0);
        }
    }

    public Charater SpawnCharater()
    {
        Vector3 spawnPos = places[0].placeTrm.position;
        spawnPos.y += 50;
        Charater p = Instantiate(playerPref,spawnPos,Quaternion.identity);
        p.transform.DOMoveY(places[0].placeTrm.position.y, 0.5f);
        return p;
    }
    public Vector3 GetPlacePos(int idx)
    {
        return places[idx].placeTrm.position;
    }
    public void WavePlace(int idx)
    {
        places[idx].WavePlace();
    }
    public int BoardSize => places.Count;

    public void CreateBuilding(int idx, BuildingType type)
    {
        places[idx].CreateBuilding(type);
    }
}
