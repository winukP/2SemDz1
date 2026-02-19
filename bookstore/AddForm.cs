using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Bookstore
{
    public partial class AddForm : Form
    {
        private List<Author> authors;
        private List<Genre> genres;

        public Book NewBook { get; private set; }

        public AddForm(List<Author> authorsList, List<Genre> genresList)
        {
            InitializeComponent();
            authors = authorsList;
            genres = genresList;
            radioButton2.Checked = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
        }

        private void AddForm_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("Введите название книги", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Введите автора", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox3.Text))
            {
                MessageBox.Show("Введите жанр", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            NewBook = new Book
            {
                Title = textBox2.Text.Trim(),
                AuthorName = textBox1.Text.Trim(), 
                GenreName = textBox3.Text.Trim(), 
                AuthorId = 0, 
                GenreId = 0,   
                HasDiscount = radioButton1.Checked 
            };
            DialogResult = DialogResult.OK;
            Close();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}