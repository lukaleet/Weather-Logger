using System;
using System.ComponentModel.DataAnnotations;

namespace esp8266Temp.Models
{
    public class Temperature
    {
        [Key]
        public int Id { get; set; }
        public DateTime MeasuredAt { get; set; }
        public string Address { get; set; }
        public float TemperatureValue { get; set; }
        public string SensorLocation { get; set; }
    }
}