using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TCategoryDocument
{
    [Key]
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    // Список документов данной категории
    public virtual List<TDocument>? Documents { get; set; }
}