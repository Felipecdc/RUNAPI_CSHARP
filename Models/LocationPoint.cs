using System;

namespace User.Models
{
    public class LocationPoint
    {
        public int Id { get; set; }  
        public required string Name { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
