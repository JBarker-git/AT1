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
                    updateListView();
                    break;
                }
            }
            clearTextBoxes();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            clearTextBoxes();
        }

        private void buttonEdit_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView.SelectedItems[0];
                int selectedIndex = listView.Items.IndexOf(selectedItem);

                String name = textBoxName.Text;
                String category = textBoxCategory.Text;
                String structure = textBoxStructure.Text;
                String definition = textBoxDefinition.Text;

                wikiArray[selectedIndex, 0] = name;
                wikiArray[selectedIndex, 1] = category;
                wikiArray[selectedIndex, 2] = structure;
                wikiArray[selectedIndex, 3] = definition;

                updateListView();
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView.SelectedItems[0];
                int selectedIndex = listView.Items.IndexOf(selectedItem);

                DialogResult result = MessageBox.Show("Are you sure you want to delete this item?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    listView.Items.Remove(selectedItem);

                    for (int i = 0; i < 4; i++)
                    {
                        wikiArray[selectedIndex, i] = null;
                    }

                    clearTextBoxes();
                }
            }
        }


        private void BubbleSortByName()
        {
            int rows = wikiArray.GetLength(0);

            for (int i = 0; i < rows - 1; i++)
            {
                for (int j = 0; j < rows - i - j; j++)
                {
                    string name1 = wikiArray[j, 0];
                    string name2 = wikiArray[j + 1, 0];
                    
                    if (string.Compare(name1, name2, StringComparison.Ordinal) > 0)
                    {
                        swapRows(j, j + 1);
                    }
                }
            }
        }

        private void swapRows(int row1, int row2)
        {
            for (int col = 0; col < 4; col++)
            {
                string temp = wikiArray[row1, col];
                wikiArray[row1, col] = wikiArray[row2, col];
                wikiArray[row2, col] = temp;
            }
        }
    }
}
