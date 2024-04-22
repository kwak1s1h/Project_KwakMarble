using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : UIBase
{
    [SerializeField] private PlayerProfile profilePrefabs;
    private Dictionary<int, PlayerProfile> uuidByProfiles;

    [SerializeField] private Transform userProfileTrm;
    [SerializeField] private Button startBtn;

    public void OtherEnterRoom(int uuid)
    {
        PlayerProfile profile = Instantiate(profilePrefabs,userProfileTrm);
        uuidByProfiles.Add(uuid, profile);
    }
    public void GameStart()
    {
        UIManager.Instance.TurnOffAllUI();
        GameManager.Instance.GameStart();
    }
    public void ChangeReady(int uuid, bool isReady)
    {
        uuidByProfiles[uuid].SetReady(isReady);
    }
    public void ReadyBtn()
    {
        //레디했다는 걸 서버한테 알림
    }

    public override void TurnOff()
    {
    }

    public override void TurnOn()
    {
    }
}
