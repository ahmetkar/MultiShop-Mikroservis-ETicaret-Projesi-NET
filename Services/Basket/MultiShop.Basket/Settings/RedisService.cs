﻿using StackExchange.Redis;

namespace MultiShop.Basket.Settings
{
    public class RedisService
    {
        public RedisService(string host, int port)
        {
            _host = host;
            _port = port;
        }

        private string _host { get; set; }
        private int _port { get; set; }

        private ConnectionMultiplexer _connectionMultiplexer;

        public void Connect()=> _connectionMultiplexer = ConnectionMultiplexer.Connect($"{_host}:{_port}");

        public IDatabase GetDb(int db = 1) => _connectionMultiplexer.GetDatabase(db-1); 
    }
}
