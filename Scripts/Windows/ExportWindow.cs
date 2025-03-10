﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuizForge.Scripts.Forms;
using QuizForge.Scripts.UserControls;

namespace QuizForge.Scripts.Windows
{
    public partial class ExportWindow : Form {
        public ExportWindow(List<Quiz> quizzes, bool selectAll) {
            this.InitializeComponent();

            exportView.Update(quizzes);

            if (selectAll) exportView.SelectAll();

            ImportExportHandler.OnExport += ImportExportHandler_OnExport;
        }

        private void ImportExportHandler_OnExport(object? sender, EventArgs e) {
            this.Close();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            this.Close();
        }
    }
}
