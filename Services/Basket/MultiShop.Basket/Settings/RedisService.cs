using StackExchange.Redis;

namespace MultiShop.Basket.Settings
{
    public class RedisService
    {
        private readonly string _host;
        private readonly int _port;
        private ConnectionMultiplexer? _connectionMultiplexer;
        private readonly object _lock = new();

        public RedisService(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public void Connect()
        {
            if (_connectionMultiplexer != null && _connectionMultiplexer.IsConnected)
                return;

            lock (_lock)
            {
                if (_connectionMultiplexer != null && _connectionMultiplexer.IsConnected)
                    return;

                var configurationOptions = new ConfigurationOptions
                {
                    EndPoints = { { _host, _port } },
                    AbortOnConnectFail = false,
                    ConnectRetry = 5,
                    ConnectTimeout = 5000,
                    SyncTimeout = 5000,
                    KeepAlive = 60
                };

                try
                {
                    _connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
                }
                catch
                {
                    _connectionMultiplexer = ConnectionMultiplexer.Connect(configurationOptions);
                }
            }
        }

        public IDatabase GetDb(int db = 1)
        {
            if (_connectionMultiplexer == null || !_connectionMultiplexer.IsConnected)
            {
                Connect();
            }
            return _connectionMultiplexer!.GetDatabase(db - 1);
        }
    }
}
