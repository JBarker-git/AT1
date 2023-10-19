using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
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

        private void buttonAdd_Click(object sender, EventArgs e)
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
            bubbleSortByName();
            updateListView();
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

                bubbleSortByName();
                updateListView();
                clearTextBoxes();
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

                    bubbleSortByName();
                    updateListView();
                    clearTextBoxes();
                }
            }
        }


        private void bubbleSortByName()
        {
            int rows = wikiArray.GetLength(0);

            for (int i = 0; i < rows - 1; i++)
            {
                for (int j = 0; j < rows - i - 1; j++)
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

        private void selectFromListView()
        {
            if (listView.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView.SelectedItems[0];
                int selectedIndex = listView.Items.IndexOf(selectedItem);

                textBoxName.Text = wikiArray[selectedIndex, 0];
                textBoxCategory.Text = wikiArray[selectedIndex, 1];
                textBoxStructure.Text = wikiArray[selectedIndex, 2];
                textBoxDefinition.Text = wikiArray[selectedIndex, 3];

            }    
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string searchName = textBoxSearch.Text;
            int result = binarySearchByName(searchName);
            if (result != -1)
            {
                textBoxName.Text = wikiArray[result, 0];
                textBoxStructure.Text = wikiArray[result, 1];
                textBoxCategory.Text = wikiArray[result, 2];
                textBoxDefinition.Text = wikiArray[result, 3];
            }
            else
            {
                MessageBox.Show("Item not found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBoxSearch.Text = string.Empty;
            }
        }

        private int binarySearchByName(string name)
        {
            int left = 0;
            int right = wikiArray.GetLength(0) - 1;

            while (left <= right)
            {
                int mid = left + (right - left) / 2;
                if (wikiArray[mid, 0] == name)
                {
                    return mid; //Found Name
                }
                else if (string.Compare(name, wikiArray[mid, 0], StringComparison.Ordinal) > 0)
                {
                    right = mid - 1; //Name is in left half
                }
                else
                {
                    left = mid + 1; //Name is in right half
                }
            }

            return -1;
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Binary Files (*.dat)|*.dat|All Files (*.*)|*.*";
            openFileDialog.FileName = "definitions.dat";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName;
                try
                {
                    loadDataFromBinaryFile(filePath);
                    updateListView();
                    MessageBox.Show("Data loaded", "Load Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading data: " + ex.Message, "Load Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void loadDataFromBinaryFile(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    int rows = binaryReader.ReadInt32();
                    int columns = binaryReader.ReadInt32();

                    if (rows != 12 || columns != 4)
                    {
                        MessageBox.Show("Invalid data format. Please select a valid data file.", "Load Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    wikiArray = new string[12, 4];
                    for (int row = 0; row < rows; row++)
                    {
                        for (int column = 0; column < columns; column++)
                        {
                            wikiArray[row, column] = binaryReader.ReadString();
                        }
                    }
                }
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Binary Files (*.dat)|*.dat|All Files (*.*)|*.*";
            saveFileDialog.FileName = "definitions.dat";

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = saveFileDialog.FileName;
                try
                {
                    saveDataToBinaryFile(filePath);
                    MessageBox.Show("Data saved", "Save Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving data: " + ex.Message, "Save Data Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
            }
        }

        private void saveDataToBinaryFile(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    binaryWriter.Write(wikiArray.GetLength(0));
                    binaryWriter.Write(wikiArray.GetLength(1));
                    
                    for (int row = 0; row < wikiArray.GetLength(0); row++)
                    {
                        for (int column = 0; column < wikiArray.GetLength(1); column++)
                        {
                            binaryWriter.Write(wikiArray[row, column]);
                        }
                    }
                }
            }
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectFromListView();
        }
    }
}
