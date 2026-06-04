# EasyPDF

A .NET 8 PDF generation library built on top of [QuestPDF](https://github.com/QuestPDF/QuestPDF).

EasyPDF gives you **two independent ways** to generate PDFs — pick whichever fits your use case, or combine both in the same project:

- **Annotation-driven** — decorate a C# model with attributes and hand it to the generator. Best for simple-to-medium documents with a fixed layout.
- **Fluent Builder** — compose documents programmatically with a chainable API and `foreach` loops. Best for complex, hierarchical reports where the structure is data-driven.

---

## Table of contents

1. [Setup](#setup)
2. [Approach 1 — Annotation-driven](#approach-1--annotation-driven)
   - [Page attributes](#page-attributes)
   - [Content attributes](#content-attributes)
   - [Styling attributes](#styling-attributes)
   - [Tables](#tables)
   - [Nested tables](#nested-tables)
   - [Repeating header / footer](#repeating-header--footer)
   - [Generate output](#generate-output-annotation)
3. [Approach 2 — Fluent Builder](#approach-2--fluent-builder)
   - [Page configuration](#page-configuration)
   - [Header and Footer](#header-and-footer)
   - [Content elements](#content-elements)
   - [Row layout](#row-layout)
   - [Simple Table](#simple-table)
   - [ReportTable — hierarchical data](#reporttable--hierarchical-data)
   - [Generate output](#generate-output-builder)
4. [Project structure](#project-structure)

---

## Setup

### 1. Reference the library

Add a project reference to `EasyPDF` from your consuming project:

```xml
<ProjectReference Include="..\EasyPDF\EasyPDF.csproj" />
```

### 2. Set the QuestPDF license

Call this **once** at application startup (before generating any PDF):

```csharp
QuestPDF.Settings.License = LicenseType.Community;
```

### 3a. Without dependency injection

```csharp
using EasyPDF.Factory;

var generator = PdfGeneratorFactory.Create();
```

### 3b. With dependency injection (ASP.NET Core, etc.)

```csharp
using EasyPDF.Extensions;

builder.Services.AddEasyPdf();
```

Then inject `IPdfGenerator` wherever you need it.

---

## Approach 1 — Annotation-driven

Create a class that implements `IPdfDocument`, decorate it with attributes, and pass it to `IPdfGenerator`.

### Minimal example

```csharp
using EasyPDF.Annotations.Content;
using EasyPDF.Annotations.Page;
using EasyPDF.Annotations.Styling;
using EasyPDF.Enums;
using EasyPDF.Interfaces;

[PdfPageSize(PdfPageSizeType.A4)]
[PdfPageMargin(2f, PdfUnit.Centimetre)]
[PdfDefaultFontSize(11f)]
public class InvoiceDocument : IPdfDocument
{
    [PdfTitle(Order = 0)]
    [PdfBold]
    [PdfFontSize(28f)]
    [PdfAlignment(PdfAlignment.Center)]
    public string Title { get; set; } = "INVOICE";

    [PdfText(Order = 1, Label = "Customer:")]
    public string CustomerName { get; set; } = "";

    [PdfSeparator(Order = 2)]
    public string? Sep { get; set; }

    [PdfTable(Order = 3, ShowHeader = true, StripedRows = true)]
    public List<InvoiceItem> Items { get; set; } = new();

    [PdfText(Order = 4, Label = "Total:")]
    [PdfBold]
    [PdfAlignment(PdfAlignment.Right)]
    public string Total { get; set; } = "";
}

public class InvoiceItem
{
    [PdfColumn("Product",    Order = 0, RelativeWidth = 3f)]
    public string ProductName { get; set; } = "";

    [PdfColumn("Qty",        Order = 1)]
    public int Quantity { get; set; }

    [PdfColumn("Unit Price", Order = 2)]
    public string UnitPrice { get; set; } = "";

    [PdfColumnIgnore]            // excluded from the table
    public Guid InternalId { get; set; }
}
```

> The `Order` property on every content attribute controls the render order top-to-bottom.

---

### Page attributes

Applied **on the class**:

| Attribute | Default | Description |
|---|---|---|
| `[PdfPageSize(PdfPageSizeType)]` | `A4` | `A3` `A4` `A5` `Letter` `Legal` `Tabloid` |
| `[PdfPageMargin(value, PdfUnit)]` | `2cm` | Uniform page margin. `PdfUnit`: `Point` `Centimetre` `Millimetre` `Inch` |
| `[PdfPageColor("#hex")]` | `#FFFFFF` | Page background color |
| `[PdfDefaultFontSize(float)]` | `12` | Base font size in points for the whole page |

---

### Content attributes

Applied **on properties**. Every one has an `Order` parameter:

| Attribute | Type | Description |
|---|---|---|
| `[PdfTitle]` | `string` | Large heading |
| `[PdfSubtitle]` | `string` | Medium heading |
| `[PdfText(Label?)]` | `string` | Text field. Optional `Label` is rendered in bold before the value |
| `[PdfTable]` | `List<T>` | Renders a collection as a table (see [Tables](#tables)) |
| `[PdfImage]` | `byte[]` or `string` (path) | Embeds an image |
| `[PdfSeparator]` | any | Horizontal rule |
| `[PdfIgnore]` | any | Skips the property entirely |

#### `[PdfSeparator]` options

```csharp
[PdfSeparator(Order = 2, Thickness = 0.5f, Color = "#CCCCCC", VerticalSpacing = 8f)]
public string? Sep { get; set; }
```

#### `[PdfImage]` options

```csharp
[PdfImage(Order = 1, Width = 200f, Height = 80f)]
public byte[] Logo { get; set; }
```

---

### Styling attributes

Combine freely with any content attribute:

| Attribute | Description |
|---|---|
| `[PdfBold]` | Bold text |
| `[PdfItalic]` | Italic text |
| `[PdfUnderline]` | Underlined text |
| `[PdfFontSize(float)]` | Font size in points |
| `[PdfFontColor("#hex")]` | Text color |
| `[PdfBackgroundColor("#hex")]` | Container background color |
| `[PdfAlignment(PdfAlignment)]` | `Left` `Center` `Right` |
| `[PdfPadding(all)]` | Uniform padding in points |
| `[PdfPadding(vertical, horizontal)]` | Axis-based padding |
| `[PdfPadding(top, bottom, left, right)]` | Full individual padding |
| `[PdfBorder(thickness, color)]` | Border around the element |
| `[PdfSpacing(before, after)]` | Vertical space before/after the element in points |

---

### Tables

Apply `[PdfTable]` to a `List<T>` property. Decorate the row class with `[PdfColumn]`:

```csharp
[PdfTable(Order = 3,
    Title                 = "Items",
    ShowHeader            = true,
    StripedRows           = true,
    HeaderBackgroundColor = "#1E3A5F",
    HeaderFontColor       = "#FFFFFF",
    StripedRowColor       = "#F0F4FA")]
public List<OrderItem> Items { get; set; } = new();

public class OrderItem
{
    [PdfColumn("Description", Order = 0, RelativeWidth = 4f)]
    public string Description { get; set; } = "";

    [PdfColumn("Qty",         Order = 1, RelativeWidth = 1f)]
    public int Qty { get; set; }

    [PdfColumn("Price",       Order = 2, RelativeWidth = 1.5f)]
    public string Price { get; set; } = "";

    [PdfColumnIgnore]
    public string InternalCode { get; set; } = "";
}
```

`RelativeWidth` works proportionally — a column with `2f` is twice as wide as one with `1f`.

---

### Nested tables

Apply `[PdfTable]` to a `List<T>` property **inside a row class**. The renderer detects it automatically and renders it as an indented sub-table below each parent row. Works recursively to any depth.

```csharp
public class Contract
{
    [PdfColumn("Number", Order = 0)]
    public string Number { get; set; } = "";

    [PdfColumn("Buyer", Order = 1, RelativeWidth = 3f)]
    public string Buyer { get; set; } = "";

    // This List<T> triggers a nested sub-table automatically
    [PdfTable(Title = "Nominations", ShowHeader = true,
              HeaderBackgroundColor = "#555555", HeaderFontColor = "#FFFFFF")]
    public List<Nomination> Nominations { get; set; } = new();
}

public class Nomination
{
    [PdfColumn("Vessel", Order = 0)] public string Vessel { get; set; } = "";
    [PdfColumn("Tonnes", Order = 1)] public string Tonnes { get; set; } = "";
    [PdfColumn("Port",   Order = 2)] public string Port   { get; set; } = "";
}
```

---

### Repeating header / footer

Mark a property with `[PdfPageHeaderContent]` or `[PdfPageFooterContent]`. Its value is itself an annotated object — its properties render inside the repeating header or footer on every page.

```csharp
public class ReportDocument : IPdfDocument
{
    [PdfPageHeaderContent]
    public ReportHeader PageHeader { get; set; } = new();

    [PdfPageFooterContent]
    public ReportFooter PageFooter { get; set; } = new();

    [PdfTable(Order = 0)]
    public List<DataRow> Rows { get; set; } = new();
}

public class ReportHeader
{
    [PdfTitle(Order = 0)]
    [PdfAlignment(PdfAlignment.Center)]
    public string Title { get; set; } = "My Report";

    [PdfSeparator(Order = 1)]
    public string? Sep { get; set; }
}

public class ReportFooter
{
    [PdfText(Order = 0)]
    [PdfAlignment(PdfAlignment.Right)]
    [PdfFontSize(8f)]
    public string Company { get; set; } = "ACME Corp.";
}
```

---

### Generate output (annotation)

```csharp
var generator = PdfGeneratorFactory.Create();
var document  = new InvoiceDocument { Title = "INVOICE", ... };

// Write to file
generator.GenerateToFile(document, "invoice.pdf");

// Get bytes (HTTP response, database, etc.)
byte[] bytes = generator.Generate(document);

// Get stream
Stream stream = generator.GenerateToStream(document);
```

---

## Approach 2 — Fluent Builder

`PdfDocumentBuilder` lets you compose documents programmatically — no model class required. Ideal for hierarchical reports driven by data from a database or API.

### Minimal example

```csharp
using EasyPDF.Builder;
using EasyPDF.Enums;

PdfDocumentBuilder.Create()
    .Page(p => p.A4().Margin(2f, PdfUnit.Centimetre).DefaultFontSize(11f))
    .Content(c =>
    {
        c.Text("Hello, EasyPDF!").Bold().FontSize(24).AlignCenter();
        c.Spacing(12);
        c.Separator();
    })
    .GeneratePdf("hello.pdf");
```

---

### Page configuration

```csharp
.Page(p => p
    .A4()                               // preset sizes: A3() A4() A5() Letter() Legal()
    .Landscape()                        // flip orientation (also Portrait())
    .Custom(297, 210)                   // custom size in millimetres
    .Margin(1.5f, PdfUnit.Centimetre)   // uniform margin
    .BackgroundColor("#FFFFFF")
    .DefaultFontSize(10f))
```

---

### Header and Footer

Both zones accept the same element methods as `Content`. They **repeat on every page**.

```csharp
.Header(h =>
{
    h.Row(r =>
    {
        r.Auto(col => col.Text("COMPANY NAME").Bold().FontSize(10));
        r.Fill(col =>
        {
            col.Text("Annual Report 2026").Bold().FontSize(16).Underline().AlignCenter();
            col.Text("Fiscal Year Jan–Dec").FontSize(9).AlignCenter();
        });
        r.Auto(col =>
        {
            col.Text("01/01/2026").AlignRight().FontSize(8);
            col.Text("09:00").AlignRight().FontSize(8);
        });
    });
    h.Separator(0.5f, "#000000", 3f);
})
.Footer(f =>
{
    f.PageNumber("Page {page} of {total}").AlignRight().FontSize(8);
})
```

---

### Content elements

All available methods inside `.Header()`, `.Footer()` and `.Content()`:

#### Text

```csharp
c.Text("Hello world")
    .Bold()
    .Italic()
    .Underline()
    .FontSize(14f)
    .Color("#1E3A5F")
    .AlignLeft()    // also .AlignCenter() .AlignRight()
```

#### Separator

```csharp
c.Separator();                          // default: 1pt, #000000, 4pt spacing
c.Separator(0.5f, "#CCCCCC", 8f);      // thickness, color, vertical spacing
```

#### Spacing

```csharp
c.Spacing(16f);   // blank vertical space in points
```

#### Page number (typically used in footer)

```csharp
f.PageNumber("Page {page} of {total}")
    .AlignRight()
    .FontSize(8f)
    .Color("#555555");
```

Supported tokens: `{page}` → current page number, `{total}` → total pages.

---

### Row layout

`Row` divides horizontal space into columns. Each column receives its own `SectionBuilder` so you can stack multiple elements inside it.

```csharp
c.Row(r =>
{
    // Shrinks to fit its content
    r.Auto(col => col.Text("Left").Bold());

    // Takes all remaining horizontal space
    r.Fill(col =>
    {
        col.Text("Center top").AlignCenter().Bold();
        col.Text("Center bottom").AlignCenter().FontSize(9);
    });

    // Fixed width in points
    r.Fixed(120, col => col.Text("Right").AlignRight());
});
```

---

### Simple Table

For flat lists with typed data:

```csharp
c.Table(myList, t =>
{
    t.Column(r => r.Code,        "Code",        1.5f);
    t.Column(r => r.Description, "Description", 4f);
    t.Column(r => r.Amount,      "Amount",      1.5f, col => col.AlignRight().Bold());

    t.HeaderBackground("#1E3A5F")
     .HeaderForeground("#FFFFFF")
     .Striped(true, "#F0F4FA")
     .Title("Products");
});
```

Each `.Column()` takes:
- A value selector `r => r.Property` (converted to string automatically)
- The header label
- The relative width
- An optional inline style: `.Bold()` `.AlignRight()` `.AlignCenter()` `.Color()` `.FontSize()`

---

### ReportTable — hierarchical data

Use `ReportTable<T>` when you need to mix data rows, full-width heading rows, and subtotal rows **inside the same QuestPDF table** so columns stay aligned across page breaks.

The typical pattern: put the column header row in the **page header** (so it repeats on every page) and the data in **content**:

```csharp
.Header(h =>
{
    // ... company info row, filter row, etc. ...

    // Column headers — must use the SAME relative widths as the ReportTable below
    h.ColumnHeaderRow(cols =>
    {
        cols.Column("Contract",  1.4f);
        cols.Column("Date",      1.8f);
        cols.Column("Seller",    3.8f);
        cols.Column("Buyer",     3.8f);
        cols.Column("Kg",        2.2f);
        cols.Column("Price",     1.3f);
        cols.Column("Port",      1.8f);
        cols.Column("Period",    1.6f);
        cols.Column("Product",   2.6f);
    });
})
.Content(c =>
{
    c.ReportTable<ContractRow>(t =>
    {
        // Column definitions — same relative widths as ColumnHeaderRow above
        t.Column(r => r.Contract, 1.4f);
        t.Column(r => r.Date,     1.8f);
        t.Column(r => r.Seller,   3.8f);
        t.Column(r => r.Buyer,    3.8f, col => col.Bold());
        t.Column(r => r.Kg,       2.2f, col => col.AlignRight());
        t.Column(r => r.Price,    1.3f, col => col.AlignRight().Bold());
        t.Column(r => r.Port,     1.8f);
        t.Column(r => r.Period,   1.6f);
        t.Column(r => r.Product,  2.6f);

        // Populate with nested foreach loops — any depth
        foreach (var client in data.Clients)
        {
            t.SpanRow($"Client: {client.Id}  {client.Name}",
                bold: true, fontSize: 8.5f, paddingTop: 3f, paddingBottom: 3f);

            foreach (var section in client.Sections)
            {
                t.SpanRow(section.Name, bold: true, fontSize: 8f, paddingTop: 1f);

                foreach (var period in section.Periods)
                {
                    foreach (var row in period.Rows)
                        t.DataRow(row);                                         // data row

                    t.SubtotalRow($"Total {period.Name}", period.Total);        // subtotal
                }

                t.SubtotalRow($"Total {section.Name}", section.Total);
            }

            t.SubtotalRow("Net Total C/V", client.NetTotal, isBold: true);
        }
    });
})
```

#### Row methods

| Method | Description |
|---|---|
| `t.DataRow(item)` | Renders one typed row using the column selectors |
| `t.SpanRow(text, bold, fontSize, paddingTop, paddingBottom)` | Full-width spanning row (client headers, section titles, …) |
| `t.SubtotalRow(label, amount, isBold, emptyBefore)` | `emptyBefore` empty cells → label (right) → amount (bold, top-border, right) → remaining empty cells |

#### SubtotalRow layout

```
[ emptyBefore cols ]  |  label →  |  ↑ amount →  |  [ remaining cols ]
```

Default `emptyBefore = 3`. For a 9-column table this places the label in col 4 and the amount in col 5.

#### ColumnHeaderRow

Renders a standalone styled header row as a table. Place it in `.Header()` so it repeats on every page and aligns with the `ReportTable` below — both must use the same relative widths.

```csharp
h.ColumnHeaderRow(cols =>
{
    cols.Column("Name",   3f);
    cols.Column("Amount", 1.5f);
    cols.Background("#000000").Foreground("#FFFFFF").FontSize(7.5f);
});
```

---

### Generate output (builder)

```csharp
var builder = PdfDocumentBuilder.Create()
    .Page(...)
    .Header(...)
    .Content(...);

// Write to file
builder.GeneratePdf("report.pdf");

// Get bytes (for HTTP responses, storing in a database, etc.)
byte[] bytes = builder.GeneratePdfBytes();

// Get a Stream
Stream stream = builder.GeneratePdfStream();

// Get the raw QuestPDF IDocument for full control
IDocument doc = builder.Build();
```

---

## Project structure

```
EasyPDF/                             ← core library (.NET 8 class library)
│
├── Annotations/
│   ├── Content/                     ← [PdfTitle] [PdfTable] [PdfText] [PdfImage] …
│   ├── Page/                        ← [PdfPageSize] [PdfPageMargin] …
│   ├── Styling/                     ← [PdfBold] [PdfFontSize] [PdfAlignment] …
│   └── Table/                       ← [PdfColumn] [PdfColumnIgnore]
│
├── Builder/
│   ├── Elements/                    ← TextElement, RowElement, TableElement,
│   │                                   ReportTableElement, ColumnHeaderRowElement, …
│   ├── Internal/                    ← BuilderDocument (wraps QuestPDF IDocument)
│   ├── PageConfigBuilder.cs
│   ├── SectionBuilder.cs
│   └── PdfDocumentBuilder.cs       ← entry point for the fluent API
│
├── Enums/                           ← PdfAlignment, PdfPageSizeType, PdfUnit
├── Extensions/                      ← AddEasyPdf() for IServiceCollection
├── Factory/                         ← PdfGeneratorFactory.Create() (no DI needed)
├── Interfaces/                      ← IPdfDocument, IPdfGenerator, IElementRenderer, …
├── Models/                          ← PropertyDescriptor, ColumnInfo, PageConfiguration
├── Registry/                        ← RendererRegistry
├── Renderers/                       ← TitleRenderer, TableRenderer, … (one per element type)
├── Resolvers/                       ← PropertyAnnotationResolver, TableColumnResolver
├── Services/                        ← PdfGeneratorService
└── Styling/                         ← ContainerStyleApplier, TextStyleApplier

EasyPDF.Sample/                      ← sample console app
├── Documents/                       ← PosicionClienteDocument (raw QuestPDF IDocument)
├── Models/                          ← InvoiceDocument, PosicionCliente models
├── Services/
│   ├── InvoiceServices.cs           ← annotation-driven (dotnet run -- invoice)
│   ├── PosicionClientesServices.cs  ← raw IDocument      (dotnet run -- posicion)
│   └── PruebaPosicionPDF.cs         ← fluent builder      (dotnet run -- prueba)
└── Program.cs
```

### Run the samples

```bash
cd EasyPDF.Sample

dotnet run -- invoice    # generates factura.pdf          (annotation-driven)
dotnet run -- posicion   # generates posicion_clientes.pdf (raw QuestPDF IDocument)
dotnet run -- prueba     # generates posicion_builder.pdf  (fluent builder)
```

---

## Dependencies

| Package | Version |
|---|---|
| [QuestPDF](https://www.questpdf.com/) | 2026.5.0 |
| Microsoft.Extensions.DependencyInjection.Abstractions | 10.0.8 |

QuestPDF is free for open-source and small commercial projects under the [Community License](https://www.questpdf.com/license/).

---

## License

MIT
