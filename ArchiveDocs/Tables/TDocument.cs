using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TDocument
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id_Doc { get; set; }

    public string? DocumentNumber { get; set; }

    [ForeignKey("DocumentType")]
    public int DocumentTypeId { get; set; }
    public virtual TNomenclature DocumentType { get; set; }

    [ForeignKey("Category")]
    public int CategoryId { get; set; }
    public virtual TCategoryDocument Category { get; set; }

    public DateTime BusinessDate { get; set; }

    [ForeignKey("Object")]
    public int? ObjectId { get; set; }
    public virtual TObject? Object { get; set; }

    public string? HashSum { get; set; }

    public string FilePath { get; set; }
    public DocumentStatus StatusDoc { get; set; }

    public DateTime? HranitDo { get; set; }
    public DateTime? DateSendToArchive { get; set; }

    public TCategoryDocument TCategoryDocument
    {
        get => default;
        set
        {
        }
    }

    public TNomenclature TNomenclature
    {
        get => default;
        set
        {
        }
    }

    public TObject TObject
    {
        get => default;
        set
        {
        }
    }

    public void CopyFile(string destinationPath)
    {
        File.Copy(this.FilePath, destinationPath + $"//{Path.GetFileName(this.FilePath)}", true);
    }

    public void Archive()
    {
        this.StatusDoc = DocumentStatus.Архивный;
        this.DateSendToArchive = DateTime.Now.ToUniversalTime().AddHours(1);
        this.HranitDo = new DateTime(this.DateSendToArchive.Value.AddYears(this.DocumentType.StorageYears + 1).Year,1,1).ToUniversalTime().AddHours(1);
    }

    public void Destroy()
    {
        this.StatusDoc = DocumentStatus.Уничтожен;
        File.Delete(this.FilePath);
        this.HashSum = null;
    }
}

public enum DocumentStatus
{
    Действующий,
    Архивный,
    Уничтожен
}