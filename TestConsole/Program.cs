using ArchiveDocs.Tables;
using ArchiveDocs;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.EntityFrameworkCore;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;
using ArchiveDocs.FileGenerator;
using BenchmarkDotNet.Running;

class Program
{
    public static string PathForDirectory = Environment.CurrentDirectory + "\\TestFiles\\";

    public static void ChangePath()
    {
        Console.WriteLine($"Текущий каталог: {PathForDirectory}");
        Console.Write("Вы хотите указать другой каталог? (Д/Н): ");
        string userChoice = Console.ReadLine();

        if (userChoice.ToLower() == "д")
        {
            Console.Write("Введите новый путь к каталогу: ");
            PathForDirectory = Console.ReadLine();
        }

        Console.Write("Вы хотите сгенерировать файлы? (Д/Н): ");
        userChoice = Console.ReadLine();

        if (userChoice.ToLower() == "д")
        {
            var optionsBuilder = new DbContextOptionsBuilder<ADDbContext>();
            optionsBuilder.UseNpgsql("PostgreSQLConnection");

            using var dbContext = new ADDbContext(optionsBuilder.Options);

            var generator = new FileGenerator(dbContext);

            // Генерация 10000 файлов по 1 МБ
            generator.GenerateFiles(10000, 1024 * 1024);

            // Генерация 10000 файлов по 10 КБ
            generator.GenerateFiles(10000, 10 * 1024);

            // Генерация 10 файлов по 1 МБ с дубликатами
            generator.GenerateFiles(10, 1024 * 1024, true);

            Console.WriteLine("Генерация выполнена!");
        }
        Console.WriteLine("Выполняется хеширование!");
        HashFiles();
        Console.WriteLine("Выполняется поиск дублей по хешу!");
        FindDupHash();
        Console.ReadLine();
    }


    public static void HashFiles()
    {
        string[] files = Directory.GetFiles(PathForDirectory);
        int index = 0;

        DateTime startHash = DateTime.Now;
        foreach (string file in files)
        {
            string pathToFile = file;
            ArchiveDocs.HashCode.GetStreebogHash(pathToFile);
            index++;
        }
        DateTime finishHash = DateTime.Now;
        TimeSpan StreebogTime = finishHash - startHash;
        Console.WriteLine($"Всего файлов захешировано Streebog - {index} за {StreebogTime}");

        index = 0;
        startHash = DateTime.Now;
        foreach (string file in files)
        {
            string pathToFile = file;
            ArchiveDocs.HashCode.GetCRC32Hash(pathToFile);
            index++;
        }
        finishHash = DateTime.Now;
        TimeSpan CRC32Time = finishHash - startHash;
        Console.WriteLine($"Всего файлов захешировано CRC32 - {index} за {CRC32Time}");

        index = 0;
        startHash = DateTime.Now;
        foreach (string file in files)
        {
            string pathToFile = file;
            ArchiveDocs.HashCode.GetMD5Hash(pathToFile);
            index++;
        }
        finishHash = DateTime.Now;
        TimeSpan MD5Time = finishHash - startHash;
        Console.WriteLine($"Всего файлов захешировано MD5 - {index} за {MD5Time}");

        index = 0;
        startHash = DateTime.Now;
        foreach (string file in files)
        {
            string pathToFile = file;
            ArchiveDocs.HashCode.GetMurmurHash(pathToFile);
            index++;
        }
        finishHash = DateTime.Now;
        TimeSpan MurmurTime = finishHash - startHash;
        Console.WriteLine($"Всего файлов захешировано Murmur - {index} за {MurmurTime}");
    }

    public static void HashFilesBench()
    {
        BenchmarkRunner.Run<HashCodeBench>();
    }

    public static void ClearHash()
    {
        using (var dbContext = new ADDbContext())
        {
            var documents = dbContext.Documents.ToList(); // Получаем все записи

            foreach (var document in documents)
            {
                document.HashSum = null; // Устанавливаем значение HashSum как null
            }

            dbContext.SaveChanges(); // Сохраняем изменения в БД

            Console.WriteLine($"Готово! Обработано  - {documents.Count}");
        }
    }

