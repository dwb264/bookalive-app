public class QuizResult
{
    public string module;
    public string subject;
    public int durationSec;
    public int score;

    public QuizResult()
    {
    }

    public QuizResult(string module, string subject, int durationSec, int score)
    {
        this.module = module;
        this.subject = subject;
        this.durationSec = durationSec;
        this.score = score;
    }
}

