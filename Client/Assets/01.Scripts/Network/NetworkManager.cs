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
{   // DummyClient ����

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

    // ���� �÷���.	
    public bool m_isServer = false;
    // ���� �÷���.
    private bool m_isConnected = false;

    // �̺�Ʈ �˸� ��������Ʈ.
    public delegate void EventHandler(NetEventState state);
    private EventHandler m_handler;

    // �̺�Ʈ �Լ� ���.
    public void RegisterEventHandler(EventHandler handler)
    {
        m_handler += handler;
    }

    // �̺�Ʈ ���� �Լ� ����.
    public void UnregisterEventHandler(EventHandler handler)
    {
        m_handler -= handler;
    }

    // ���� 1���� ��� �����̹Ƿ�, SessionManager �̻��
    ServerSession _session = new ServerSession();


    // �������� ������ ����!
    public void Send(ArraySegment<byte> sendBuff)
    {
        _session.Send(sendBuff);
        Debug.Log("������ �۽�!");
    }

    public bool ConnectToServer(IPAddress ipAddr, int port)
    {
        try
        {
            Debug.Log("���� ����.");
            IPEndPoint endPoint = new IPEndPoint(ipAddr, port); // IP�ּ�, ��Ʈ��ȣ �Է�
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
            // ���� ����� �˸��ϴ�.
            NetEventState state = new NetEventState();
            state.type = NetEventType.Connect;
            state.result = (m_isConnected == true) ? NetEventResult.Success : NetEventResult.Failure;
            m_handler(state);
            Debug.Log("event handler called");
        }
        return true;
    }


    // �������� Ȯ��.
    public bool IsServer()
    {
        return m_isServer;
    }

    // ���� Ȯ��.
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
