using System;

namespace KnowledgeShare.Store.Core
{
    public interface ILocation
    {
        string Note { get; set; }
    }

    public struct OnlineLocation : ILocation
    {
        public string Url { get; set; }
        public string Note { get; set; }
    }

    public struct OnsiteLocation : ILocation
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Note { get; set; }
    }
}
