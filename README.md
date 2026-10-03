# Quiz Game - Multiple Choice Questions with Scoring

## Understanding
Build a console-based and beginner simplicity C# Quiz Game application where users answer multiple choice questions, get immediate feedback, track their score, and view results.

## Assumptions
- Console-based application (simple, fast to develop)
- Questions loaded from JSON or CSV file
- Single quiz session per run
- Real-time score tracking
- Immediate feedback on answers (correct/incorrect)
- Final score summary and statistics

## Approach
Create a modular quiz system with:
1. Question & Answer classes for data modeling
2. QuizEngine to manage quiz flow and scoring
3. QuizLoader to load questions from file
4. Console UI for user interaction
5. Score calculator with statistics
6. Questions stored in JSON format for easy management

## Key Files
- Question.cs - Question model
- Answer.cs - Answer model
- QuizEngine.cs - Core quiz logic and scoring
- QuizLoader.cs - Load questions from JSON file
- Program.cs - Console UI and main flow
- questions.json - Quiz questions database

## Risks & Open Questions
- JSON parsing complexity (mitigated by using System.Text.Json)
- File path handling (use relative paths from project folder)
- Question validation (ensure valid question format)

## Steps
1. Create Question.cs - Question data model
2. Create Answer.cs - Answer data model
3. Create QuizEngine.cs - Quiz logic, scoring, and flow control
4. Create QuizLoader.cs - Load questions from JSON file
5. Create questions.json - Sample quiz questions
6. Update Program.cs - Console UI and user interaction
7. Build and test the quiz game
8. Add additional features (categories, difficulty levels, etc.)
