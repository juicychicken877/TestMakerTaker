﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestMakerTaker.Scripts.Forms
{
    public partial class Form_TestResult : Form {
        public Form_TestResult(Test solvedTest, Form_SolveTest.UserAnswer[] userAnswers) {
            InitializeComponent();

            int correct = 0, incorrect = 0, unanswered = 0;

            foreach (Form_SolveTest.UserAnswer answer in userAnswers) {
                if (answer != null) {
                    if (answer.IsCorrect()) correct++;
                    else incorrect++;

                    AnswerListElement newListElement = new AnswerListElement(answer.question, answer);

                    userAnswersPanel.Controls.Add(newListElement);
                } else {
                    unanswered++;
                }
            }

            correctCount.Text = correct.ToString();
            incorrectCount.Text = incorrect.ToString();
            unansweredCount.Text = unanswered.ToString();
        }

        private void closeBtn_Click(object? sender, EventArgs e) {
            this.Close();
        }
    }
}
