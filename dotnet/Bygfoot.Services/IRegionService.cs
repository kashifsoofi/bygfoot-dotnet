namespace Bygfoot.Services
{
    using System.Collections.Generic;
    using Bygfoot.Models;

    public interface IRegionService
    {
        List<Region> GetAllRegions();
    }
}