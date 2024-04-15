namespace Utill
{
    class Program
    {
        private const string ROOT_DIR = "D:\\Server\\Project_KwakMarble\\";

        private const string CLIENT_PACKET_DIR = ROOT_DIR + "Server\\Network\\Packet\\Client";
        private const string SERVER_PACKET_DIR = ROOT_DIR + "Server\\Network\\Packet\\Server";

        private const string SERVER_OUT_DIR = ROOT_DIR + "Server\\Server\\PacketManager.cs";
        private const string CLIENT_OUT_DIR = ROOT_DIR + "Client\\Assets\\01.Scripts\\Network\\Packet\\PacketManager.cs";

        private const string TEMPLATE = @"using Network;
using System;
using System.Collections.Generic;
using Network.Packet;
using Network.Packet.Client;
using Network.Packet.Server;

namespace Server
{
    public class PacketManager
    {
        #region 싱글톤
        // 패킷매니저는 수정할 일없으므로 싱글톤으로 간편히 유지
        static PacketManager _instance = new PacketManager();
        public static PacketManager Instance { get { return _instance; } }
        #endregion

        PacketManager()
        {
            Register();
        }


        Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>> _makeFunc
            = new Dictionary<ushort, Func<PacketSession, ArraySegment<byte>, IPacket>>();
        Dictionary<ushort, Action<PacketSession, IPacket>> _handler
            = new Dictionary<ushort, Action<PacketSession, IPacket>>();

        public void Register()
        {
            REGISTER_TEXT
        }

        public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer, Action<PacketSession, IPacket> onRecvCallback = null)
        {
            ushort count = 0;

            ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
            count += 2;
            ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
            count += 2;

            Func<PacketSession, ArraySegment<byte>, IPacket> func = null;
            if (_makeFunc.TryGetValue(id, out func))
            {
                IPacket packet = func.Invoke(session, buffer);
                if (onRecvCallback != null)
                    onRecvCallback.Invoke(session, packet);
                else
                    HandlePacket(session, packet);
            }
        }

        T MakePacket<T>(PacketSession session, ArraySegment<byte> buffer) where T : IPacket, new()
        {
            T pkt = new T();
            pkt.Read(buffer);
            return pkt;
        }

        public void HandlePacket(PacketSession session, IPacket packet)
        {
            Action<PacketSession, IPacket> action = null;
            if (_handler.TryGetValue(packet.Protocol, out action))
                action.Invoke(session, packet);
        }
    }
}
";

        private const string REGISTER_TEMPLATE = @"_makeFunc.Add((ushort)PacketID.PACKET_NAME, MakePacket<PACKET_NAME>);
            _handler.Add((ushort)PacketID.PACKET_NAME, PacketHandler.PACKET_NAMEHandler);

            ";

        public enum GenerateState
        {
            CLIENT,
            SERVER,
        }

        static void Main(string[] args)
        {
            GeneratePacketManager(CLIENT_PACKET_DIR, SERVER_OUT_DIR);
            Console.WriteLine();
            GeneratePacketManager(SERVER_PACKET_DIR, CLIENT_OUT_DIR);
        }

        private static void GeneratePacketManager(string packetDir, string outDir)
        {
            Console.WriteLine($"[Debug] Start Generate PacketManager.cs");
            Console.WriteLine($"[Debug] PacketDir: {packetDir}");
            Console.WriteLine($"[Debug] OutDir:    {outDir}");

            DirectoryInfo clientPacketDir = new DirectoryInfo(packetDir);
            string[] fileNames = Directory.GetFiles(packetDir, "*.cs");

            string register = string.Empty;
            for (int i = 0; i < fileNames.Length; i++)
            {
                string fileName = fileNames[i];
                if (File.Exists(fileName))
                {
                    string className = Path.GetFileNameWithoutExtension(fileName);
                    register += REGISTER_TEMPLATE.Replace("PACKET_NAME", className);
                }
            }

            string result = TEMPLATE.Replace("REGISTER_TEXT", register.TrimEnd());

            if (File.Exists(outDir))
            {
                File.WriteAllText(outDir, result);
            }
            else
            {
                Console.WriteLine($"[Error] File {outDir} Dosen't Exits!");
            }

            Console.WriteLine($"[Debug] Generate PacketManager.cs Completed. File: {outDir}");
        }
    }
}