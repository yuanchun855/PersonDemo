using System;
using System.Net;
using System.Net.Sockets;

namespace HotUpdate.GameFrameWork.Net
{
    public interface INetWorkChannel
    {
        string Name { get; }
        
        Socket Socket { get; }
        
        bool Connected { get; }
        
        AddressFamily AddressFamily { get; }
        ServiceType ServiceType
        {
            get;
        }
        
        int SendPacketCount { get; }
        
        int ReceivePacketCount { get; }
        
        int ReceivedPacketCount { get; }
        
        bool ResetHeartBeatElapseSecondsWhenReceivePacket
        {
            get;
            set;
        }
        
        int MissHeartBeatCount
        {
            get;
        }
        
        float HeartBeatInterval
        {
            get;
            set;
        }
        
        float HeartBeatElapseSeconds
        {
            get;
        }
        /// <summary>
        /// 注册网络消息包处理函数。
        /// </summary>
        /// <param name="handler">要注册的网络消息包处理函数。</param>
        void RegisterHandler(IPacketHandler handler);

        /// <summary>
        /// 设置默认事件处理函数。
        /// </summary>
        /// <param name="handler">要设置的默认事件处理函数。</param>
        void SetDefaultHandler(EventHandler<Packet> handler);

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        void Connect(IPAddress ipAddress, int port);

        /// <summary>
        /// 连接到远程主机。
        /// </summary>
        /// <param name="ipAddress">远程主机的 IP 地址。</param>
        /// <param name="port">远程主机的端口号。</param>
        /// <param name="userData">用户自定义数据。</param>
        void Connect(IPAddress ipAddress, int port, object userData);

        /// <summary>
        /// 关闭网络频道。
        /// </summary>
        void Close();

        /// <summary>
        /// 向远程主机发送消息包。
        /// </summary>
        /// <typeparam name="T">消息包类型。</typeparam>
        /// <param name="packet">要发送的消息包。</param>
        void Send<T>(T packet) where T : Packet;
        
        
        
    }
}