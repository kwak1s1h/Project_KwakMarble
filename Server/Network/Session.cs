using System.Net.Sockets;
using System.Net;

namespace Network
{
    public abstract class Session
    {
        private Socket _socket;

        private int _disconnected = 0;

        private RecvBuffer _recvBuffer = new RecvBuffer(1024);

        private SocketAsyncEventArgs _sendArgs = new SocketAsyncEventArgs();
        private SocketAsyncEventArgs _recvArgs = new SocketAsyncEventArgs();

        private Queue<ArraySegment<byte>> _sendQueue = new Queue<ArraySegment<byte>>();
        private List<ArraySegment<byte>> _pendinglist = new List<ArraySegment<byte>>();

        private object _lock = new object();

        public abstract void OnConnected(EndPoint endPoint);
        public abstract void OnDisconnected(EndPoint endPoint);
        public abstract void OnSend(int numOfBytes);
        public abstract int OnRecv(ArraySegment<byte> buffer);


        public void Start(Socket socket)
        {
            _socket = socket;
            _recvArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnRecvCompleted); // (2-2) 낚시대 들기
            _sendArgs.Completed += new EventHandler<SocketAsyncEventArgs>(OnSendCompleted);
            // RegisterSend();

            RegisterRecv(); // (1) 수신대기
        }
        #region 데이터 수신
        // 1. 연결대기
        void RegisterRecv()
        {
            _recvBuffer.Clean(); // 커서 뒤로 이동 방지
            ArraySegment<byte> segment = _recvBuffer.WriteSegment;
            _recvArgs.SetBuffer(segment.Array, segment.Offset, segment.Count);

            bool pending = _socket.ReceiveAsync(_recvArgs);
            if (pending == false)
                OnRecvCompleted(null, _recvArgs);    // (2-1) 낚시대 들어올리기(데이터수신 발생)
        }
        // 2. 데이터수신
        void OnRecvCompleted(object sender, SocketAsyncEventArgs args)
        {   // 조건1 : 내가 몇바이트를 받았는가? (연결이 끊길경우 0바이트 수신)
            // 조건2 : 연결에 특별한 문제 없는지 체크
            if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
            {
                // TODO
                try
                {
                    // Write 커서 이동
                    if (_recvBuffer.OnWrite(args.BytesTransferred) == false)
                    {
                        Disconnect();
                        return;
                    }

                    // 컨텐츠 쪽으로 데이터를 넘겨주고 얼마나 처리했는지 받는다
                    int processLen = OnRecv(_recvBuffer.ReadSegment);
                    if (processLen < 0 || _recvBuffer.DataSize < processLen)
                    {
                        Disconnect();
                        return;
                    }
                    // Read 커서 이동
                    if (_recvBuffer.OnRead(processLen) == false)
                    {
                        Disconnect();
                        return;
                    }

                    RegisterRecv(); // (3) 낚시대 다시 던지기(이벤트 재호출)
                }
                catch (Exception e)
                {
                    Console.WriteLine($"OnReceiveCompleted Failed! {e}");
                }
            }
            else
            {
                // Disconnect
            }
        }
        #endregion

        #region 데이터송신
        public void Send(ArraySegment<byte> sendBuff)
        {
            lock (_lock)
            {
                _sendQueue.Enqueue(sendBuff);
                if (_pendinglist.Count == 0)    // 대기중인 것이 없을때
                    RegisterSend();
            }

        }

        void RegisterSend()
        {
            while (_sendQueue.Count > 0) // SendQueue가 빌때까지 반복
            {
                ArraySegment<byte> buff = _sendQueue.Dequeue();
                _pendinglist.Add(buff);
            }
            _sendArgs.BufferList = _pendinglist;

            bool pending = _socket.SendAsync(_sendArgs);
            if (pending == false)    // 바로 보낼 수 있을 때
            {
                OnSendCompleted(null, _sendArgs);   // 이벤트 버퍼 초기화
            }
        }

        void OnSendCompleted(object sender, SocketAsyncEventArgs args)
        {
            lock (_lock)
            {
                if (args.BytesTransferred > 0 && args.SocketError == SocketError.Success)
                {
                    try
                    {
                        _sendArgs.BufferList = null;    // 이벤트의 버퍼리스트 깔끔히 정리
                        _pendinglist.Clear();           // 정리

                        OnSend(_sendArgs.BytesTransferred);
                        //TODO
                        if (_sendQueue.Count > 0)   // 혹시 아직 SendQueue에 값이 있다면 전송
                        {
                            RegisterSend();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"OnReceiveCompleted Failed! {ex}");
                    }

                }
                else
                {
                    Disconnect();
                }
            }

        }



        public void Disconnect()
        {
            if (Interlocked.Exchange(ref _disconnected, 1) == 1)
            {
                return;
            }
            OnDisconnected(_socket.RemoteEndPoint);
            // 연결종료
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Close();
        }
        #endregion
    }
}
