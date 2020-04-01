namespace CovidTelegramBot
{
    public class CovidRegion
    {
        public string Name { get; set; }
        public int Total { get; set; }
        public int Today { get; set; }

        public override string ToString()
        {
            return $"{Name} : {Total} +{Today} \n";
        }
    }
}