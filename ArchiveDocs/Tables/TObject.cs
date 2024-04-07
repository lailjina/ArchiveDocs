using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TObject
{
    [Key]
    public int Id_Object { get; set; }

    public string Name { get; set; }

    public string Address { get; set; }

    public virtual List<TDocument>? Documents { get; set; }

    public ObjectStatus StatusObject { get; set; }

    public DateTime? DateArchived { get; set; }
    public DateTime? DateSendToArchive { get; set; }


    public void Archive()
    {
        this.StatusObject = ObjectStatus.Архивный;
        this.DateSendToArchive = DateTime.Now.ToUniversalTime();

        foreach (var document in this.Documents)
        {
            document.Archive();
        }

    }

}

public enum ObjectStatus
{
    Действующий,
    Архивный
}