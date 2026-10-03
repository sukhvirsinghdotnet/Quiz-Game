namespace QuizGame;

/// <summary>
/// Represents the result of a quiz session
/// </summary>
public class QuizResult
{
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; }
    public int WrongAnswers { get; set; }
    public double Percentage { get; set; }
    public TimeSpan TimeTaken { get; set; }
    public string Grade { get; set; } = string.Empty;
    public List<QuestionResult> QuestionResults { get; set; } = new();

    public void CalculateResults()
    {
        WrongAnswers = TotalQuestions - CorrectAnswers;
        Percentage = (double)CorrectAnswers / TotalQuestions * 100;
        Grade = GetGrade(Percentage);
    }

    private string GetGrade(double percentage)
    {
        return percentage switch
        {
            >= 90 => "A (Excellent!)",
            >= 80 => "B (Very Good)",
            >= 70 => "C (Good)",
            >= 60 => "D (Pass)",
            >= 50 => "E (Poor)",
            _ => "F (Failed)"
        };
    }
}

/// <summary>
/// Represents the result of a single question
/// </summary>
public class QuestionResult
{
    public int QuestionId { get; set; }
    public string Question { get; set; } = string.Empty;
    public int? UserAnswerIndex { get; set; }
    public int CorrectAnswerIndex { get; set; }
    public bool IsCorrect { get; set; }
    public string UserAnswer { get; set; } = string.Empty;
    public string CorrectAnswer { get; set; } = string.Empty;
    public string? Explanation { get; set; }
}