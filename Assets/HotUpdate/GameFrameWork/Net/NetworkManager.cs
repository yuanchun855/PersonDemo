using System;
using System.Collections.Generic;

namespace HotUpdate.GameFrameWork.Net
{
    internal sealed class NetworkManager: BaseManager<NetworkManager>,INetworkManager
    {
        // private readonly Dictionary<string, NetworkChannelBase> m_NetworkChannels;

        private EventHandler<NetworkConnectedEventArgs> m_NetworkConnectedEventHandler;
        private EventHandler<NetworkClosedEventArgs> m_NetworkClosedEventHandler;
        private EventHandler<NetworkMissHeartBeatEventArgs> m_NetworkMissHeartBeatEventHandler;
        private EventHandler<NetworkErrorEventArgs> m_NetworkErrorEventHandler;
        private EventHandler<NetworkCustomErrorEventArgs> m_NetworkCustomErrorEventHandler;
        public void Init()
        {
            
        }

        public int NetworkChannelCount { get; }
        public event EventHandler<NetworkConnectedEventArgs> NetworkConnected;
        public event EventHandler<NetworkClosedEventArgs> NetworkClosed;
        public event EventHandler<NetworkMissHeartBeatEventArgs> NetworkMissHeartBeat;
        public event EventHandler<NetworkErrorEventArgs> NetworkError;
        public event EventHandler<NetworkCustomErrorEventArgs> NetworkCustomError;
        public bool HasNetworkChannel(string name)
        {
            throw new NotImplementedException();
        }

        public INetWorkChannel GetNetworkChannel(string name)
        {
            throw new NotImplementedException();
        }

        public INetWorkChannel[] GetAllNetworkChannels()
        {
            throw new NotImplementedException();
        }

        public void GetAllNetworkChannels(List<INetWorkChannel> results)
        {
            throw new NotImplementedException();
        }

        public INetWorkChannel CreateNetworkChannel(string name, ServiceType serviceType, INetworkChannelHelper networkChannelHelper)
        {
            throw new NotImplementedException();
        }

        public bool DestroyNetworkChannel(string name)
        {
            throw new NotImplementedException();
        }
    }
}