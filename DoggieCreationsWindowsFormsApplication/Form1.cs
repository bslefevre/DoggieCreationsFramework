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

        private void fromTextBox_KeyPress(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Return) return;

        }

        

        void Form1_ParentChanged(object sender, EventArgs e)
        {
            var panel = sender as Panel;
            panel.Controls.SetFullWidthToAllControls();
        }
    }

    public static class DCControl
    {
        public static void SetFullWidthToAllControls(this System.Windows.Forms.Control.ControlCollection controlCollection)
        {
            var ownerControl = controlCollection.Owner;
            var maxWidth = ownerControl.Parent != null ? ownerControl.Parent.Width : (int?)null;
            if (!maxWidth.HasValue) return;

            maxWidth = maxWidth - ownerControl.Margin.Horizontal - ownerControl.Parent.Margin.Horizontal;

            controlCollection.Owner.Width = maxWidth.Value;
            foreach (var control in controlCollection.Cast<Control>())
            {
                control.Width = maxWidth.Value - control.Margin.Horizontal;
            }
        }
        public static void AddControl(this System.Windows.Forms.Control.ControlCollection controlCollection, Control controlToAdd)
        {
            var combinedHeight = 0;
            foreach (var control in controlCollection.Cast<Control>())
            {
                combinedHeight += control.Height;
            }
            controlToAdd.Location = new Point(controlToAdd.Location.X, combinedHeight);
            controlCollection.Add(controlToAdd);
        }
    }
}
