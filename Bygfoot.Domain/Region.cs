namespace Bygfoot.Domain;

public class Region
{
    public string Name { get; set; }
    public string Symbol { get; set; }
    public string SId { get; set; }
    public int Rating { get; set; }

    public List<Region> SubRegions { get; set; } = new List<Region>();
    public List<League> Leagues { get; set; } = new List<League>();
    public List<Cup> Cups { get; set; } = new List<Cup>();
}