using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoSingleton<Board>
{
    [SerializeField]private List<Place> places = new List<Place>();

    public void MoveCharator(int[] diceResult, int uuid, int mapIdx)
    {

    }
}
