﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CadExtension
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnLoadLines_Click(object sender, EventArgs e)
        {
            DbLoadUtil dbload=new DbLoadUtil();
            string result = dbload.LoadLines();
            lblInfo.Text = result;
        }

    
    }
}
