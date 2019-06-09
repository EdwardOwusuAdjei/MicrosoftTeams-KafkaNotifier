namespace AutoLagNotifier.Setting
{
    public class Config
    {
        public int Delay { get;set; }
    }
    public class BurrowEndpoints
    {
        public string  ListConsumers { get; set; }
        public string  LagStatus { get; set; }
        
    }

    public class Lag
    {
        public int MaxLagAlert { get; set; }
    }

    public class FilterName
    {
        public string IgnoreCaseFilterNames { get; set; }
    }
}