namespace QuizGame;

/// <summary>
/// Core engine for managing quiz flow and scoring
/// </summary>
public class QuizEngine
{
    private List<Question> _questions = new();
    private int _currentQuestionIndex = 0;
    private QuizResult _result = new();
    private DateTime _startTime;
    private List<int?> _userAnswers = new();

    public QuizEngine(List<Question> questions)
    {
        _questions = questions.OrderBy(q => Guid.NewGuid()).ToList(); // Shuffle questions
        _result.TotalQuestions = _questions.Count;
        _userAnswers = new List<int?>(new int?[_questions.Count]);
    }

    public int TotalQuestions => _questions.Count;
    public int CurrentQuestionIndex => _currentQuestionIndex;
    public Question? CurrentQuestion => _currentQuestionIndex < _questions.Count ? _questions[_currentQuestionIndex] : null;
    public bool IsQuizComplete => _currentQuestionIndex >= _questions.Count;
    public QuizResult Result => _result;

    /// <summary>
    /// Start the quiz timer
    /// </summary>
    public void StartQuiz()
    {
        _startTime = DateTime.Now;
    }

    /// <summary>
    /// Submit an answer for the current question
    /// </summary>
    public bool SubmitAnswer(int optionIndex)
    {
        var currentQuestion = CurrentQuestion;
        if (currentQuestion == null)
            return false;

        _userAnswers[_currentQuestionIndex] = optionIndex;

        // Check if answer is correct
        bool isCorrect = currentQuestion.IsAnswerCorrect(optionIndex);

        if (isCorrect)
        {
            _result.CorrectAnswers++;
        }

        // Record question result
        var questionResult = new QuestionResult
        {
            QuestionId = currentQuestion.Id,
            Question = currentQuestion.Title,
            UserAnswerIndex = optionIndex,
            IsCorrect = isCorrect,
            UserAnswer = currentQuestion.Options[optionIndex].Text,
            CorrectAnswer = currentQuestion.GetCorrectAnswer()?.Text ?? "Unknown",
            Explanation = currentQuestion.Explanation,
            CorrectAnswerIndex = currentQuestion.Options.FindIndex(o => o.IsCorrect)
        };

        _result.QuestionResults.Add(questionResult);

        return isCorrect;
    }

    /// <summary>
    /// Move to the next question
    /// </summary>
    public bool NextQuestion()
    {
        if (!IsQuizComplete)
        {
            _currentQuestionIndex++;
            return !IsQuizComplete;
        }
        return false;
    }

    /// <summary>
    /// Complete the quiz and calculate results
    /// </summary>
    public QuizResult CompleteQuiz()
    {
        _result.TimeTaken = DateTime.Now - _startTime;
        _result.CalculateResults();
        return _result;
    }

    /// <summary>
    /// Get progress percentage
    /// </summary>
    public double GetProgressPercentage()
    {
        return (_currentQuestionIndex + 1) / (double)_questions.Count * 100;
    }

    /// <summary>
    /// Filter questions by category
    /// </summary>
    public static List<Question> FilterByCategory(List<Question> questions, string category)
    {
        return questions.Where(q => q.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
    }

    /// <summary>
    /// Filter questions by difficulty
    /// </summary>
    public static List<Question> FilterByDifficulty(List<Question> questions, string difficulty)
    {
        return questions.Where(q => q.Difficulty.Equals(difficulty, StringComparison.OrdinalIgnoreCase)).ToList();
    }
}