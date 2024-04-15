using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoSingleton<Board>
{
    [SerializeField]private List<Place> places = new List<Place>();

    public Vector3 GetPlacePos(int idx)
    {
        return places[idx].placeTrm.position;
    }
    public void WavePlace(int idx)
    {
        places[idx].WavePlace();
    }
    public int BoardSize => places.Count;
}
