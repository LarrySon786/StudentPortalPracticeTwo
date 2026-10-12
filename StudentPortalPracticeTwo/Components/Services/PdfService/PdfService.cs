using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace StudentPortalPracticeTwo.Components.Services.PdfServices;

public class PdfService
{
    public byte[] GenerateApplicationPdf()
    {
        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.Margin(40);

                page.Header()
                    .Text("Cascade State University")
                    .FontSize(22)
                    .Bold();

                page.Content()
                    .PaddingVertical(20)
                    .Column(column =>
                    {
                        column.Spacing(10);

                        column.Item()
                            .Text("Student Application")
                            .FontSize(18)
                            .Bold();

                        column.Item()
                            .Text("Student Information")
                            .FontSize(14)
                            .Bold();

                        column.Item()
                            .Text("Full Name: __________________________");

                        column.Item()
                            .Text("Email: ______________________________");

                        column.Item()
                            .Text("Date of Birth: ________________________");
                    });

                page.Footer()
                    .AlignCenter()
                    .Text("Cascade State University");
            });
        }).GeneratePdf();
    }
}