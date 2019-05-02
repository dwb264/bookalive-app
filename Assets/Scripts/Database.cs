using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;

public class Database : MonoBehaviour {

    public static string StudentId = "0";
    public static DatabaseReference reference;

	// Use this for initialization
	void Start () {

        FirebaseApp.DefaultInstance.SetEditorDatabaseUrl("https://bookalive-270fb.firebaseio.com");
        reference = FirebaseDatabase.DefaultInstance.RootReference;

        //WriteQuizResult(60, 100, "Ancient Rome", "History");

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void WriteQuizResult(int durationSec, int score, string module, string subject)
    {
        QuizResult q = new QuizResult(module, subject, durationSec, score);
        string json = JsonUtility.ToJson(q);
        string date = System.DateTime.Now.ToString().Replace("/", "-");
        Debug.Log(date);

        reference.Child("quizResults/" + StudentId).Child(date).SetRawJsonValueAsync(json);
    }
}
