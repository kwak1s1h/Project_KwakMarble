using Server.Packet.Client;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomUI : UIBase
{
    [SerializeField] private PlayerProfile profilePrefabs;
    private Dictionary<int, PlayerProfile> uuidByProfiles;

    [SerializeField] private Transform userProfileTrm;
    [SerializeField] private Button startBtn;
    [SerializeField] private TextMeshProUGUI roomUuid;

    private bool _myReady = false;

    public void Init(int myId)
    {
        uuidByProfiles = new Dictionary<int, PlayerProfile>();
        _myReady = false;
        PlayerProfile myProfile = Instantiate(profilePrefabs, userProfileTrm);
        myProfile.SetId(myId, true);
        uuidByProfiles.Add(myId, myProfile);
    }

    public void OtherEnterRoom(int uuid)
    {
        PlayerProfile profile = Instantiate(profilePrefabs,userProfileTrm);
        profile.SetId(uuid);
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
        _myReady = !_myReady;

        //레디했다는 걸 서버한테 알림
        C_Ready ready = new C_Ready();
        ready.value = _myReady;
        NetworkManager.Instance.Send(ready.Write());
    }

    public void SetRoomUuid(string uuid)
    {
        roomUuid.SetText(uuid);
    }

    public void CopyRoomUuid()
    {
        GUIUtility.systemCopyBuffer = roomUuid.text;
    }

    public override void TurnOff()
    {
    }

    public override void TurnOn()
    {
    }
}
