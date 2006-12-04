using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace AIMLGUI
{
    public partial class ViewInformation : Form
    {
        public string OutputMessage
        {
            set
            {
                this.richTextBoxInfo.Text = value;
            }
        }

        public ViewInformation()
        {
            InitializeComponent();
        }
    }
}