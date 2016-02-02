using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace hoTools.Find
{
    public partial class RegularExpression : Form
    {
        public RegularExpression()
        {
            InitializeComponent();
            
        }

        private void RegularExpression_Load(object sender, EventArgs e)
        {
           lnkTutorial1.Links.Add(0,100, "http://www.mycsharp.de/wbb2/thread.php?threadid=41009");
           lnkRegExCoach.Links.Add(0,100, "http://www.weitz.de/regex-coach/");
            

        }

        private void lnkTutorial1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            e.Link.Visited = true;
            Process.Start(e.Link.LinkData as string);
        }

        private void lnkRegExCoach_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            e.Link.Visited = true;
            Process.Start(e.Link.LinkData as string);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }
    }
}
