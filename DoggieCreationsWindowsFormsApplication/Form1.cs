using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DoggieCreationsFramework;

namespace DoggieCreationsWindowsFormsApplication
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            var toTextBox = new TextBox();
            var fromTextBox = new TextBox(); fromTextBox.KeyDown += (sender, e) =>
            {
                if (e.KeyCode != Keys.Return) return;
                var textBox = sender as TextBox;
                toTextBox.Text = TranslateClass.Translate(textBox.Text, "nl", "es");
            };
            var button = new Button() { Text = "Vertaal" };
            
            var panel = new Panel();
            panel.ParentChanged += Form1_ParentChanged;
            panel.Controls.AddControl(fromTextBox);
            panel.Controls.AddControl(button);
            panel.Controls.AddControl(toTextBox);
            button.Click += (sender, arg) => toTextBox.Text = TranslateClass.Translate(fromTextBox.Text, "nl", "es");
            components.Add(panel);
            Controls.Add(panel);
        }

        void Form1_ParentChanged(object sender, EventArgs e)
        {
            var panel = sender as Panel;
            panel.Controls.SetFullWidthToAllControls();
        }
    }
}
