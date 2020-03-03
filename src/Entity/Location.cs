using System;

namespace KnowledgeShare.Entity
{
    public interface ILocation : IEquatable<ILocation>
    {
        string Note { get; set; }
    }

    public struct OnlineLocation : ILocation, IEquatable<OnlineLocation>
    {
        public string Url { get; set; }
        public string Note { get; set; }

        public override bool Equals(object obj)
        {
            return obj is OnlineLocation onsite && Equals(onsite);
        }

        public bool Equals(ILocation other)
        {
            return other is OnlineLocation online && Equals(online);
        }

        public bool Equals(OnlineLocation other)
        {
            return Url == other.Url;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return Url.GetHashCode();
            }
        }
    }

    public struct OnsiteLocation : ILocation, IEquatable<OnsiteLocation>
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Note { get; set; }

        public override bool Equals(object obj)
        {
            return obj is OnsiteLocation onsite && Equals(onsite);
        }

        public bool Equals(ILocation other)
        {
            return other is OnsiteLocation onsite && Equals(onsite);
        }

        public bool Equals(OnsiteLocation other)
        {
            return Latitude == other.Latitude && Longitude == other.Longitude;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = -883231652;
                hashCode = hashCode * -1521134295 + Latitude.GetHashCode();
                hashCode = hashCode * -1521134295 + Longitude.GetHashCode();
                return hashCode;
            }
        }
    }
}