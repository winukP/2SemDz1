using Bookstore.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Bookstore
{
    public partial class ModifyForm : Form
    {
        private List<Author> authors;
        private List<Genre> genres;
        private Book currentBook;

        public Book EditedBook { get; private set; }

        public ModifyForm(Book bookToEdit, List<Author> authorsList, List<Genre> genresList)
        {
            InitializeComponent();
            authors = authorsList;
            genres = genresList;
            currentBook = bookToEdit;

            LoadBookData();
        }

        private void ModifyForm_Load(object sender, EventArgs e)
        {
            
        }

        private void LoadBookData()
        {
            textBox1.Text = currentBook.AuthorName;
            textBox2.Text = currentBook.Title;      
            textBox3.Text = currentBook.GenreName;  

            if (currentBook.HasDiscount)
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
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

            EditedBook = new Book
            {
                Id = currentBook.Id,
                Title = textBox2.Text.Trim(),
                AuthorId = currentBook.AuthorId,
                GenreId = currentBook.GenreId,  
                HasDiscount = radioButton1.Checked, 
                AuthorName = textBox1.Text.Trim(), 
                GenreName = textBox3.Text.Trim() 
            };
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
