﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Ch.Tallichet.WindMobile.Model
{
    [DataContract]
    public class Station
    {
        [DataMember(Name="_id")]
        public string ID { get; set; }

        [DataMember(Name="status")]
        public string StatusString { get; set; }

        [DataMember(Name = "loc")]
        public Location Location { get; set; }

        [DataMember(Name="short")]
        public string ShortName { get; set; }

        [DataMember(Name="name")]
        public string DisplayName { get; set; }

        [DataMember(Name="tags")]
        public string[] Tags { get; set; }

        [DataMember(Name="prov")]
        public string Provider { get; set; }

        [DataMember(Name="cat")]
        public string Category { get; set; }

        [DataMember(Name = "seen")]
        public long LastSeen { get; set; }

        [DataMember(Name = "alt")]
        public int Altitude { get; set; }

        [DataMember(Name="timezone")]
        public string TimeZoneString { get; set; }

        [DataMember(Name="last")]
        public StationData Last { get; set; }

        [DataMember(Name="url")]
        public string Url { get; set; }

        [DataMember(Name="desc")]
        public string Description { get; set; }

        [IgnoreDataMember]
        public bool IsValid
        {
            get
            {
                return this.StatusString.Equals("green", StringComparison.OrdinalIgnoreCase) ||
                       this.StatusString.Equals("yellow", StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    [DataContract]
    public class Location
    {
        [DataMember(Name="lat")]
        public double Latitude { get; set; }

        [DataMember(Name="lon")]
        public double Longitude { get; set; }
    }

    [DataContract]
    public class StationData
    {
        [DataMember(Name="_id")]
        public string ID { get; set; }

        [IgnoreDataMember]
        public DateTime Date
        {
            get { return new DateTime(1970, 1, 1) + TimeSpan.FromSeconds(long.Parse(ID)); }
        }

        [DataMember(Name="w-dir")]
        public int WindDirection { get; set; }

        [DataMember(Name="temp")]
        public double? Temperature { get; set; }

        [DataMember(Name = "w-min")]
        public double? WindMin { get; set; }
        [DataMember(Name = "w-max")]
        public double? WindMax { get; set; }

        [DataMember(Name = "w-avg")]
        public double? WindAverage { get; set; }

        [DataMember(Name = "w-inst")]
        public double?[] WindInstant { get; set; }


        [DataMember(Name="hum")]
        public double? Humidity { get; set; }
        [DataMember(Name = "rain")]
        public double? Rain { get; set; }
        [DataMember(Name = "pres")]
        public double? Pression { get; set; }

    }

    [DataContract]
    public class TextSearchResult
    {
        [DataMember(Name="score")]
        public double Score { get; set; }
        [DataMember(Name="obj")]
        public Station Station { get; set; }
    }
}
