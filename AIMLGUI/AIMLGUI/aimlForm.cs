using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AIMLbot;

namespace AIMLGUI
{
    public partial class aimlForm : Form
    {
        private Bot myBot;
        private User myUser;
        private Request lastRequest = null;
        private Result lastResult = null;

        public aimlForm()
        {
            InitializeComponent();
            this.richTextBoxInput.Focus();
            myBot = new Bot();
            myUser = new User("DefaultUser",this.myBot);
            myBot.WrittenToLog += new Bot.LogMessageDelegate(myBot_WrittenToLog);
        }

        void myBot_WrittenToLog()
        {
            this.richTextBoxOutput.Text += this.myBot.LastLogMessage+Environment.NewLine+Environment.NewLine;
            this.richTextBoxOutput.ScrollToCaret();
        }

        #region Menu Item Events
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult exitQuery = MessageBox.Show("Are you sure?", "Are you sure?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (exitQuery == DialogResult.OK)
            {
                Application.Exit();
            }
        }

        private void saveBotToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo fi = new FileInfo(Application.ExecutablePath);
                saveFileDialogDump.InitialDirectory = fi.DirectoryName;
                saveFileDialogDump.AddExtension = true;
                saveFileDialogDump.DefaultExt = "dat";
                saveFileDialogDump.FileName = "Graphmaster.dat";
                DialogResult dr = saveFileDialogDump.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    if (this.myBot.Size > 0)
                    {
                        this.myBot.isAcceptingUserInput = false;
                        this.myBot.saveToBinaryFile(saveFileDialogDump.FileName);
                        this.myBot.isAcceptingUserInput = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.richTextBoxOutput.Text += ex.Message + Environment.NewLine;
            }
        }

