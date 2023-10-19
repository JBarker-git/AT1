using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WikiForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // Global Variables
        static int row = 12;
        static int column = 4;
        string[,] wikiArray = new string[row, column];

        private void updateListView()
        {
            listView.Items.Clear();
            for (int row = 0;  row < wikiArray.GetLength(0); row++)
            {
                if (!string.IsNullOrEmpty(wikiArray[row, 0]))
                {
                    ListViewItem item = new ListViewItem(wikiArray[row, 0]);
                    item.SubItems.Add(wikiArray[row, 1]);
                    listView.Items.Add(item);
                }
            }
        }

        private void clearTextBoxes()
        {
            textBoxName.Text = string.Empty;
            textBoxCategory.Text = string.Empty;
            textBoxStructure.Text = string.Empty;
            textBoxDefinition.Text = string.Empty;
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            string name = textBoxName.Text;
            string category = textBoxCategory.Text;
            string structure = textBoxStructure.Text;
            string definition = textBoxDefinition.Text;

            for (int row = 0; row < wikiArray.GetLength(0); row++)
            {
                if (string.IsNullOrEmpty(wikiArray[row, 0]))
                {
                    wikiArray[row, 0] = name;
                    wikiArray[row, 1] = category;
                    wikiArray[row, 2] = structure;
                    wikiArray[row, 3] = definition;
                    break;
                }
            }
            clearTextBoxes();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            clearTextBoxes();
        }
    }
}
