namespace CovidTelegramBot
{
    public class CovidInfo
    {
        public int AllTime { get; set; }
        public int Today { get; set; }
        public int Recovered { get; set; }
        public int Dead { get; set; }

        public override string ToString()
        {
            return $"За все время заражено: {AllTime} \n" +
                   $"Сегодня: {Today} \n" +
                   $"Вылечилось: {Recovered} \n" +
                   $"Погибло: {Dead}";
        }
    }
}