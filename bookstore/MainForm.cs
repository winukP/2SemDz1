using Bookstore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using Bookstore.Models;

namespace bookstore
{
    public partial class MainForm : Form
    {
        private List<Book> books;
        private List<Author> authors;
        private List<Genre> genres;
        private Book selectedBook;

        public MainForm()
        {
            InitializeComponent();
            LoadDataFromFiles();
            SetupDataGridView();
            UpdateBooksList();
        }

        private void LoadDataFromFiles()
        {
            authors = new List<Author>();
            genres = new List<Genre>();
            books = new List<Book>();
            string data = @"..\..\Files\Authors.txt";
            if (File.Exists(data))
            {
                string[] authorLines = File.ReadAllLines(data);
                foreach (string line in authorLines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    string[] parts = line.Split(new[] { ". " }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        authors.Add(new Author
                        {
                            Id = int.Parse(parts[0]),
                            Name = parts[1]
                        });
                    }
                }
            }
            string data1 = @"..\..\Files\Genres.txt";
            if (File.Exists(data1))
            {
                string[] genreLines = File.ReadAllLines(data1);
                foreach (string line in genreLines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split(new[] { ". " }, StringSplitOptions.None);
                    if (parts.Length == 2)
                    {
                        genres.Add(new Genre
                        {
                            Id = int.Parse(parts[0]),
                            Name = parts[1]
                        });
                    }
                }
            }
            string data2 = @"..\..\Files\Books.txt";
            if (File.Exists(data2))
            {
                string[] bookLines = File.ReadAllLines(data2);
                foreach (string line in bookLines)
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    string[] parts = line.Split(new[] { ", " }, StringSplitOptions.None);
                    if (parts.Length == 4)
                    {
                        string[] idTitle = parts[0].Split(new[] { ". " }, StringSplitOptions.None);
                        if (idTitle.Length == 2)
                        {
                            int id = int.Parse(idTitle[0]);
                            string title = idTitle[1];
                            int authorId = int.Parse(parts[1]);
                            int genreId = int.Parse(parts[2]);
                            bool hasDiscount = bool.Parse(parts[3].ToLower());

                            Book book = new Book
                            {
                                Id = id,
                                Title = title,
                                AuthorId = authorId,
                                GenreId = genreId,
                                HasDiscount = hasDiscount,
                                AuthorName = GetAuthorName(authorId),
                                GenreName = GetGenreName(genreId)
                            };
                            books.Add(book);
                        }
                    }
                }
            }
        }

        private string GetAuthorName(int authorId)
        {
            var author = authors.FirstOrDefault(a => a.Id == authorId);
            return author?.Name ?? "Неизвестно";
        }

        private string GetGenreName(int genreId)
        {
            var genre = genres.FirstOrDefault(g => g.Id == genreId);
            return genre?.Name ?? "Неизвестно";
        }

        private void SetupDataGridView()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            DataGridViewTextBoxColumn idColumn = new DataGridViewTextBoxColumn();
            idColumn.DataPropertyName = "Id";
            idColumn.HeaderText = "ID";
            idColumn.Width = 50;
            idColumn.ReadOnly = true;
            dataGridView1.Columns.Add(idColumn);

            DataGridViewTextBoxColumn authorColumn = new DataGridViewTextBoxColumn();
            authorColumn.DataPropertyName = "AuthorName";
            authorColumn.HeaderText = "Автор";
            authorColumn.Width = 150;
            dataGridView1.Columns.Add(authorColumn);

            DataGridViewTextBoxColumn titleColumn = new DataGridViewTextBoxColumn();
            titleColumn.DataPropertyName = "Title";
            titleColumn.HeaderText = "Название";
            titleColumn.Width = 200;
            dataGridView1.Columns.Add(titleColumn);

            DataGridViewTextBoxColumn genreColumn = new DataGridViewTextBoxColumn();
            genreColumn.DataPropertyName = "GenreName";
            genreColumn.HeaderText = "Жанр";
            genreColumn.Width = 150;
            dataGridView1.Columns.Add(genreColumn);

