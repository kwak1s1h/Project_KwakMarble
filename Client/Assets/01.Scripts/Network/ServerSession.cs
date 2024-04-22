using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DummyClient.Session
{

    class ServerSession : PacketSession // 게임컨텐츠 영역
    {                           // 데이터의 송수신 구현보다, 송수신시의 동작 작성
        private int _sessionId;
        public int SessionId
        {
            get => _sessionId;
            set => _sessionId = value;
        }

        public override void OnConnected(EndPoint endPoint)
        {
            Debug.Log($"OnConnected bytes: {endPoint}");

        }
        public override void OnDisconnected(EndPoint endPoint)
        {
            Debug.Log($"OnDisConnected bytes: {endPoint}");
        }
        public override void OnRecvPacket(ArraySegment<byte> buffer)  // 리시브 이벤트 발생시 동작
        {
            PacketManager.Instance.OnRecvPacket(this, buffer, (s, p) => PacketQueue.Instance.Push(p));  // 패킷매니저 시작(해석기 가동)
            // (s, p) PacketSession, IPacket
        }
        public override void OnSend(int numOfBytes)             // 샌드 이벤트 발생시 동작
        {
            //Debug.Log($"Transferred bytes: {numOfBytes}");
        }
    }
}
