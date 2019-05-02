using System;
using System.Collections.Generic;

[System.Serializable]
public class Quiz
{
    public List<String> Questions;
    public List<List<String>> PossibleAnswers;
    public List<String> CorrectAnswers;

    public Quiz()
    {
    }

    public Quiz(List<String> questions, List<List<String>> possibleAnswers, List<String> correctAnswers) {
        this.Questions = questions;
        this.PossibleAnswers = possibleAnswers;
        this.CorrectAnswers = correctAnswers;
    }
}

