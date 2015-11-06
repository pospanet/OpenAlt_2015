using System;

namespace DocumentDBClient
{
    public class DataClass
    {
        public string Name { get; set; }
        public Guid UniqueId { get; set; }
        public DateTime Date { get; set; }
        public int ObjectKey { get; set; }
    }
}
