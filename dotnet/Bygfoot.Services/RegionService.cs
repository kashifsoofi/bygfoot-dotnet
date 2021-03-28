namespace Bygfoot.Services
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bygfoot.Models;

    public class RegionService : IRegionService
    {
        public List<Region> GetAllRegions()
        {
            string[] countryFiles = Directory.GetFiles("./definitions", "country_*.json", SearchOption.AllDirectories);
            var rootPath = Path.GetDirectoryName("./definitions");

            var regions = new List<Region>();
            foreach (var countryFile in countryFiles)
            {
                var normalisedPath = countryFile.Replace(rootPath, "");
                var parts = normalisedPath.Split(Path.DirectorySeparatorChar, StringSplitOptions.None).TakeLast(3).ToArray();

                var parentRegion = regions.FirstOrDefault(r => r.Name == parts[0]);
                if (parentRegion == null)
                {
                    parentRegion = new Region
                    {
                        Name = parts[0],
                    };
                    regions.Add(parentRegion);
                }

                var subRegion = new Region
                {
                    Name = parts[1],
                };
                parentRegion.SubRegions.Add(subRegion);
            }

            return regions;
        }
    }
}
