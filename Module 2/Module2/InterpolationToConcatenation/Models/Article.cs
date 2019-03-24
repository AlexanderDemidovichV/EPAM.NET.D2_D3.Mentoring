namespace InterpolationToConcatenation.Models
{
    public class Article
    {
        public Article(string title, int cost)
        {
            Title = title;
            Cost = cost;
        }
        
        public string Title { get; }
        public int Cost { get; }
    }
}