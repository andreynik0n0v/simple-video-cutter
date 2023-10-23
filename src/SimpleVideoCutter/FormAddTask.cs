using SimpleVideoCutter.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SimpleVideoCutter
{
    public partial class FormAddTask : Form
    {
        private FFmpegTask task;
        private bool selectionsOnKeyFrames = false;

        public FormAddTask(FFmpegTask task, bool selectionsOnKeyFrames)
        {
            InitializeComponent();
            this.selectionsOnKeyFrames = selectionsOnKeyFrames;

            this.Task = Utils.DeepCloneXML(task);
            TaskToGUI();
            buttonEnqueueReEncoding.Focus();
        }

        public FFmpegTask Task { get => task; set => task = value; }

        private const string OriginalFilePathLabel = "Original File Path:";
        private const string OutputFilePathLabel = "Output File Path:";
        private const string DurationLabel = "Duration:";

        private void TaskToGUI()
        {
            try
            {
                this.textBoxOriginalFilePath.Text = Task.InputFilePath;
                this.textBoxOutputFilePath.Text = Task.OutputFilePath;
                this.textBoxDuration.Text = TimeSpan.FromMilliseconds(Task.OverallDuration).ToString(@"hh\:mm\:ss\.fff");
                UpdatePossibilities();
            }
            finally
            {
            }
        }

        
        private void UpdatePossibilities()
        {
            var losslessCutPossible = selectionsOnKeyFrames; 

            buttonAdjustSelections.Visible = !losslessCutPossible;
            buttonEnqueueLoseless.Enabled = losslessCutPossible;
            buttonEnqueueReEncoding.Enabled = true;

            if (losslessCutPossible)
            {
                labelLossless.Text = GlobalStrings.FormAddTask_LosslessCutPossible;
                labelReEncode.Text = GlobalStrings.FormAddTask_ReEncodePossibleButSlow;
                groupBoxLosslessCut.BackColor = Color.Honeydew;
            }
            else
            {
                labelLossless.Text = GlobalStrings.FormAddTask_LosslessCutNotPossible;
                labelReEncode.Text = GlobalStrings.FormAddTask_ReEncodePossibleButSlow;
            }
        }

        internal class ComboBoxItem
        {
            public string Title { get; set; }
            public string Value { get; set; }

            public ComboBoxItem(string title, string value)
            {
                Title = title;
                Value = value;
            }

            public bool Equals(ComboBoxItem other)
            {
                return String.Equals(Value, other.Value);
            }

            public override bool Equals(object obj)
            {
                if (obj is ComboBoxItem)
                {
                    var other = obj as ComboBoxItem;
                    return Equals(other);
                }
                else
                    return false;
            }

            public override int GetHashCode()
            {
                return Value == null ? 0 : Value.GetHashCode();
            }
        }


        private void richTextBoxExplanation_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(e.LinkText);
        }
        private void HandleLosslessButtonClick()
        {
            Task.Lossless = true;
        }

        private void HandleReEncodingButtonClick()
        {
            Task.Lossless = false;
        }

        private void buttonEnqueueLoseless_Click(object sender, EventArgs e)
        {
            HandleLosslessButtonClick();
        }

        private void buttonEnqueueReEncoding_Click(object sender, EventArgs e)
        {
            HandleReEncodingButtonClick();
        }

        
    }
}
