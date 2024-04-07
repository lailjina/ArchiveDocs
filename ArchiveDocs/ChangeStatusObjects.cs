using ArchiveDocs.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArchiveDocs
{
    public partial class ChangeStatusObjects : Form
    {
        private BindingSource bindingSource;
        private List<TObject> documents;

        public ChangeStatusObjects()
        {
            InitializeComponent();

            // Инициализируем BindingSource
            bindingSource = new BindingSource();

            // Загружаем данные из базы данных или другого источника
            LoadData();

            // Привязываем данные к BindingSource
            bindingSource.DataSource = documents;

            // Привязываем BindingSource к DataGridView
            dataGridView1.DataSource = bindingSource;
            dataGridView1.Columns[0].HeaderText = "№ объекта";
            dataGridView1.Columns[1].HeaderText = "Название";
            dataGridView1.Columns[2].HeaderText = "Адрес";
            dataGridView1.Columns[3].HeaderText = "Статус";
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;

            // Добавляем колонку-кнопку
            DataGridViewButtonColumn buttonColumn = new DataGridViewButtonColumn();
            buttonColumn.Name = "btnChangeStatus";
            buttonColumn.HeaderText = "Отправить в архив";
            buttonColumn.Text = "Архивировать";
            buttonColumn.UseColumnTextForButtonValue = true;
            dataGridView1.Columns.Add(buttonColumn);

            // Обработчик события нажатия на кнопку
            dataGridView1.CellContentClick += DataGridView_CellContentClick;

        }

        private void DataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Проверяем, что событие произошло в колонке кнопки
            if (e.ColumnIndex == dataGridView1.Columns["btnChangeStatus"].Index && e.RowIndex >= 0)
            {
                using (var dbContext = new ADDbContext())
                {
                    var documentsObject = dbContext.Documents
                        .Include(doc => doc.DocumentType)
                        .Include(doc => doc.Category)
                        .Where(doc => doc.ObjectId == int.Parse(dataGridView1.CurrentRow.Cells[1].Value.ToString()))
                        .ToList();

                    foreach (var document in documentsObject)
                    {
                        var documentByID = dbContext.Documents.Find(document.Id_Doc);

                        if (documentByID != null)
                        {
                            documentByID.Archive();
                            dbContext.SaveChanges(); // Сохраняем изменения
                            Console.WriteLine("Документ отправлен в архив!");
                        }
                        else
                        {
                            Console.WriteLine("Документ не найден!");
                        }
                    }

                }

                MessageBox.Show($"Объект и связанные документы отправлены в архив");
                LoadData();
            }
        }

        private void LoadData()
        {
            // Загружаем данные из базы данных или другого источника
            // Пример загрузки данных из базы данных:
            using (var dbContext = new ADDbContext())
            {
                documents = dbContext.Objects.ToList(); // Получаем все записи
            }
        }

        private void ChangeStatusObjects_Load(object sender, EventArgs e)
        {

        }
    }
}
