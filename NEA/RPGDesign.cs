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
        private void DesignRPG(TableLayoutPanel tblLayout)
        {

            /* this method changes the title to "Design RPG" */
            ModifyTitle(tblLayout, "Design RPG");

            HideStandardButtons(tblLayout);

            List<Tuple<string, int, int>> DirectionalTextBoxes = CreateButtonTuples();

            ShowDirectionalTextBoxes(tblLayout, DirectionalTextBoxes);

            //tblLayout.Controls.Add(CreateLabel(Rooms[currentRoom].GetName(), "lblCurrentRoom"), 2, 3);
        }

        private void ShowDirectionalTextBoxes(TableLayoutPanel tblLayout, List<Tuple<string, int, int>> DirectionalTextBoxes)
        {
            foreach (Tuple<string, int, int> tuple in DirectionalTextBoxes)
            {
                // creates a temporary TextBox with text as the direction
                TextBox tempBox = new TextBox();
                tempBox.Text = tuple.Item1;
                tempBox.Anchor = (AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom);
                // adds the button to tblLayout at the x/y coordinates stored in the tuple
                tblLayout.Controls.Add(tempBox, tuple.Item2, tuple.Item3);
            }
        }

    }
}
