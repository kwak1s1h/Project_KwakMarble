namespace Network
{
    public abstract class PacketSession : Session
    {
        public static readonly int HeaderSize = 2;

        // 상속클래스에서 변경 못하도록 씰(봉인)
        // 패킷구조 : [size(2)][packetId(2)][내용......] 
        public sealed override int OnRecv(ArraySegment<byte> buffer)
        {
            int processLen = 0;

            while (true)
            {
                // 최소한 헤더(사이즈)는 받을 수 있는지 확인
                if (buffer.Count < HeaderSize)
                    break;
                // 헤더(size(2))를 읽고, 내용이 몇바이트 짜리 패킷인지 확인 
                ushort dataSize = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
                if (buffer.Count < dataSize)    // 부분적 도착
                    break;
                // 패킷 조립, OnRecvPacket에 단위 패킷만큼 전달 (매개변수 해석 : 버퍼, 시작점, 끝점)
                OnRecvPacket(new ArraySegment<byte>(buffer.Array, buffer.Offset, dataSize));
                // 조립 완료되면 패킷 읽기 커서 이동
                processLen += dataSize;
                // 여기까지 오면 패킷 1개, 읽기 성공한 것

                // 현재까지 처리한 패킷의 데이터를 버퍼에서 제거하고 남은 데이터를 새로운 버퍼에 할당
                buffer = new ArraySegment<byte>(
                    buffer.Array, buffer.Offset + dataSize, buffer.Count - dataSize);
            }
            // 현재까지 처리한 누적 데이터 크기 반환
            return processLen;
        }
        // 상속클래스에서 OnRecv를 이것으로 대체하여 사용하도록 유도
        public abstract void OnRecvPacket(ArraySegment<byte> buffer);
    }
}
