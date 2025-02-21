﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TestMakerTaker.Scripts.Forms;
using System.Text.Json.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using static TestMakerTaker.Scripts.TestListJSON;

namespace TestMakerTaker.Scripts
{
    internal class JSONHandler
    {
        private const string SAVED_TESTS_FILE_PATH = "../../../Data/SavedTests.json";
        private const string EXAMPLE_TESTS_FILE_PATH = "../../../Data/ExampleTests.json";
        public JSONHandler() { }

        public void SaveTestsToJSON(List<Test> tests)
        {
            try
            {
                TestListJSON testsObject = new TestListJSON();
                testsObject.Initialize(tests);

                string jsonString = JsonSerializer.Serialize(testsObject);
                File.WriteAllText(SAVED_TESTS_FILE_PATH, jsonString);
            }
            catch (Exception ex)
            {
                ShowFileError("Saving Failed", ex.Message);
            }
        }
        private List<Test> ConvertToTestList(TestListJSON testList) {
            List<Test> tests = new();

            foreach (TestJSON testJSON in testList.testsJSON) {
                // Get questions and convert from QuestionJSON to Question
                List<Question> questions = new();
                foreach (QuestionJSON questionJSON in testJSON.questions) {
                    Question question = new(questionJSON.question, questionJSON.answers, questionJSON.correctAnswer);
                    questions.Add(question);
                }
                // Instantiate test
                tests.Add(new Test(testJSON.title, testJSON.description, questions));
            }

            return tests;
        }
        private TestListJSON LoadTestsFromJSON(string path) {
            string jsonString = File.ReadAllText(path);
            TestListJSON testList = JsonSerializer.Deserialize<TestListJSON>(jsonString);

            return testList;
        }

        private void ShowFileError(string title, string description) {
            MessageWindow newErrorMessage = new MessageWindow(MessageWindow.MessageDialogMode.Error, title, description, "OK", "OK");

            newErrorMessage.ShowDialog();
        }

        public List<Test> GetSavedTests()
        {
            List<Test> loadedTests = new();
            try
            {
                if (File.Exists(SAVED_TESTS_FILE_PATH))
                {
                    TestListJSON testList = LoadTestsFromJSON(SAVED_TESTS_FILE_PATH);

                    loadedTests = ConvertToTestList(testList);

                    return loadedTests;
                }
                else
                {
                    ShowFileError("No File Found", "SavedTests.json was not found.");
                }
            }
            catch (Exception ex)
            {
                ShowFileError("JSON Handler Exception", ex.Message);
            }

            return loadedTests;
        }
    }
    public class TestListJSON {
        /*
            Special classes for creating objects for json serialization purposes
            - public getters & setters
            - empty constructors
            - json property name defined
         */
        public class TestJSON {
            [JsonPropertyName("title")]
            public string title { get; set; }
            [JsonPropertyName("description")]
            public string description { get; set; }
            [JsonPropertyName("questions")]
            public List<QuestionJSON> questions { get; set; }

            public TestJSON() { }
            public void Initialize(string title, string description, List<QuestionJSON> questions) {
                this.title = title;
                this.description = description;
                this.questions = questions;
            }
        }

        public class QuestionJSON {
            [JsonPropertyName("question")]
            public string question { get; set; }
            [JsonPropertyName("answers")]
            public List<string> answers { get; set; }
            [JsonPropertyName("correctAnswer")]
            public string correctAnswer { get; set; }

            public QuestionJSON() { }
            public void Initialize(string q, List<string> ans, string correct) {
                this.question = q;
                this.answers = ans;
                this.correctAnswer = correct;
            }
        }


        [JsonPropertyName("tests")]
        public List<TestJSON> testsJSON { get; set; }
        public TestListJSON() { }

        public void Initialize(List<Test> tests) {
            testsJSON = new();

            // Generate Test JSON for every test
            foreach (Test test in tests) {
                // Initialize Question JSON Object
                List<QuestionJSON> newQuestions = new();
                foreach (Question question in test.questions) {
                    QuestionJSON newQuestion = new QuestionJSON();
                    newQuestion.Initialize(question.question, question.answers, question.correctAnswer);
                    newQuestions.Add(newQuestion);
                }
                TestJSON newTest = new TestJSON();
                newTest.Initialize(test.title, test.description, newQuestions);
                testsJSON.Add(newTest);
            }
        }
    }
}
