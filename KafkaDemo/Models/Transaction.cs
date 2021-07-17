using System;
using System.Collections.Generic;
using System.Text;

namespace KafkaDemo.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, object> AddtionalData { get; set; }
    }
}
