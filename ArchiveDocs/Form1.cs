using ArchiveDocs.Tables;
using ArchiveDocs.FileGenerator;
using Microsoft.EntityFrameworkCore;

namespace ArchiveDocs
{
    public partial class Form1 : Form
    {
        private ADDbContext dbContext;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            dbContext = new ADDbContext();
            txtPath.Text = Environment.CurrentDirectory + "\\TestFiles\\";

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

        private void button1_Click(object sender, EventArgs e)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ADDbContext>();
            optionsBuilder.UseNpgsql("PostgreSQLConnection");

            using var dbContext = new ADDbContext(optionsBuilder.Options);

            var generator = new FileGenerator.FileGenerator(dbContext);

            // Генерация 10000 файлов по 1 МБ
            generator.GenerateFiles(10000, 1024 * 1024);

            // Генерация 10000 файлов по 10 КБ
            generator.GenerateFiles(10000, 10 * 1024);

            // Генерация 10 файлов по 1 МБ с дубликатами
            generator.GenerateFiles(10, 1024 * 1024, true);

            MessageBox.Show("Генерация выполнена!");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = Environment.CurrentDirectory + "\\TestFiles\\";

                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    txtPath.Text = dialog.SelectedPath;
                }
                else
                {
                    Console.WriteLine("Отменено или не выбрана директория.");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("Укажите объект!");
                comboBox1.Focus();
            }

            int IDSelectObject = int.Parse(comboBox1.SelectedValue.ToString());

            try
            {
                string[] files = Directory.GetFiles(txtPath.Text);

                foreach (string file in files)
                {
                    string pathToFile = file;
                    string fileName = Path.GetFileName(file);

                    string hashStreebogFile = HashCode.GetCRC32Hash(pathToFile);

                    using (var dbContext = new ADDbContext())
                    {
                        var document = dbContext.Documents.FirstOrDefault(doc => doc.HashSum == hashStreebogFile);
                        if (document != null)
                        {
                            listBox1.Items.Add($"{fileName} уже есть в БД! (дубль - {document.Id_Doc})");
                        }
                        else
                        {
                            int firstIndex = fileName.IndexOf('_');
                            int secondIndex = fileName.IndexOf('_', firstIndex + 1);

                            string typeDoc = fileName.Substring(0, firstIndex);
                            int typeDocID = dbContext.Nomenclatures.FirstOrDefault(n => n.DocumentType == typeDoc).DocumentTypeId;

                            string categoryDoc = fileName.Substring(firstIndex + 1, secondIndex - firstIndex - 1);
                            int categoryDocID = dbContext.Categories.FirstOrDefault(cat => cat.CategoryName == categoryDoc).CategoryId;
                            string dateTimeDocStr = fileName.Substring(fileName.LastIndexOf('_') + 1, fileName.LastIndexOf('.') - fileName.LastIndexOf('_') - 1);
                            DateTime dateTimeDoc = DateTime.ParseExact(dateTimeDocStr, "yyyyMMddHHmmssfff", null, System.Globalization.DateTimeStyles.None);

                            string destinationFilePath = Path.Combine(Environment.CurrentDirectory + "\\Storage\\", fileName); // Путь к скопированному файлу
                            File.Copy(pathToFile, destinationFilePath, true);

                            TDocument newDocument = new TDocument
                            {
                                CategoryId = categoryDocID,
                                DocumentTypeId = typeDocID,
                                BusinessDate = DateTime.SpecifyKind(dateTimeDoc, DateTimeKind.Utc),
                                HashSum = hashStreebogFile,
                                FilePath = destinationFilePath,
                                StatusDoc = DocumentStatus.Действующий,
                                ObjectId = IDSelectObject,
                            };

                            dbContext.Documents.Add(newDocument);
                            dbContext.SaveChanges(); // Сохранение изменений в базе данных

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void удалениеИстекшихДокументовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int countDestrouDoc = 0;

            using (var dbContext = new ADDbContext())
            {
                var documentsObject = dbContext.Documents
                    .Include(doc => doc.DocumentType)
                    .Include(doc => doc.Category)
                    .ToList();

                foreach (var document in documentsObject)
                {
                    if (document.HranitDo < DateTime.Now)
                    {
                        document.Destroy();
                        dbContext.SaveChanges(); // Сохраняем изменения
                        Console.WriteLine($"Документ {document.Id_Doc} уничтожен!");
                        countDestrouDoc++;
                    }
                }

                MessageBox.Show($"Всего отправлено в архив {countDestrouDoc} документов");
            }
        }

        private void поискДокументаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindDoc f = new FindDoc();
            f.ShowDialog();
        }

        private void изменениеСтатусаОбъектовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeStatusObjects f = new ChangeStatusObjects();
            f.ShowDialog();
        }
    }
}