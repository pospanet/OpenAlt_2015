using System;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace RedisClient
{
    class Program
    {
        private const string RedisConnectionString =
            "<host_name>.redis.cache.windows.net,abortConnect=false,ssl=true,password=<access_key>";

        static void Main(string[] args)
        {
            IDatabase cache = Connection.GetDatabase();

            cache.StringSet("key1", "value");
            cache.StringSet("key2", 25);

            string key1 = cache.StringGet("key1");
            int key2 = (int) cache.StringGet("key2");

            cache.StringSet("key1", "value1");

            string value = cache.StringGet("key1");

            cache.StringSet("key1", "value1", TimeSpan.FromMinutes(90));

            cache.StringSet("e25", JsonConvert.SerializeObject(new Employee(25, "Clayton Gragg")));

            Employee e25 = JsonConvert.DeserializeObject<Employee>(cache.StringGet("e25"));
        }

        private static readonly Lazy<ConnectionMultiplexer> LazyConnection =
            new Lazy<ConnectionMultiplexer>(
                () => ConnectionMultiplexer.Connect(
                    RedisConnectionString));

        public static ConnectionMultiplexer Connection
        {
            get { return LazyConnection.Value; }
        }
    }

    [Serializable]
    class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Employee(int employeeId, string name)
        {
            Id = employeeId;
            Name = name;
        }
    }
}