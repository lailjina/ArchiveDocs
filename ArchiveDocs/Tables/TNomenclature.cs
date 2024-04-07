using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TNomenclature
{
    [Key]
    public int DocumentTypeId { get; set; }

    public string DocumentType { get; set; }

    public int StorageYears { get; set; }

    public virtual List<TDocument>? Documents { get; set; }

}
