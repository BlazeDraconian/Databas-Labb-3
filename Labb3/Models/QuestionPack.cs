namespace Labb3.Models
{
    public enum Difficulty { Easy, Medium, Hard }
    public class QuestionPack
    {
        public QuestionPack()
        {
            Questions = new List<Question>();
        }
        public QuestionPack(string name, Difficulty difficulty = Difficulty.Medium, int timeLimitInSeconds = 30, string category = "")
        {
            Name = name;
            Difficulty = difficulty;
            TimeLimitInSeconds = timeLimitInSeconds;
            Questions = new List<Question>();
            Category = category;
        }

        public string Name { get; set; }
        public Difficulty Difficulty { get; set; }
        public int TimeLimitInSeconds { get; set; }

        public string Category { get; set; }

        public List<Question> Questions { get; set; }
    }
}