            DataGridViewTextBoxColumn discountColumn = new DataGridViewTextBoxColumn();
            discountColumn.DataPropertyName = "DiscountText";
            discountColumn.HeaderText = "Скидка";
            discountColumn.Width = 80;
            dataGridView1.Columns.Add(discountColumn);

            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.MultiSelect = false;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionChanged += DataGridView1_SelectionChanged;
        }

        private void DataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int index = dataGridView1.SelectedRows[0].Index;
                if (index >= 0 && index < books.Count)
                {
                    selectedBook = books[index];
                    UpdateCardPanel();
                }
            }
        }

        private void UpdateCardPanel()
        {
            if (selectedBook != null)
            {
                textBox1.Text = selectedBook.AuthorName;
                textBox2.Text = selectedBook.Title;
                textBox3.Text = selectedBook.GenreName;

                if (selectedBook.HasDiscount)
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false; 
                }
                else
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }
            }
        }

        private void UpdateBooksList()
        {
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = books;
        }

        private void UpdateBookInList(Book updatedBook)
        {
            var existingBook = books.FirstOrDefault(b => b.Id == updatedBook.Id);
            if (existingBook != null)
            {
                existingBook.Title = updatedBook.Title;
                existingBook.HasDiscount = updatedBook.HasDiscount;
                existingBook.AuthorName = updatedBook.AuthorName;
                existingBook.GenreName = updatedBook.GenreName;
            }
            dataGridView1.Refresh();
            if (selectedBook != null && selectedBook.Id == updatedBook.Id)
            {
                selectedBook = existingBook;
                UpdateCardPanel();
            }
        }

        private void добавитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddForm addForm = new AddForm(authors, genres);
            if (addForm.ShowDialog() == DialogResult.OK)
            {
                Book newBook = addForm.NewBook;
                newBook.Id = books.Count > 0 ? books.Max(b => b.Id) + 1 : 1;
                books.Add(newBook);
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = books;
                MessageBox.Show("Книга успешно добавлена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу для редактирования", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ModifyForm modifyForm = new ModifyForm(selectedBook, authors, genres);
            if (modifyForm.ShowDialog() == DialogResult.OK)
            {
                Book editedBook = modifyForm.EditedBook;
                UpdateBookInList(editedBook);
                MessageBox.Show("Книга успешно изменена!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedBook == null)
            {
                MessageBox.Show("Выберите книгу для удаления", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DeleteForm deleteForm = new DeleteForm();
            if (deleteForm.ShowDialog() == DialogResult.OK && deleteForm.Confirmed)
            {
                books.Remove(selectedBook);
                selectedBook = null;
                UpdateBooksList();
                textBox1.Clear();
                textBox2.Clear();
                textBox3.Clear();
                radioButton2.Checked = true;
            }
        }

        private void выйтиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.добавитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.изменитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.выйтиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.добавитьToolStripMenuItem,
            this.изменитьToolStripMenuItem,
            this.удалитьToolStripMenuItem,
            this.выйтиToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1288, 28);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // добавитьToolStripMenuItem
            // 
            this.добавитьToolStripMenuItem.Name = "добавитьToolStripMenuItem";
            this.добавитьToolStripMenuItem.Size = new System.Drawing.Size(90, 24);
            this.добавитьToolStripMenuItem.Text = "Добавить";
            // 
            // изменитьToolStripMenuItem
            // 
            this.изменитьToolStripMenuItem.Name = "изменитьToolStripMenuItem";
            this.изменитьToolStripMenuItem.Size = new System.Drawing.Size(92, 24);
            this.изменитьToolStripMenuItem.Text = "Изменить";
            // 
            // удалитьToolStripMenuItem
            // 
            this.удалитьToolStripMenuItem.Name = "удалитьToolStripMenuItem";
            this.удалитьToolStripMenuItem.Size = new System.Drawing.Size(79, 24);
            this.удалитьToolStripMenuItem.Text = "Удалить";
            this.удалитьToolStripMenuItem.Click += new System.EventHandler(this.удалитьToolStripMenuItem_Click);
            // 
            // выйтиToolStripMenuItem
            // 
            this.выйтиToolStripMenuItem.Name = "выйтиToolStripMenuItem";
            this.выйтиToolStripMenuItem.Size = new System.Drawing.Size(67, 24);
            this.выйтиToolStripMenuItem.Text = "Выйти";
            this.выйтиToolStripMenuItem.Click += new System.EventHandler(this.выйтиToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 31);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(601, 443);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(657, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(194, 46);
            this.label1.TabIndex = 2;
            this.label1.Text = "Карточка";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(620, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 25);
            this.label3.TabIndex = 4;
            this.label3.Text = "Автор";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(730, 105);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(506, 22);
            this.textBox1.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(620, 147);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 25);
            this.label4.TabIndex = 6;
            this.label4.Text = "Название";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(730, 150);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(506, 22);
            this.textBox2.TabIndex = 7;
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(730, 202);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(506, 22);
            this.textBox3.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(620, 198);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 25);
            this.label5.TabIndex = 9;
            this.label5.Text = "Жанр";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(620, 254);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(233, 25);
            this.label6.TabIndex = 10;
            this.label6.Text = "Информация о скидках";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(625, 300);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(173, 20);
            this.radioButton1.TabIndex = 11;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Скидка для студентов";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(625, 341);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(101, 20);
            this.radioButton2.TabIndex = 12;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Нет скидки";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1288, 475);
            this.Controls.Add(this.radioButton2);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "bookstore";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
            this.добавитьToolStripMenuItem.Click += new System.EventHandler(this.добавитьToolStripMenuItem_Click);
            this.изменитьToolStripMenuItem.Click += new System.EventHandler(this.изменитьToolStripMenuItem_Click);
        }
        
    }
}
