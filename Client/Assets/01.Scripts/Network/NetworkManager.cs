using DummyClient;
using DummyClient.Session;
using Server.Packet.Client;
using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEditor;
using UnityEngine;
using static System.Collections.Specialized.BitVector32;

public class NetworkManager : MonoBehaviour
{   // DummyClient 역할

    private static NetworkManager _instance;
    public static NetworkManager Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<NetworkManager>();
            }
            if(_instance == null)
            {
                Debug.LogError("NetworkManager not exist!");
            }
            return _instance;
        }
    }

    // 서버 플래그.	
    public bool m_isServer = false;
    // 접속 플래그.
    private bool m_isConnected = false;

    // 이벤트 알림 델리게이트.
    public delegate void EventHandler(NetEventState state);
    private EventHandler m_handler;

    // 이벤트 함수 등록.
    public void RegisterEventHandler(EventHandler handler)
    {
        m_handler += handler;
    }

    // 이벤트 통지 함수 삭제.
    public void UnregisterEventHandler(EventHandler handler)
    {
        m_handler -= handler;
    }

    // 세션 1개만 사용 예정이므로, SessionManager 미사용
    ServerSession _session = new ServerSession();


    // 컨텐츠단 데이터 전송!
    public void Send(ArraySegment<byte> sendBuff)
    {
        _session.Send(sendBuff);
        Debug.Log("서버로 송신!");
    }

    public bool ConnectToServer(IPAddress ipAddr, int port)
    {
        try
        {
            Debug.Log("연결 시작.");
            IPEndPoint endPoint = new IPEndPoint(ipAddr, port); // IP주소, 포트번호 입력
            Connector connector = new Connector();

            connector.Connect(endPoint, () => { return _session; }, 1);
            m_isConnected = true;
        }
        catch (Exception e)
        {
            Debug.Log($"Connect fail. {e.ToString()}");
            return false;
        }

        if (m_handler != null)
        {
            // 접속 결과를 알립니다.
            NetEventState state = new NetEventState();
            state.type = NetEventType.Connect;
            state.result = (m_isConnected == true) ? NetEventResult.Success : NetEventResult.Failure;
            m_handler(state);
            Debug.Log("event handler called");
        }
        return true;
    }


    // 서버인지 확인.
    public bool IsServer()
    {
        return m_isServer;
    }

    // 접속 확인.
    public bool IsConnected()
    {
        return m_isConnected;
    }

    private void Start()
    {
        IPHostEntry iphost = Dns.GetHostEntry("127.0.0.1");
        Debug.Log(iphost.AddressList[0]);
        ConnectToServer(iphost.AddressList[0], 8080);
    }

    void Update()
    {
        List<IPacket> list = PacketQueue.Instance.PopAll();
        foreach(IPacket packet in list)
            PacketManager.Instance.HandlePacket(_session, packet);
    }

    private void OnDestroy()
    {
        
    }
}
