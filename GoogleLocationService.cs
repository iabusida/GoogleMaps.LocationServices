﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace GoogleMaps.Geolocation
{
    public class GoogleLocationService : ILocationService
    {
        /// <summary>
        /// Translates a Latitude / Longitude into a Region (state) using Google Maps api
        /// </summary>
        /// <param name="Latitude"></param>
        /// <param name="Longitude"></param>
        /// <returns></returns>
        public Region GetRegionFromLatLong(double Latitude, double Longitude)
        {
            string urlFormat = "http://maps.googleapis.com/maps/api/geocode/xml?latlng={0},{1}&sensor=false";
            XDocument doc = XDocument.Load(string.Format(urlFormat, Latitude, Longitude));

            var els = doc.Descendants("result").First().Descendants("address_component").Where(s => s.Descendants("short_name").First().Value.Length == 2).FirstOrDefault();
            if (null != els)
            {
                return new Region() { Name = els.Descendants("long_name").First().Value, ShortCode = els.Descendants("short_name").First().Value };
            }
            return null;
        }

        public MapPoint GetLatLongFromAddress(string Address)
        {

            string urlFormat = "http://maps.googleapis.com/maps/api/geocode/xml?address={0}&sensor=false";
            XDocument doc = XDocument.Load(string.Format(urlFormat, Address));
            var els = doc.Descendants("result").Descendants("geometry").Descendants("location").First();
            if (null != els)
            {
                return new MapPoint() { Latitude = Double.Parse((els.Nodes().First() as XElement).Value), Longitude = Double.Parse((els.Nodes().ElementAt(1) as XElement).Value) };
            }
            return null;
        }
    }
}