    public static void ObjectsWork()
    {
        using (var dbContext = new ADDbContext())
        {
            var documents = dbContext.Objects.ToList(); // Получаем все записи

            Console.WriteLine("===================================================");
            Console.WriteLine("ID\tНазвание\tАдрес\tСтатус");
            foreach (var document in documents)
            {
                Console.WriteLine($"{document.Id_Object}\t{document.Name}\t{document.Address}\t{document.StatusObject}");
            }

            Console.WriteLine($"Всего записей  - {documents.Count}");
            Console.WriteLine("===================================================");

            Console.WriteLine("Действия:");
            Console.WriteLine("1 - Добавить");
            //Console.WriteLine("2 - Изменить");
            Console.WriteLine("3 - Удалить");
            Console.WriteLine("4 - Назад");

            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    TObject obj = new TObject();
                    Console.WriteLine("Укажите название:");
                    obj.Name = Console.ReadLine();
                    Console.WriteLine("Укажите Адрес:");
                    obj.Address = Console.ReadLine();
                    obj.StatusObject = ObjectStatus.Действующий;

                    dbContext.Objects.Add(obj);
                    dbContext.SaveChanges();

                    break;
                case "2":
                    break;
                case "3":
                    Console.WriteLine("Укажите ID:");

                    int objectIdToDelete =int.Parse(Console.ReadLine() ?? "0"); // Замените на реальный ID
                    var objectToDelete = dbContext.Objects.FirstOrDefault(obj => obj.Id_Object == objectIdToDelete);

                    if (objectToDelete != null)
                    {
                        dbContext.Objects.Remove(objectToDelete); // Удаляем объект из контекста БД
                        dbContext.SaveChanges(); // Сохраняем изменения
                        Console.WriteLine("Объект успешно удален.");
                    }
                    else
                    {
                        Console.WriteLine("Объект с указанным ID не найден.");
                    }
                    break;
                case "4":
                    return;
                default:
                    Console.WriteLine("Некорректный выбор. Попробуйте еще раз.");
                    Console.ReadLine(); // Для остановки программы после некорректного ввода
                    break;
            }

        }
    }

    public static void ViewDocsByObject()
    {
        using (var dbContext = new ADDbContext())
        {
            var documents = dbContext.Objects.ToList(); // Получаем все записи

            Console.WriteLine("===================================================");
            Console.WriteLine("ID\tНазвание\tАдрес\tСтатус");
            foreach (var document in documents)
            {
                Console.WriteLine($"{document.Id_Object}\t{document.Name}\t{document.Address}\t{document.StatusObject}");
            }

            Console.WriteLine($"Всего записей  - {documents.Count}");
            Console.WriteLine("===================================================");

            Console.WriteLine("Укажите ID объекта:");
            string choice = Console.ReadLine();

            var documentsObject = dbContext.Documents
                .Include(doc => doc.DocumentType)
                .Include(doc => doc.Category)
                .Where(doc => doc.ObjectId == int.Parse(choice))
                .ToList();

            Console.WriteLine("===================Документы объекта===========================");
            Console.WriteLine("Номер документа\tТип документа\tКатегория\tБизнес-дата\tСсылка на файл\tСтатус документа\tХранить до");
            foreach (var document in documentsObject)
            {
                Console.WriteLine($"{document.Id_Doc}\t{document.DocumentType.DocumentType}\t{document.Category.CategoryName}\t{document.BusinessDate}\t{document.FilePath}\t{document.StatusDoc}\t{document.HranitDo}");
            }

            Console.WriteLine($"Всего записей  - {documentsObject.Count}");
            Console.WriteLine("===================================================");

            Console.WriteLine($"Для отправки объекта и связанных документов в архив нажмите А");
            string ar = Console.ReadLine();
            if (ar == "A" || ar == "А")
            {
                var obj = dbContext.Objects.Find(int.Parse(choice));
                obj.Archive();
                dbContext.SaveChanges();
                Console.WriteLine("Объект и документы отправлены в архив!");
            }


        }
    }

    static void AddData(XmlDocument xmlDoc, XmlElement parentElement, string id, string document, string duplicate)
    {
        XmlElement dataElement = xmlDoc.CreateElement("data");

        XmlElement idElement = xmlDoc.CreateElement("ID");
        idElement.InnerText = id;
        dataElement.AppendChild(idElement);

        XmlElement documentElement = xmlDoc.CreateElement("Наименование файла");
        documentElement.InnerText = document;
        dataElement.AppendChild(documentElement);

        XmlElement duplicateElement = xmlDoc.CreateElement("Дубль “номер карточки”");
        duplicateElement.InnerText = duplicate;
        dataElement.AppendChild(duplicateElement);

        parentElement.AppendChild(dataElement);
    }

    public static void LoadDocs()
    {
        using (var dbContext = new ADDbContext())
        {
            var documents = dbContext.Objects.ToList(); // Получаем все записи

            Console.WriteLine("===================================================");
            Console.WriteLine("ID\tНазвание\tАдрес\tСтатус");
            foreach (var document in documents)
            {
                Console.WriteLine($"{document.Id_Object}\t{document.Name}\t{document.Address}\t{document.StatusObject}");
            }

            Console.WriteLine($"Всего записей  - {documents.Count}");
            Console.WriteLine("===================================================");

            Console.WriteLine("Укажите ID объекта:");
            string idObject = Console.ReadLine();

            Console.WriteLine("Укажите путь к директории с файлами:");
            string path = Console.ReadLine();

            try
            {
                // Путь к файлу XML
                string filePath = Environment.CurrentDirectory+"\\LogExtract"+DateTime.Now.ToString("ddMMyyyyHHmmss")+".xsl";

                XslDocumentBuilder xslBuilder = new XslDocumentBuilder();


                int foundDuplicate = 0;

                string[] files = Directory.GetFiles(path);

                foreach (string file in files)
                {
                    string pathToFile = file;
                    string fileName = Path.GetFileName(file);

                    DateTime startdtS = DateTime.Now;
                    string hashStreebogFile = ArchiveDocs.HashCode.GetStreebogHash(pathToFile);
                    DateTime finishdtS = DateTime.Now;
                    TimeSpan timeStreebog = finishdtS - startdtS;

                    DateTime startdtCRC = DateTime.Now;
                    string hashCRCFile = ArchiveDocs.HashCode.GetCRC32Hash(pathToFile);
                    DateTime finishdtCRC = DateTime.Now;
                    TimeSpan timeCRC = finishdtCRC - startdtCRC;

                    DateTime startdtMD5 = DateTime.Now;
                    string hashMD5File = ArchiveDocs.HashCode.GetMD5Hash(pathToFile);
                    DateTime finishdtMD5 = DateTime.Now;
                    TimeSpan timeMD5 = finishdtMD5 - startdtMD5;

                    string hashSum = hashStreebogFile;
                    TimeSpan minTime = TimeSpan.MinValue;

                    if (timeStreebog < minTime || minTime == TimeSpan.MinValue)
                    {
                        hashSum = hashStreebogFile;
                    }

                    if (timeCRC < minTime || minTime == TimeSpan.MinValue)
                    {
                        hashSum = hashCRCFile;
                    }

                    if (timeMD5 < minTime || minTime == TimeSpan.MinValue)
                    {
                        hashSum = hashMD5File;
                    }

                    var document = dbContext.Documents.FirstOrDefault(doc =>
                            doc.HashSum == hashStreebogFile ||
                            doc.HashSum == hashCRCFile ||
                            doc.HashSum == hashMD5File);
                    if (document != null)
                    {
                        xslBuilder.AddRow(document.DocumentTypeId.ToString(), Path.GetFileNameWithoutExtension(document.FilePath), $"Дубль {document.Id_Doc}");
                        foundDuplicate++;
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
                            HashSum = hashSum,
                            FilePath = destinationFilePath,
                            StatusDoc = DocumentStatus.Действующий,
                        };

                        dbContext.Documents.Add(newDocument);
                        dbContext.SaveChanges(); // Сохранение изменений в базе данных

                        xslBuilder.AddRow(newDocument.DocumentTypeId.ToString(), Path.GetFileNameWithoutExtension(newDocument.FilePath), $"Успех");
                    }
                }

                // Сохранение файла
                xslBuilder.SaveAndClose(filePath);
                // Применение XSLT-преобразования


                Console.WriteLine($"Все файлы обработаны, найдено {foundDuplicate} дублей");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }


        }
    }
    static void TransformXml(string inputXmlPath, string xsltFilePath)
    {
        // Путь к файлу результирующего XML-документа
        string resultXmlPath = "путь_к_результирующему_файлу.xml";

        // Создание XslCompiledTransform и выполнение преобразования
        XslCompiledTransform xslt = new XslCompiledTransform();
        xslt.Load(xsltFilePath);
        xslt.Transform(inputXmlPath, resultXmlPath);
    }

    public static void ExtractDoc()
    {
        using (var dbContext = new ADDbContext())
        {
            var documentsObject = dbContext.Documents
                .Include(doc => doc.DocumentType)
                .Include(doc => doc.Category)
                .ToList();

            Console.WriteLine("===================Документы===========================");
            Console.WriteLine("Номер документа\tТип документа\tКатегория\tБизнес-дата\tСсылка на файл\tСтатус документа\tХранить до");
            
            foreach (var document in documentsObject)
            {
                Console.WriteLine($"{document.Id_Doc}\t{document.DocumentType.DocumentType}\t{document.Category.CategoryName}\t{document.BusinessDate}\t{document.FilePath}\t{document.StatusDoc}\t{document.HranitDo}");
            }

            Console.WriteLine($"Всего записей  - {documentsObject.Count}");
            Console.WriteLine("===================================================");
            
            Console.WriteLine("Укажите ID документа:");
            string choice = Console.ReadLine();

            var documentByID = dbContext.Documents.Find(int.Parse(choice));

            if (documentByID != null)
            {
                Console.WriteLine("Укажите путь:");
                string path = Console.ReadLine();

                documentByID.CopyFile(path);
                Console.WriteLine("Готово");
            }
            else
            {
                Console.WriteLine("Документ не найден!");
            }
        }
    }

    public static void ArchiveDoc()
    {
        using (var dbContext = new ADDbContext())
        {
            var documentsObject = dbContext.Documents
                .Include(doc => doc.DocumentType)
                .Include(doc => doc.Category)
                .ToList();

            Console.WriteLine("===================Документы===========================");
            Console.WriteLine("Номер документа\tТип документа\tКатегория\tБизнес-дата\tСсылка на файл\tСтатус документа\tХранить до");

            foreach (var document in documentsObject)
            {
                Console.WriteLine($"{document.Id_Doc}\t{document.DocumentType.DocumentType}\t{document.Category.CategoryName}\t{document.BusinessDate}\t{document.FilePath}\t{document.StatusDoc}\t{document.HranitDo}");
            }

            Console.WriteLine($"Всего записей  - {documentsObject.Count}");
            Console.WriteLine("===================================================");

            Console.WriteLine("Укажите ID документа:");
            string choice = Console.ReadLine();

            var documentByID = dbContext.Documents.Find(int.Parse(choice));

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

    public static void FindDupHash()
    {
        using (var dbContext = new ADDbContext())
        {
            try
            {
                int foundDuplicate = 0;

                string[] files = Directory.GetFiles(PathForDirectory);

                foreach (string file in files)
                {
                    string pathToFile = file;
                    string fileName = Path.GetFileName(file);

                    DateTime startdtS = DateTime.Now;
                    string hashStreebogFile = ArchiveDocs.HashCode.GetStreebogHash(pathToFile);
                    DateTime finishdtS = DateTime.Now;
                    TimeSpan timeStreebog = finishdtS - startdtS;

                    DateTime startdtCRC = DateTime.Now;
                    string hashCRCFile = ArchiveDocs.HashCode.GetCRC32Hash(pathToFile);
                    DateTime finishdtCRC = DateTime.Now;
                    TimeSpan timeCRC = finishdtCRC - startdtCRC;

                    DateTime startdtMD5 = DateTime.Now;
                    string hashMD5File = ArchiveDocs.HashCode.GetMD5Hash(pathToFile);
                    DateTime finishdtMD5 = DateTime.Now;
                    TimeSpan timeMD5 = finishdtMD5 - startdtMD5;

                    DateTime startdtMurmur = DateTime.Now;
                    string hashMurmurFile = ArchiveDocs.HashCode.GetMurmurHash(pathToFile);
                    DateTime finishdtMurmur = DateTime.Now;
                    TimeSpan timeMurmur = finishdtMurmur - startdtMurmur;

                    string hashSum = hashStreebogFile;
                    TimeSpan minTime = TimeSpan.MinValue;

                    if (timeStreebog < minTime || minTime == TimeSpan.MinValue)
                    {
                        hashSum = hashStreebogFile;
                    }

                    if (timeCRC < minTime || minTime == TimeSpan.MinValue)
                    {
                        hashSum = hashCRCFile;
                    }

                    if (timeMD5 < minTime || minTime == TimeSpan.MinValue)
                    {
                        hashSum = hashMD5File;
                    }

                    if (timeMurmur < minTime || minTime == TimeSpan.MinValue)
                    {
                        hashSum = hashMurmurFile;
                    }

                    var document = dbContext.Documents.FirstOrDefault(doc =>
                            doc.HashSum == hashStreebogFile ||
                            doc.HashSum == hashCRCFile ||
                            doc.HashSum == hashMurmurFile ||
                            doc.HashSum == hashMD5File);
                    if (document != null)
                    {
                        foundDuplicate++;
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
                            HashSum = hashSum,
                            FilePath = destinationFilePath,
                            StatusDoc = DocumentStatus.Действующий,
                        };

                        dbContext.Documents.Add(newDocument);
                        dbContext.SaveChanges(); // Сохранение изменений в базе данных
                    }
                }

                Console.WriteLine($"Все файлоы обработаны, найдено {foundDuplicate} дублей");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }


        }
    }

    public static void DestroyDoc()
    {
        using (var dbContext = new ADDbContext())
        {
            var documentsObject = dbContext.Documents
                .Include(doc => doc.DocumentType)
                .Include(doc => doc.Category)
                .ToList();

            Console.WriteLine("===================Документы===========================");
            Console.WriteLine("Номер документа\tТип документа\tКатегория\tБизнес-дата\tСсылка на файл\tСтатус документа\tХранить до");

            foreach (var document in documentsObject)
            {
                Console.WriteLine($"{document.Id_Doc}\t{document.DocumentType.DocumentType}\t{document.Category.CategoryName}\t{document.BusinessDate}\t{document.FilePath}\t{document.StatusDoc}\t{document.HranitDo}");
            }

            Console.WriteLine($"Всего записей  - {documentsObject.Count}");
            Console.WriteLine("===================================================");

            Console.WriteLine("Укажите ID документа:");
            string choice = Console.ReadLine();

            var documentByID = dbContext.Documents.Find(int.Parse(choice));

            if (documentByID != null)
            {
                if (documentByID.HranitDo > DateTime.Now)
                {
                    Console.WriteLine("Срок хранения документа еще не истек!");
                    return;
                }

                documentByID.Destroy();
                dbContext.SaveChanges(); // Сохраняем изменения
                Console.WriteLine("Документ уничтожен!");
            }
            else
            {
                Console.WriteLine("Документ не найден!");
            }
        }
    }

    public static void Querys()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Запросы:");
            Console.WriteLine("1. Загружать документы с проверкой на дубль");
            Console.WriteLine("2. Просмотр документов по объекту");
            Console.WriteLine("3. Выгружать документы из БД");
            Console.WriteLine("4. Менять статус объекта на Архивный");
            Console.WriteLine("5. Уничтожать документы в БД");
            Console.WriteLine("6. Выход");

            Console.Write("Выберите действие (введите номер): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.WriteLine("Вы выбрали: Загружать документы с проверкой на дубль");
                    LoadDocs();
                    Console.ReadLine(); // Для остановки программы после выбора
                    break;
                case "2":
                    Console.WriteLine("Вы выбрали: Просмотр документов по объекту");
                    ViewDocsByObject();
                    Console.ReadLine(); // Для остановки программы после выбора
                    break;
                case "3":
                    Console.WriteLine("Вы выбрали: Выгружать документы из БД");
                    ExtractDoc();
                    Console.ReadLine(); // Для остановки программы после выбора
                    break;
                case "4":
                    Console.WriteLine("Вы выбрали: Менять статус объекта на Архивный");
                    ArchiveDoc();
                    Console.ReadLine(); // Для остановки программы после выбора
                    break;
                case "5":
                    Console.WriteLine("Уничтожать документы в БД");
                    DestroyDoc();
                    Console.ReadLine(); // Для остановки программы после выбора
                    break;
                case "6":
                    Console.WriteLine("Выход из меню.");
                    return;
                default:
                    Console.WriteLine("Некорректный выбор. Попробуйте еще раз.");
                    Console.ReadLine(); // Для остановки программы после некорректного ввода
                    break;
            }
        }
    }

    static void Main()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Меню:");
            Console.WriteLine("1. Выбор директории с тестовыми файлами");
            Console.WriteLine("2. Хеширование файлов");
            Console.WriteLine("3. Хеширование файлов (Benchmark)");
            Console.WriteLine("4. Выход");

            Console.Write("Выберите действие (введите номер): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    ChangePath();
                    break;
                case "2":
                    Console.WriteLine("Вы выбрали: Хеширование файлов");
                    HashFiles();
                    Console.ReadLine(); // Для остановки программы после выбора
                    break;
                case "3":
                    Console.WriteLine("Вы выбрали: Хеширование файлов (Benchmark)");
                    HashFilesBench();
                    Console.ReadLine(); // Для остановки программы после выбора
                    break;
                case "4":
                    Console.WriteLine("Выход из программы.");
                    return;
                default:
                    Console.WriteLine("Некорректный выбор. Попробуйте еще раз.");
                    Console.ReadLine(); // Для остановки программы после некорректного ввода
                    break;
            }

            //switch (choice)
            //{
            //    case "1":
            //        ChangePath();
            //        break;
            //    case "2":
            //        Console.WriteLine("Вы выбрали: Хеширование файлов");
            //        HashFiles();
            //        Console.ReadLine(); // Для остановки программы после выбора
            //        break;
            //    case "3":
            //        Console.WriteLine("Вы выбрали: Поиск дублей файлов в базе данных");
            //        FindDupHash();
            //        Console.ReadLine(); // Для остановки программы после выбора
            //        break;
            //    case "4":
            //        Console.WriteLine("Вы выбрали: Очистка базы данных от хешей");
            //        ClearHash();
            //        Console.ReadLine(); // Для остановки программы после выбора
            //        break;
            //    case "5":
            //        Console.WriteLine("Работа с объектами");
            //        ObjectsWork();
            //        break;
            //    case "6":
            //        Querys();
            //        return;
            //    case "7":
            //        Console.WriteLine("Выход из программы.");
            //        return;
            //    default:
            //        Console.WriteLine("Некорректный выбор. Попробуйте еще раз.");
            //        Console.ReadLine(); // Для остановки программы после некорректного ввода
            //        break;
            //}
        }
    }
}
