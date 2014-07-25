using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Resolver
{
    public partial class MainForm : System.Web.UI.Page
    {
        InputResolver ir;

        protected void Page_Load(object sender, EventArgs e)
        {
            ir = new InputResolver();
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            result.Text = "Result: " + ir.getResult(inputbox.Text);
        }

    }
}