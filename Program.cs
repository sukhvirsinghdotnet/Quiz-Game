using System.Diagnostics;

namespace QuizGame;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // Load questions
        var loader = new QuizLoader("questions.json");
        var allQuestions = loader.LoadQuestions();

        if (allQuestions.Count == 0)
        {
            Console.WriteLine("❌ No questions found! Please ensure questions.json exists.");
            return;
        }

        Console.WriteLine("╔════════════════════════════════════╗");
        Console.WriteLine("║    🎮 WELCOME TO QUIZ GAME! 🎮    ║");
        Console.WriteLine("╚════════════════════════════════════╝\n");

        // Show menu
        var quizQuestions = ShowCategoryMenu(allQuestions, loader);

        if (quizQuestions.Count == 0)
        {
            Console.WriteLine("❌ No questions available for selected criteria.");
            return;
        }

        // Create and run quiz
        var engine = new QuizEngine(quizQuestions);
        RunQuiz(engine);

        // Show results
        var result = engine.CompleteQuiz();
        ShowResults(result);

        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    static List<Question> ShowCategoryMenu(List<Question> allQuestions, QuizLoader loader)
    {
        var categories = loader.GetCategories(allQuestions);
        var difficulties = loader.GetDifficultyLevels(allQuestions);

        Console.WriteLine("📚 Available Categories:");
        for (int i = 0; i < categories.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {categories[i]}");
        }
        Console.WriteLine($"  {categories.Count + 1}. All Categories");

        Console.Write("\nSelect a category (enter number): ");
        if (!int.TryParse(Console.ReadLine(), out int categoryChoice) || categoryChoice < 1 || categoryChoice > categories.Count + 1)
        {
            Console.WriteLine("Invalid selection. Using all categories.");
            return allQuestions;
        }

        List<Question> selectedQuestions = categoryChoice <= categories.Count
            ? QuizEngine.FilterByCategory(allQuestions, categories[categoryChoice - 1])
            : allQuestions;

        // Difficulty selection
        Console.WriteLine("\n📊 Difficulty Levels:");
        for (int i = 0; i < difficulties.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {difficulties[i]}");
        }
        Console.WriteLine($"  {difficulties.Count + 1}. All Levels");

        Console.Write("\nSelect difficulty (enter number): ");
        if (!int.TryParse(Console.ReadLine(), out int difficultyChoice) || difficultyChoice < 1 || difficultyChoice > difficulties.Count + 1)
        {
            Console.WriteLine("Invalid selection. Using all levels.");
            return selectedQuestions;
        }

        if (difficultyChoice <= difficulties.Count)
        {
            selectedQuestions = QuizEngine.FilterByDifficulty(selectedQuestions, difficulties[difficultyChoice - 1]);
        }

        return selectedQuestions;
    }

    static void RunQuiz(QuizEngine engine)
    {
        engine.StartQuiz();
        Console.WriteLine($"\n✅ Starting Quiz with {engine.TotalQuestions} questions...\n");

        while (!engine.IsQuizComplete)
        {
            DisplayQuestion(engine);

            if (!GetAndValidateAnswer(engine))
                continue;

            if (!engine.IsQuizComplete)
            {
                Console.WriteLine("\nPress any key to continue to next question...");
                Console.ReadKey();
                Console.Clear();
            }

            engine.NextQuestion();
        }
    }

    static void DisplayQuestion(QuizEngine engine)
    {
        var question = engine.CurrentQuestion;
        if (question == null) return;

        var progress = engine.GetProgressPercentage();
        Console.WriteLine($"📋 Question {engine.CurrentQuestionIndex + 1}/{engine.TotalQuestions} ({progress:F0}% complete)");
        Console.WriteLine($"📂 Category: {question.Category} | 📊 Difficulty: {question.Difficulty}");
        Console.WriteLine($"\n{question.Title}\n");

        for (int i = 0; i < question.Options.Count; i++)
        {
            Console.WriteLine($"  {(char)('A' + i)}) {question.Options[i].Text}");
        }
    }

    static bool GetAndValidateAnswer(QuizEngine engine)
    {
        Console.Write("\nYour answer (A/B/C/D): ");
        var input = Console.ReadLine()?.ToUpper();

        if (string.IsNullOrEmpty(input) || input.Length != 1 || !char.IsLetter(input[0]))
        {
            Console.WriteLine("❌ Invalid input. Please enter A, B, C, or D.");
            return false;
        }

        int optionIndex = input[0] - 'A';
        var question = engine.CurrentQuestion;

        if (optionIndex < 0 || optionIndex >= question?.Options.Count)
        {
            Console.WriteLine("❌ Invalid option. Please try again.");
            return false;
        }

        bool isCorrect = engine.SubmitAnswer(optionIndex);

        Console.WriteLine(isCorrect ? "✅ Correct!" : "❌ Incorrect!");
        Console.WriteLine($"   Correct answer: {question.GetCorrectAnswer()?.Text}");
        if (!string.IsNullOrEmpty(question.Explanation))
        {
            Console.WriteLine($"   💡 Explanation: {question.Explanation}");
        }

        return true;
    }

    static void ShowResults(QuizResult result)
    {
        Console.Clear();
        Console.WriteLine("\n╔════════════════════════════════════╗");
        Console.WriteLine("║      🏆 QUIZ RESULTS 🏆            ║");
        Console.WriteLine("╚════════════════════════════════════╝\n");

        Console.WriteLine($"📊 Score: {result.CorrectAnswers}/{result.TotalQuestions} ({result.Percentage:F1}%)");
        Console.WriteLine($"🎯 Grade: {result.Grade}");
        Console.WriteLine($"⏱️  Time Taken: {result.TimeTaken.Minutes}m {result.TimeTaken.Seconds}s");
        Console.WriteLine($"✅ Correct: {result.CorrectAnswers}");
        Console.WriteLine($"❌ Wrong: {result.WrongAnswers}\n");

        Console.WriteLine("📝 Question Review:");
        Console.WriteLine("─────────────────────────────────────\n");

        for (int i = 0; i < result.QuestionResults.Count; i++)
        {
            var qr = result.QuestionResults[i];
            var icon = qr.IsCorrect ? "✅" : "❌";
            Console.WriteLine($"{icon} Q{i + 1}: {qr.Question}");
            Console.WriteLine($"   Your answer: {qr.UserAnswer}");
            if (!qr.IsCorrect)
            {
                Console.WriteLine($"   Correct answer: {qr.CorrectAnswer}");
            }
            if (!string.IsNullOrEmpty(qr.Explanation))
            {
                Console.WriteLine($"   💡 {qr.Explanation}");
            }
            Console.WriteLine();
        }
    }
}