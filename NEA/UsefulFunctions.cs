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
        private Label CreateLabel(TableLayoutPanel tblLayout, string text, string name, int x, int y, int colSpan = 1, int rowSpan = 1, float fontSize = 18.0F, ContentAlignment alignment = ContentAlignment.MiddleCenter)
        {
            // creates a temporary Label
            Label lblTemp = new Label();
            // sets text formatting to centre alignment
            lblTemp.TextAlign = alignment;
            // allows the label to get larger and smaller as the window does so
            lblTemp.AutoSize = true;
            // makes the label stretch as the window changes
            lblTemp.Anchor = (AnchorStyles.Left | AnchorStyles.Right);
            // sets the label font
            lblTemp.Font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold);
            // sets location for the button - the centre of the container (TableLayoutPanel cell)
            lblTemp.Location = new Point(this.Width / 2 - 2 * lblTemp.Width, 50);
            // sets the text on the label to contain the string from the first parameter
            lblTemp.Text = text;
            // sets the name of the label to the string from the second parameter
            lblTemp.Name = name;
            // sets the span of the Label to the number of columns in the optional parameter
            tblLayout.SetColumnSpan(lblTemp, colSpan);
            // sets the span of the Label to the number of rows in the optional parameter
            tblLayout.SetRowSpan(lblTemp, rowSpan);
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
            // sets the button font
            tempButton.Font = new Font(tempButton.Font.FontFamily, 12.0F);
            // makes the button stretch as the window changes
            tempButton.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            // EventHandler, when button is clicked the parameter method is run using Delegates
            // use of Lambda Expressions
            tempButton.Click += new EventHandler((sender, e) =>
            {
                // use of Delegates
                ShowMenu = new DisplayOption(option);
                ShowMenu(tblLayout);
            });

            return tempButton;
        }
        // creates and returns a Button without an EventHandler
        // use of Overloading
        private Button CreateButton(string text)
        {
            // creates a temporary button
            Button tempButton = new Button();
            // sets the text on the button to the string from parameters
            tempButton.Text = text;
            // sets the button font
            tempButton.Font = new Font(tempButton.Font.FontFamily, 12.0F);
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
        // use of optional parameters (simpler Overloading)
        private void MoveControl(TableLayoutPanel tblLayout, int fromX, int fromY, int toX, int toY, bool show = true)
        {
            // try catch used as defensive programming for if a control is moved and not known about
            // use of defensive programming and error handling
            try
            {
                // shows the control so it can be manipulated (only visible controls can be moved)
                tblLayout.GetControlFromPosition(fromX, fromY).Show();
                // moves control from (fromX, fromY) to (toX, toY) in the TableLayoutPanel
                tblLayout.Controls.Add(tblLayout.GetControlFromPosition(fromX, fromY), toX, toY);
                // sets the control's visibility to that specified in the parameter
                tblLayout.GetControlFromPosition(toX, toY).Visible = show;
            }
            catch
            {
                Debug.WriteLine("control not found");
            }
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

        // clears the screen ready for adding new Controls
        private void ClearScreen(TableLayoutPanel tblLayout, int xMin = 0, int yMin= 1, int xMax = 5, int yMax = 5)
        {
            // loops through specified (or pre-determined) columns
            // use of nested loops
            for (int x = xMin; x < xMax; x++)
            {
                // loops through specified (or pre-determined) rows
                for (int y = yMin; y < yMax; y++)
                {
                    // removes Control
                    tblLayout.Controls.Remove(tblLayout.GetControlFromPosition(x, y));
                }
            }
            // removes a Return to Menu Button
            tblLayout.Controls.Remove(tblLayout.GetControlFromPosition(2, 5));
        }

        // creates a TextBox and adds it to the TableLayoutPanel
        private TextBox CreateTextBox(TableLayoutPanel tblLayout, string text, string name, int x, int y, int colSpan = 1, int rowSpan = 1, float fontSize = 18.0F, HorizontalAlignment alignment = HorizontalAlignment.Left, bool multiLine = false)
        {
            // creates a temporary Label
            TextBox tempBox = new TextBox();
            // sets text formatting to centre alignment
            tempBox.TextAlign = alignment;
            // allows the label to get larger and smaller as the window does so
            tempBox.AutoSize = true;
            // makes the label stretch as the window changes
            tempBox.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
            // sets the label font
            tempBox.Font = new Font(FontFamily.GenericSansSerif, fontSize, FontStyle.Bold);
            // sets location for the button - the centre of the container (TableLayoutPanel cell)
            tempBox.Location = new Point(this.Width / 2 - 2 * tempBox.Width, 50);
            // sets the text on the label to contain the string from the first parameter
            tempBox.Text = text;
            // sets the name of the label to the string from the second parameter
            tempBox.Name = name;
            // sets whether the texbox can take multiple lines
            tempBox.Multiline = multiLine;
            
            // sets the span of the Label to the number of columns in the optional parameter
            tblLayout.SetColumnSpan(tempBox, colSpan);
            // sets the span of the Label to the number of rows in the optional parameter
            tblLayout.SetRowSpan(tempBox, rowSpan);
            // adds the Label to the TableLayoutPanel
            tblLayout.Controls.Add(tempBox, x, y);
            // returns the Label
            return tempBox;
        }
    }
}
