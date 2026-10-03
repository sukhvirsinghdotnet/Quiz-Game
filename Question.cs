namespace QuizGame;

/// <summary>
/// Represents a quiz question with multiple answers
/// </summary>
public class Question
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Difficulty { get; set; } = "Easy"; // Easy, Medium, Hard
    public List<Answer> Options { get; set; } = new();
    public string? Explanation { get; set; }

    public Question() { }

    public Question(int id, string title, string category, string difficulty)
    {
        Id = id;
        Title = title;
        Category = category;
        Difficulty = difficulty;
    }

    /// <summary>
    /// Get the correct answer
    /// </summary>
    public Answer? GetCorrectAnswer()
    {
        return Options.FirstOrDefault(o => o.IsCorrect);
    }

    /// <summary>
    /// Check if the given answer is correct
    /// </summary>
    public bool IsAnswerCorrect(int optionIndex)
    {
        if (optionIndex < 0 || optionIndex >= Options.Count)
            return false;
        return Options[optionIndex].IsCorrect;
    }
}