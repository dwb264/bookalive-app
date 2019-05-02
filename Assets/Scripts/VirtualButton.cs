using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class VirtualButton : MonoBehaviour, IVirtualButtonEventHandler
{

    public GameObject ButtonA;
    public GameObject ButtonB;
    public GameObject ButtonC;

    public TextMesh AnswerA;
    public TextMesh AnswerB;
    public TextMesh AnswerC;

    public TextMesh QuestionText;
    public TextMesh ExplainText;
    public TextMesh CaesarText;

    public enum QuizState { AwaitingAnswer, LoadingNextQuestion, Finishing, Done };
    public Quiz q;
    public int currentQuestion = 0;
    public int numCorrect = 0;
    public QuizState currentState;

    public float quizStartTime;
    public float questionStartTime;
    public float delay = 5.0f;

    public AudioSource wrong;
    public AudioSource correct;
    public AudioSource done;

    GameObject[] newCharacters;

    void Awake()
    {
        newCharacters = GameObject.FindGameObjectsWithTag("NewCharacters");
        foreach(GameObject c in newCharacters)
        {
            c.SetActive(false);
        }
    }

    // Use this for initialization
    void Start()
    {
        MakeQuiz();

        ButtonA = GameObject.Find("VirtualButtonA");
        ButtonB = GameObject.Find("VirtualButtonB");
        ButtonC = GameObject.Find("VirtualButtonC");
        QuestionText = GameObject.Find("QuestionText").GetComponent<TextMesh>();
        ExplainText = GameObject.Find("ExplainText").GetComponent<TextMesh>();
        CaesarText = GameObject.Find("CaesarText").GetComponent<TextMesh>();

        AnswerA = GameObject.Find("AnswerTextA").GetComponent<TextMesh>();
        AnswerB = GameObject.Find("AnswerTextB").GetComponent<TextMesh>();
        AnswerC = GameObject.Find("AnswerTextC").GetComponent<TextMesh>();

        wrong = GameObject.Find("Wrong").GetComponent<AudioSource>();
        correct = GameObject.Find("Right").GetComponent<AudioSource>();
        done = GameObject.Find("Done").GetComponent<AudioSource>();

        QuestionText.text = q.Questions[0];
        AnswerA.text = q.PossibleAnswers[0][0];
        AnswerB.text = q.PossibleAnswers[0][1];
        AnswerC.text = q.PossibleAnswers[0][2];

        ButtonA.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        ButtonB.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        ButtonC.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);

        currentState = QuizState.AwaitingAnswer;

        quizStartTime = Time.time;

    }

    public void OnButtonPressed(VirtualButtonBehaviour vb)
    {
        Debug.Log(vb.VirtualButtonName + " pressed");
    }

    public void OnButtonReleased(VirtualButtonBehaviour vb)
    {
        Debug.Log(vb.VirtualButtonName + " released");

        if (currentState == QuizState.AwaitingAnswer)
        {
            if (vb.VirtualButtonName == q.CorrectAnswers[currentQuestion])
            {
                correct.Play();
                ExplainText.text = "Correct!\n" + q.CorrectAnswers[currentQuestion] + " is the right answer!";
                CaesarText.text = "Correct!";
                numCorrect += 1;

            }
            else
            {
                if (vb.VirtualButtonName == "A") AnswerA.color = Color.red;
                if (vb.VirtualButtonName == "B") AnswerB.color = Color.red;
                if (vb.VirtualButtonName == "C") AnswerC.color = Color.red;

                wrong.Play();
                ExplainText.text = "Sorry, " + vb.VirtualButtonName + " is incorrect.\nThe correct answer was " + q.CorrectAnswers[currentQuestion];
                CaesarText.text = "Wrong!";
            }

            questionStartTime = Time.time;

            if (q.CorrectAnswers[currentQuestion] == "A") AnswerA.color = Color.green;
            if (q.CorrectAnswers[currentQuestion] == "B") AnswerB.color = Color.green;
            if (q.CorrectAnswers[currentQuestion] == "C") AnswerC.color = Color.green;

            if (currentQuestion < 2)
            {
                currentState = QuizState.LoadingNextQuestion;
            }
            else
            {
                currentState = QuizState.Finishing;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (currentState == QuizState.LoadingNextQuestion)
        {

            float currentTime = Time.time;
            if (currentTime - questionStartTime > delay)
            {
                AnswerA.color = Color.white;
                AnswerB.color = Color.white;
                AnswerC.color = Color.white;

                // Show next question
                currentQuestion += 1;
                QuestionText.text = q.Questions[currentQuestion];
                ExplainText.text = "";
                AnswerA.text = q.PossibleAnswers[currentQuestion][0];
                AnswerB.text = q.PossibleAnswers[currentQuestion][1];
                AnswerC.text = q.PossibleAnswers[currentQuestion][2];

                currentState = QuizState.AwaitingAnswer;
                CaesarText.text = "Let's go!";
            }
        }
        else if (currentState == QuizState.Finishing)
        {
            float currentTime = Time.time;
            if (currentTime - questionStartTime > 3.0f)
            {
                AnswerC.color = Color.white;
                CaesarText.text = "Great Job!";
                done.Play();
                FinishQuiz();
                currentState = QuizState.Done;
            }
        }
    }

    public void MakeQuiz()
    {
        // Hardcode for now

        q.Questions = new List<String>(new string[] {
            "Against whom did Caesar\nfight a civil war ?",
            "Which of these important things\ndid Caesar reform?",
            "What were Caesar's \n(alleged) last words?"
        });

        q.CorrectAnswers = new List<string>(new string[] { "A", "C", "B" });

        q.PossibleAnswers = new List<List<String>>();

        q.PossibleAnswers.Add(new List<string>(new string[] {
            "Pompey the Great", "Mark Antony", "Marcus Licinius Crassus"
        }));

        q.PossibleAnswers.Add(new List<string>(new string[] {
            "The Alphabet", "Units of measurement", "The Calendar"
        }));

        q.PossibleAnswers.Add(new List<string>(new string[] {
            "I can't believe you've done this", "You too, Brutus?", "Peace out, y'all"
        }));
    }

    public void FinishQuiz()
    {
        QuestionText.text = "Finished the quiz!\nYour score: " + numCorrect + "/3";
        ExplainText.text = "";
        AnswerA.text = "";
        AnswerB.text = "";
        AnswerC.text = "";

        int quizTime = (int)(Time.time - quizStartTime);
        int pct = (int)(numCorrect * 100 / 3.0f);

        Database.WriteQuizResult(quizTime, pct, "Ancient Rome", "History");

        AnswerC.text = "You unlocked new characters!";
        foreach (GameObject c in newCharacters)
        {
            c.SetActive(true);
        }
    }

}