        private void fromAIMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                folderBrowserDialogAIML.RootFolder = Environment.SpecialFolder.MyComputer;
                folderBrowserDialogAIML.SelectedPath = this.myBot.PathToAIML;
                DialogResult dr = folderBrowserDialogAIML.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    AIMLbot.Utils.AIMLLoader loader = new AIMLbot.Utils.AIMLLoader(this.myBot);
                    this.myBot.isAcceptingUserInput = false;
                    if (folderBrowserDialogAIML.SelectedPath.Length > 0)
                    {
                        loader.loadAIML(folderBrowserDialogAIML.SelectedPath);
                    }
                    else
                    {
                        loader.loadAIML(this.myBot.PathToAIML);
                    }
                    this.myBot.isAcceptingUserInput = true;
                }
            }
            catch (Exception ex)
            {
                this.richTextBoxOutput.Text += ex.Message + Environment.NewLine;
            }
        }

        private void fromDatFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo fi = new FileInfo(Application.ExecutablePath);
                openFileDialogDump.InitialDirectory = fi.DirectoryName;
                openFileDialogDump.AddExtension = true;
                openFileDialogDump.DefaultExt = "dat";
                openFileDialogDump.FileName = "Graphmaster.dat";
                DialogResult dr = openFileDialogDump.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    this.myBot.isAcceptingUserInput = false;
                    this.myBot.loadFromBinaryFile(openFileDialogDump.FileName);
                    this.myBot.isAcceptingUserInput = true;
                }
            }
            catch (Exception ex)
            {
                this.richTextBoxOutput.Text += ex.Message + Environment.NewLine;
            }
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder();

            result.Append("Bot Settings:" + Environment.NewLine + Environment.NewLine);
            foreach (string setting in this.myBot.GlobalSettings.SettingNames)
            {
                result.Append(setting + ": " + this.myBot.GlobalSettings.grabSetting(setting)+Environment.NewLine);
            }

            this.showInformation(result);
        }

        private void userToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder result = new StringBuilder();

            result.Append("User Information:" + Environment.NewLine + Environment.NewLine);

            result.Append("UserID: " + this.myUser.UserID + Environment.NewLine);
            result.Append("Topic: " + this.myUser.Topic + Environment.NewLine + Environment.NewLine);

            result.Append("User Predicate List:" + Environment.NewLine);
            foreach (string setting in this.myUser.Predicates.SettingNames)
            {
                result.Append(setting + ": " + this.myUser.Predicates.grabSetting(setting)+Environment.NewLine);
            }
            this.showInformation(result);
        }

        private void lastRequestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!object.Equals(null, this.lastRequest))
            {
                StringBuilder result = new StringBuilder();

                result.Append("Last Request:" + Environment.NewLine + Environment.NewLine);

                result.Append("Raw Input: " + this.lastRequest.rawInput.Replace(Environment.NewLine,"") + Environment.NewLine);
                result.Append("Started On: " + this.lastRequest.StartedOn + Environment.NewLine);
                result.Append("Has Timed Out: " + Convert.ToString(this.lastRequest.hasTimedOut) + Environment.NewLine + Environment.NewLine);
                this.showInformation(result);
            }
        }

        private void lastResultToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!object.Equals(null, this.lastResult))
            {
                StringBuilder result = new StringBuilder();

                result.Append("Last Result:" + Environment.NewLine + Environment.NewLine);

                result.Append("Raw Input: " + this.lastResult.RawInput + Environment.NewLine);
                result.Append("Output: " + this.lastResult.Output + Environment.NewLine);
                result.Append("Raw Output: " + this.lastResult.RawOutput + Environment.NewLine);
                result.Append("Duration: "+this.lastResult.Duration.ToString() + Environment.NewLine + Environment.NewLine);
                result.Append("Sentences: " + Environment.NewLine);
                foreach (string sentence in this.lastResult.InputSentences)
                {
                    result.Append(sentence + Environment.NewLine);
                }
                result.Append(Environment.NewLine);
                
                result.Append(Environment.NewLine);
                result.Append("Sub Queries: " + Environment.NewLine);
                result.Append(Environment.NewLine);
                foreach (AIMLbot.Utils.SubQuery query in this.lastResult.SubQueries)
                {
                    result.Append("Path: " + query.FullPath + Environment.NewLine);
                    result.Append("Template: " + Environment.NewLine + query.Template + Environment.NewLine);
                    result.Append(Environment.NewLine);
                    result.Append("Input Stars:" + Environment.NewLine);
                    foreach (string star in query.InputStar)
                    {
                        result.Append(star + Environment.NewLine);
                    }
                    result.Append(Environment.NewLine);
                    result.Append("That Stars:" + Environment.NewLine);
                    foreach (string that in query.ThatStar)
                    {
                        result.Append(that + Environment.NewLine);
                    }
                    result.Append(Environment.NewLine);
                    result.Append("Topic Stars:" + Environment.NewLine);
                    foreach (string topic in query.TopicStar)
                    {
                        result.Append(topic + Environment.NewLine);
                    }
                    result.Append(Environment.NewLine);
                }
                result.Append(Environment.NewLine);
                result.Append("Output Sentences: " + Environment.NewLine);
                foreach (string outputSentence in this.lastResult.OutputSentences)
                {
                    result.Append(outputSentence + Environment.NewLine);
                }
                this.showInformation(result);
            }
        }

        private void showInformation(StringBuilder result)
        {
            ViewInformation vi = new ViewInformation();
            vi.OutputMessage = result.ToString();
            vi.ShowDialog(this);
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string about = @"AIMLGui, Program# / AIMLBot (c) 2006 Nicholas H.Tollervey.
http://ntoll.org

This is a .NET implementation of the ALICE chatterbot using the AIML specification. Put simply, this software will allow you to chat (by entering text) with your computer using natural language.

Program# is a complete re-write of an earlier C# AIML implementation called AIMLBot. It is available under the Gnu LGPL. This means that you are free to download, modify and share it. Links to download Program# can be found at the bottom of the page.

";
            MessageBox.Show(about, "About", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void licenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string content = @"
Program# / AIMLBot - a .Net implementation of the AIML standard.
Copyright (C) 2006  Nicholas H.Tollervey (http://ntoll.org)

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
";
            MessageBox.Show(content, "License", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        #endregion

        private void buttonGo_Click(object sender, EventArgs e)
        {
            this.processInputFromUser();
        }

        private void processInputFromUser()
        {
            if (this.myBot.isAcceptingUserInput)
            {
                string rawInput = this.richTextBoxInput.Text;
                this.richTextBoxInput.Text = string.Empty;
                this.richTextBoxOutput.AppendText("You: " + rawInput + Environment.NewLine);
                Request myRequest = new Request(rawInput, this.myUser, this.myBot);
                Result myResult = this.myBot.Chat(myRequest);
                this.lastRequest = myRequest;
                this.lastResult = myResult;
                this.richTextBoxOutput.AppendText("Bot: " + myResult.RawOutput + Environment.NewLine + Environment.NewLine);
            }
            else
            {
                this.richTextBoxInput.Text = string.Empty;
                this.richTextBoxOutput.AppendText("Bot not accepting user input." + Environment.NewLine);
            }
        }

        private void singleFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo fi = new FileInfo(Path.Combine(Application.ExecutablePath, "aiml"));
                openFileDialogDump.InitialDirectory = fi.DirectoryName;
                openFileDialogDump.AddExtension = true;
                openFileDialogDump.DefaultExt = "aiml";
                openFileDialogDump.FileName = "Reduce.aiml";
                DialogResult dr = openFileDialogDump.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    AIMLbot.Utils.AIMLLoader loader = new AIMLbot.Utils.AIMLLoader(this.myBot);
                    this.myBot.isAcceptingUserInput = false;
                    loader.loadAIMLFile(openFileDialogDump.FileName);
                    this.myBot.isAcceptingUserInput = true;
                }
            }
            catch (Exception ex)
            {
                this.richTextBoxOutput.Text += ex.Message + Environment.NewLine;
            }
        }

        private void richTextBoxInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.processInputFromUser();
            }
        }

        private void richTextBoxOutput_TextChanged(object sender, EventArgs e)
        {
            this.richTextBoxOutput.ScrollToCaret();
        }

        private void toolStripMenuItemCustomLib_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo fi = new FileInfo(Application.ExecutablePath);
                openFileDialogDump.InitialDirectory = fi.DirectoryName;
                openFileDialogDump.AddExtension = true;
                openFileDialogDump.DefaultExt = "dll";
                DialogResult dr = openFileDialogDump.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    if (openFileDialogDump.FileName.Length > 0)
                    {
                        this.myBot.isAcceptingUserInput = false;
                        this.myBot.loadCustomTagHandlers(openFileDialogDump.FileName);
                        this.myBot.isAcceptingUserInput = true;
                    }
                }
            }
            catch (Exception ex)
            {
                this.richTextBoxOutput.Text += ex.Message + Environment.NewLine;
            }
        }
    }
}