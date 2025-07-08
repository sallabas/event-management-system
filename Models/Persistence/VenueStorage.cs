using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using MAS_Project.Models.Base;

namespace MAS_Project.Models.Persistence
{
    public static class VenueStorage
    {
        public static void SaveExtent(string filePath)
        {
#pragma warning disable SYSLIB0011
            using FileStream fs = new(filePath, FileMode.Create);
            new BinaryFormatter().Serialize(fs, Venue.GetExtent());
#pragma warning restore SYSLIB0011
        }

        public static IReadOnlyList<Venue> LoadExtent(string filePath)
        {
#pragma warning disable SYSLIB0011
            using FileStream fs = new(filePath, FileMode.Open);
            return (IReadOnlyList<Venue>)new BinaryFormatter().Deserialize(fs);
#pragma warning restore SYSLIB0011
        }
    }
}