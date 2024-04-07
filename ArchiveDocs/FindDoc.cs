using ArchiveDocs.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArchiveDocs
{
    public partial class FindDoc : Form
    {
        private BindingSource bindingSource;

        public FindDoc()
        {
            InitializeComponent();
            // Инициализируем BindingSource
            bindingSource = new BindingSource();

            // Загружаем данные из базы данных или другого источника
            LoadData();

            // Привязываем BindingSource к DataGridView
            dataGridView1.DataSource = bindingSource;
            dataGridView1.Columns[0].HeaderText = "№ документа";
            dataGridView1.Columns[1].HeaderText = "Тип";
            dataGridView1.Columns[2].HeaderText = "Категория";
            dataGridView1.Columns[3].HeaderText = "Бизнес-дата";
            dataGridView1.Columns[4].HeaderText = "Объект";
            dataGridView1.Columns[5].HeaderText = "Статус";

            // Добавляем колонку-кнопку
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.Name = "btnExtract";
            buttonColumn.HeaderText = "Выгрузить";
            buttonColumn.Text = "Выгрузить";
            buttonColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(buttonColumn);

            // Обработчик события нажатия на кнопку
            dataGridView1.CellContentClick += DataGridView_CellContentClick;

            using (var dbContext = new ADDbContext())
            {
                // Получите список объектов из базы данных
                List<TObject> objects = dbContext.Objects.ToList();

                // Установите этот список в качестве источника данных для ComboBox
                comboBox1.DataSource = objects;

                // Укажите свойство, которое будет отображаться в ComboBox
                comboBox1.DisplayMember = "Name";

                // Укажите свойство, которое будет использоваться в качестве значения
                comboBox1.ValueMember = "Id_Object";
                comboBox1.SelectedIndex = -1;

            }

        }

        private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что событие произошло в колонке кнопки
            if (e.ColumnIndex == dataGridView1.Columns["btnExtract"].Index && e.RowIndex >= 0)
            {
                using (var dbContext = new ADDbContext())
                {
                    var documentByID = dbContext.Documents.Find(int.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString()));

                    string path = "";

                    using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
                    {
                        DialogResult result = folderBrowserDialog.ShowDialog();

                        if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(folderBrowserDialog.SelectedPath))
                        {
                            path = folderBrowserDialog.SelectedPath;
                        }
                    }

                    documentByID.CopyFile(path);
                    MessageBox.Show("Готово");

                }
            }
        }

        private void LoadData(int idObject = 0)
        {
            using (var dbContext = new ADDbContext())
            {
                if (idObject != 0)
                {
                    var documents = dbContext.Documents
                        .Include(d => d.Category)
                        .Include(d => d.DocumentType)
                        .Where(d => d.ObjectId == idObject)
                        .Select(d => new
                        {
                            Id_Doc = d.Id_Doc,
                            DocumentType = d.DocumentType.DocumentType,
                            Category = d.Category.CategoryName,
                            BusinessDate = d.BusinessDate,
                            Object = d.Object.Name,
                            StatusDoc = d.StatusDoc
                        })
                        .ToList(); // Получаем все записи

                    bindingSource.DataSource = documents;
                }
                else
                {
                    var documents = dbContext.Documents
                        .Include(d => d.Category)
                        .Include(d => d.DocumentType)
                        .Select(d => new
                        {
                            Id_Doc = d.Id_Doc,
                            DocumentType = d.DocumentType.DocumentType,
                            Category = d.Category.CategoryName,
                            BusinessDate = d.BusinessDate,
                            Object = d.Object.Name,
                            StatusDoc = d.StatusDoc
                        })
                        .ToList(); // Получаем все записи

                    bindingSource.DataSource = documents;
                }
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData(int.Parse(comboBox1.SelectedValue.ToString()));
        }

        private void FindDoc_Load(object sender, EventArgs e)
        {

        }
    }
}
