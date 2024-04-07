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
                // �������� ������ �������� �� ���� ������
                List<TObject> objects = dbContext.Objects.ToList();

                // ���������� ���� ������ � �������� ��������� ������ ��� ComboBox
                comboBox1.DataSource = objects;

                // ������� ��������, ������� ����� ������������ � ComboBox
                comboBox1.DisplayMember = "Name";

                // ������� ��������, ������� ����� �������������� � �������� ��������
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

            // ��������� 10000 ������ �� 1 ��
            generator.GenerateFiles(10000, 1024 * 1024);

            // ��������� 10000 ������ �� 10 ��
            generator.GenerateFiles(10000, 10 * 1024);

            // ��������� 10 ������ �� 1 �� � �����������
            generator.GenerateFiles(10, 1024 * 1024, true);

            MessageBox.Show("��������� ���������!");
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
                    Console.WriteLine("�������� ��� �� ������� ����������.");
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
            {
                MessageBox.Show("������� ������!");
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
                            listBox1.Items.Add($"{fileName} ��� ���� � ��! (����� - {document.Id_Doc})");
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

                            string destinationFilePath = Path.Combine(Environment.CurrentDirectory + "\\Storage\\", fileName); // ���� � �������������� �����
                            File.Copy(pathToFile, destinationFilePath, true);

                            TDocument newDocument = new TDocument
                            {
                                CategoryId = categoryDocID,
                                DocumentTypeId = typeDocID,
                                BusinessDate = DateTime.SpecifyKind(dateTimeDoc, DateTimeKind.Utc),
                                HashSum = hashStreebogFile,
                                FilePath = destinationFilePath,
                                StatusDoc = DocumentStatus.�����������,
                                ObjectId = IDSelectObject,
                            };

                            dbContext.Documents.Add(newDocument);
                            dbContext.SaveChanges(); // ���������� ��������� � ���� ������

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������: {ex.Message}");
            }
        }

        private void ��������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
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
                        dbContext.SaveChanges(); // ��������� ���������
                        Console.WriteLine($"�������� {document.Id_Doc} ���������!");
                        countDestrouDoc++;
                    }
                }

                MessageBox.Show($"����� ���������� � ����� {countDestrouDoc} ����������");
            }
        }

        private void ��������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FindDoc f = new FindDoc();
            f.ShowDialog();
        }

        private void ������������������������ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeStatusObjects f = new ChangeStatusObjects();
            f.ShowDialog();
        }
    }
}