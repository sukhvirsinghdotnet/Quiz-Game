using System.Text.Json;

namespace QuizGame;

/// <summary>
/// Load quiz questions from JSON file
/// </summary>
public class QuizLoader
{
    private readonly string _filePath;

    public QuizLoader(string filePath = "questions.json")
    {
        _filePath = filePath;
    }

    /// <summary>
    /// Load all questions from file
    /// </summary>
    public List<Question> LoadQuestions()
    {
        try
        {
            if (!File.Exists(_filePath))
            {
                Console.WriteLine($"Error: {_filePath} not found!");
                return new();
            }

            var json = File.ReadAllText(_filePath);
            var questions = JsonSerializer.Deserialize<List<Question>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return questions ?? new();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading questions: {ex.Message}");
            return new();
        }
    }

    /// <summary>
    /// Get available categories
    /// </summary>
    public List<string> GetCategories(List<Question> questions)
    {
        return questions.Select(q => q.Category).Distinct().ToList();
    }

    /// <summary>
    /// Get available difficulty levels
    /// </summary>
    public List<string> GetDifficultyLevels(List<Question> questions)
    {
        return questions.Select(q => q.Difficulty).Distinct().ToList();
    }
}