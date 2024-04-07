using System.Xml;

public class XslDocumentBuilder
{
    private XmlDocument xslDocument;

    public XslDocumentBuilder()
    {
        InitializeXslDocument();
    }

    private void InitializeXslDocument()
    {
        // Создание нового документа XSL
        xslDocument = new XmlDocument();

        // Создание корневого элемента xsl:stylesheet
        XmlElement stylesheetElement = xslDocument.CreateElement("xsl", "stylesheet", "http://www.w3.org/1999/XSL/Transform");
        xslDocument.AppendChild(stylesheetElement);

        // Добавление пространства имен для xsl
        XmlAttribute xmlnsAttribute = xslDocument.CreateAttribute("xmlns:xsl");
        xmlnsAttribute.Value = "http://www.w3.org/1999/XSL/Transform";
        stylesheetElement.Attributes.Append(xmlnsAttribute);

        // Добавление пространства имен для xml
        xmlnsAttribute = xslDocument.CreateAttribute("xmlns");
        xmlnsAttribute.Value = "http://www.w3.org/1999/xhtml";
        stylesheetElement.Attributes.Append(xmlnsAttribute);

        // Добавление атрибута version
        XmlAttribute versionAttribute = xslDocument.CreateAttribute("version");
        versionAttribute.Value = "1.0";
        stylesheetElement.Attributes.Append(versionAttribute);

        // Создание шаблона для identity transform
        XmlElement templateElement = xslDocument.CreateElement("xsl", "template", "http://www.w3.org/1999/XSL/Transform");
        templateElement.SetAttribute("match", "@*|node()");
        stylesheetElement.AppendChild(templateElement);

        XmlElement copyElement = xslDocument.CreateElement("xsl", "copy", "http://www.w3.org/1999/XSL/Transform");
        templateElement.AppendChild(copyElement);

        XmlElement applyTemplatesElement = xslDocument.CreateElement("xsl", "apply-templates", "http://www.w3.org/1999/XSL/Transform");
        applyTemplatesElement.SetAttribute("select", "@*|node()");
        copyElement.AppendChild(applyTemplatesElement);
    }

    public void AddRow(string id, string documentName, string duplicate)
    {
        // Получение корневого элемента
        XmlElement rootElement = xslDocument.DocumentElement;

        // Создание узла xsl:template для строки данных
        XmlElement templateElement = xslDocument.CreateElement("xsl", "template", "http://www.w3.org/1999/XSL/Transform");
        templateElement.SetAttribute("match", $"/root/data[@ID='{id}']");
        rootElement.AppendChild(templateElement);

        // Создание узлов xsl:value-of для каждого поля
        CreateValueOfElement("ID", templateElement);
        CreateValueOfElement("Наименование файла", templateElement);
        CreateValueOfElement("Дубль “номер карточки”", templateElement);

        // Добавление текста для каждого поля
        templateElement.InnerText = $"\n\t\t{id}\n\t\t{documentName}\n\t\t{duplicate}\n\t";

        // Создание узла xsl:apply-templates
        XmlElement applyTemplatesElement = xslDocument.CreateElement("xsl", "apply-templates", "http://www.w3.org/1999/XSL/Transform");
        applyTemplatesElement.SetAttribute("select", "following-sibling::node()");
        templateElement.AppendChild(applyTemplatesElement);
    }

    private void CreateValueOfElement(string select, XmlElement parentElement)
    {
        XmlElement valueOfElement = xslDocument.CreateElement("xsl", "value-of", "http://www.w3.org/1999/XSL/Transform");
        valueOfElement.SetAttribute("select", select);
        parentElement.AppendChild(valueOfElement);
    }

    public void SaveAndClose(string filePath)
    {
        // Сохранение XSL-документа
        xslDocument.Save(filePath);
    }
}
