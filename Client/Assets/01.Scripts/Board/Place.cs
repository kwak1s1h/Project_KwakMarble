using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Place : MonoBehaviour
{
    [SerializeField] private List<GameObject> building = new();
    [SerializeField] private List<Transform> buildingPos = new();

    public Transform placeTrm;
    private Sequence seq = null;
    private float yPos;
    public void WavePlace()
    {
        if (seq == null)
        {
            MkSeq();
            return;
        }

        if (seq.IsPlaying())
        {
            seq.Kill();
            seq = null;
            Vector3 pos = transform.position;
            pos.y = yPos;
            transform.position = pos;
        }
        MkSeq();
    }
    private void MkSeq()
    {
        seq = DOTween.Sequence();
        float yPos = transform.position.y;
        seq.Append(transform.DOMoveY(transform.position.y - 2, 0.1f));
        seq.Append(transform.DOMoveY(yPos, 0.15f)).SetEase(Ease.OutElastic);
        seq.OnComplete(() => seq = null);
    }
    public void CreateBuilding(BuildingType type)
    {
        StartCoroutine(BuildCor(type));
    }
    private IEnumerator BuildCor(BuildingType type)
    {
        for (int i = 0; i < 3; i++)
        {
            if (((int)type & (int)BuildingType.Monitor << i) > 0)
            {
                BuildingSeq(i);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }
    private void BuildingSeq(int idx)
    {
        building[idx].SetActive(true);
        Transform trm = building[idx].transform;
        trm.position = buildingPos[idx].position + Vector3.up * 5;
        trm.DOMove(buildingPos[idx].position, 0.3f).SetEase(Ease.OutBounce);
    }
}
