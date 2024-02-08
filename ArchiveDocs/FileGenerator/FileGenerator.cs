using ArchiveDocs.Tables;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArchiveDocs.FileGenerator
{
    public class FileGenerator
    {
        private readonly ADDbContext _dbContext;

        public FileGenerator(ADDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void GenerateFiles(int count, int sizeInBytes, bool addDuplicates = false)
        {
            var documentTypes = _dbContext.Nomenclatures.Select(n => n.DocumentType).ToList();
            var categories = _dbContext.Categories.Select(c => c.CategoryName).ToList();


            var random = new Random();

            var documents = documentTypes.SelectMany(d => categories.Select(c => $"{d}_{c}")).ToList();

            if (addDuplicates)
            {
                // Добавление дубликатов
                documents.AddRange(documents.Take(random.Next(1, documents.Count)));
            }

            for (var i = 1; i <= count; i++)
            {
                var fileName = $"{documents[random.Next(0, documents.Count)]}_{DateTime.Now:yyyyMMddHHmmssfff}.txt";
                var filePath = Path.Combine(Environment.CurrentDirectory+"\\TestFiles\\", fileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    fileStream.SetLength(sizeInBytes);
                }

                Console.WriteLine($"Файл {fileName} сгенерирован, размером {sizeInBytes / 1024} KB.");
            }
        }
    }
}
