using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace NEA
{
    public partial class RPG : Form
    {
        // creates and returns a Label and adds it to the TableLayoutPanel
        private Label CreateLabel(TableLayoutPanel tblLayout, string text, string name, int x, int y, int colSpan = 1)
        {
            // creates a temporary Label
            Label lblTemp = new Label();
            // sets text formatting to centre alignment
            lblTemp.TextAlign = ContentAlignment.MiddleCenter;
            // allows the label to get larger and smaller as the window does so
            lblTemp.AutoSize = true;
            // makes the label stretch as the window changes
            lblTemp.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            // sets the label font
            lblTemp.Font = new Font(FontFamily.GenericSansSerif, 24.0F, FontStyle.Bold);
            // sets location for the button - the centre of the container (TableLayoutPanel cell)
            lblTemp.Location = new Point(this.Width / 2 - 2 * lblTemp.Width, 50);
            // sets the text on the label to contain the string from the first parameter
            lblTemp.Text = text;
            // sets the name of the label to the string from the second parameter
            lblTemp.Name = name;
            // sets the span of the Label to the number of columns in the optional parameter
            tblLayout.SetColumnSpan(lblTemp, colSpan);
            // adds the Label to the TableLayoutPanel
            tblLayout.Controls.Add(lblTemp, x, y);
            // returns the Label
            return lblTemp;
        }

        // creates and reutrns a Button with an EventHandler
        private Button CreateButton(TableLayoutPanel tblLayout, string text, DisplayOption option)
        {
            // creates a temporary button
            Button tempButton = new Button();
            // sets the text on the button to the string from parameters
            tempButton.Text = text;
            // makes the button stretch as the window changes
            tempButton.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            // EventHandler, when button is clicked the parameter method is run through the use of Delegates
            tempButton.Click += new EventHandler((sender, e) =>
            {
                ShowMenu = new DisplayOption(option);
                ShowMenu(tblLayout);
            });

            return tempButton;
        }
        // creates and returns a Button without an EventHandler
        private Button CreateButton(TableLayoutPanel tblLayout, string text)
        {
            // creates a temporary button
            Button tempButton = new Button();
            // sets the text on the button to the string from parameters
            tempButton.Text = text;
            // makes the button stretch as the window changes
            tempButton.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            return tempButton;
        }

        // adds a Button to the TableLayoutPanel
        private void AddButton(TableLayoutPanel tblLayout, Button btn, int x, int y)
        {
            tblLayout.Controls.Add(btn, x, y);
        }

        // moves a Control from one TableLayoutPanel cell to another
        private void MoveControl(TableLayoutPanel tblLayout, int fromX, int fromY, int toX, int toY, bool show = true)
        {
            // shows the control so it can be manipulated (only visible controls can be moved)
            tblLayout.GetControlFromPosition(fromX, fromY).Show();
            // moves control from (fromX, fromY) to (toX, toY) in the TableLayoutPanel
            tblLayout.Controls.Add(tblLayout.GetControlFromPosition(fromX, fromY), toX, toY);
            // sets the control's visibility to that specified in the parameter
            tblLayout.GetControlFromPosition(toX, toY).Visible = show;
        }

        // changes the title Label to be the specified text
        private void ModifyTitle(TableLayoutPanel tblLayout, string text)
        {
            tblLayout.GetControlFromPosition(0, 0).Text = text;
        }

        // hides the three main menu buttons (Design, Play, Quit)
        private void HideStandardButtons(TableLayoutPanel tblLayout)
        {
            /* moves the controls from original locations to new locations and then hides each button */
            MoveControl(tblLayout, 1, 2, 0, 5, false);
            MoveControl(tblLayout, 3, 2, 1, 5, false);
            MoveControl(tblLayout, 2, 4, 4, 5, false);
        }

        // shows the three main menu buttons (Design, Play, Quit)
        private void ShowStandardButtons(TableLayoutPanel tblLayout)
        {
            /* moves the controls from new locations to original locations and then shows each button */
            MoveControl(tblLayout, 0, 5, 1, 2);
            MoveControl(tblLayout, 1, 5, 3, 2);
            MoveControl(tblLayout, 4, 5, 2, 4);
        }

        private void ClearScreen(TableLayoutPanel tblLayout)
        {
            for (int x = 0; x < 5; x++)
            {
                for (int y = 1; y < 5; y++)
                {
                    tblLayout.Controls.Remove(tblLayout.GetControlFromPosition(x, y));
                }
            }
        }
    }
}
