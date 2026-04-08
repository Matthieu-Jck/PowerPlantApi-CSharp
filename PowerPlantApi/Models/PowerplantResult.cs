namespace PowerPlantApi.Models;

public class PowerplantResult
{
    public string Name { get; set; }
    public double P { get; set; }

    public PowerplantResult(string name, double p)
    {
        Name = name;
        P = p;
    }
}
