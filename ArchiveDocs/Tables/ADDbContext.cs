using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace ArchiveDocs.Tables
{
    public class ADDbContext : DbContext
    {
        public DbSet<TCategoryDocument> Categories { get; set; }
        public DbSet<TObject> Objects { get; set; }
        public DbSet<TNomenclature> Nomenclatures { get; set; }
        public DbSet<TDocument> Documents { get; set; }

        public ADDbContext()
        {
        }

        public ADDbContext(DbContextOptions<ADDbContext> options) : base(options)
        {
            bool categoriesExist = Database.GetDbConnection().GetSchema("Tables").Rows
                .OfType<DataRow>().Any(row => row["TABLE_NAME"].ToString().Equals("Categories"));

            if (categoriesExist)
                if (!Categories.Any())
                {
                    var defaultCategories = new List<TCategoryDocument>
                    {
                        new TCategoryDocument { CategoryId = 1, CategoryName = "ОВиК" },
                        new TCategoryDocument { CategoryId = 2, CategoryName = "ЭС" },
                        new TCategoryDocument { CategoryId = 3, CategoryName = "АР" },
                        new TCategoryDocument { CategoryId = 4, CategoryName = "ВК" }
                    };

                    Categories.AddRange(defaultCategories);
                    SaveChanges();
                }

            bool nomenclaturesExist = Database.GetDbConnection().GetSchema("Tables").Rows
                .OfType<DataRow>().Any(row => row["TABLE_NAME"].ToString().Equals("Nomenclatures"));
            if (nomenclaturesExist)
                if (!Nomenclatures.Any())
                {
                    var defaultNomenclature = new List<TNomenclature>
                    {
                        new TNomenclature { DocumentTypeId = 1, DocumentType = "Исполнительная документация", StorageYears = 5 },
                        new TNomenclature { DocumentTypeId = 2, DocumentType = "Акты", StorageYears = 3 },
                        new TNomenclature { DocumentTypeId = 3, DocumentType = "Протокол проверки", StorageYears = 1 }
                    };

                    Nomenclatures.AddRange(defaultNomenclature);
                    SaveChanges();
                }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var connectionString = config.GetConnectionString("PostgreSQLConnection");

            optionsBuilder.UseNpgsql(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Настройка модели данных
            base.OnModelCreating(modelBuilder);
        }

    }
}
