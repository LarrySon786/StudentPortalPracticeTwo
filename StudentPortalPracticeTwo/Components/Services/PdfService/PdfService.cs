
using StudentPortalPracticeTwo.Database.Models.Application;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Reflection;

namespace StudentPortalPracticeTwo.Components.Services.PdfServices;

public class PdfService
{
    // CSU BRAND COLORS
    private const string Navy = "#243B4A";
    private const string Teal = "#2A9D8F";
    private const string HeadingTeal = "#1B6F6A";
    private const string PaleTeal = "#EEF7F6";
    private const string LightTeal = "#D8E8E5";
    private const string LabelColor = "#667F97";
    private const string White = "#FFFFFF";
    private const string Muted = "#486581";

    public byte[] GenerateApplicationPdf(ApplicationModel final)
    {
        ArgumentNullException.ThrowIfNull(final);

        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.Letter);
                page.MarginHorizontal(36);
                page.MarginVertical(30);

                page.DefaultTextStyle(style => style
                    .FontFamily("Lato")
                    .FontSize(9)
                    .FontColor(Navy));

                // REPEATED HEADER
                page.Header().Column(header =>
                {
                    header.Item()
                        .Background(Navy)
                        .PaddingBottom(11)
                        .Row(row =>
                        {
                            row.RelativeItem().Column(left =>
                            {
                                left.Item()
                                    .Text("CASCADE STATE UNIVERSITY")
                                    .FontSize(16)
                                    .Bold()
                                    .FontColor(White);

                                left.Item()
                                    .PaddingTop(3)
                                    .Text("OFFICE OF ENROLLMENT")
                                    .FontSize(8)
                                    .LetterSpacing(0.8f)
                                    .FontColor("#CAF0F8");
                            });

                            row.ConstantItem(100)
                                .AlignRight()
                                .AlignMiddle()
                                .Text("APPLICATION RECORD")
                                .FontSize(7)
                                .Bold()
                                .FontColor("#6FD3C8");
                        });

                    header.Item()
                        .Height(3)
                        .Background(Teal);
                });

                // DOCUMENT CONTENT
                page.Content()
                    .PaddingTop(12)
                    .Column(content =>
                    {
                        content.Spacing(8);

                        // APPLICATION TITLE
                        content.Item().Row(row =>
                        {
                            row.RelativeItem().Column(left =>
                            {
                                left.Item()
                                    .Text("Student Application")
                                    .FontSize(19)
                                    .Bold()
                                    .FontColor(Navy);

                                left.Item()
                                    .PaddingTop(2)
                                    .Text("Official applicant copy")
                                    .FontSize(9)
                                    .FontColor(Muted);
                            });

                            row.ConstantItem(130)
                                .Background(PaleTeal)
                                .Padding(8)
                                .Column(right =>
                                {
                                    right.Item()
                                        .Text("APPLICATION ID")
                                        .FontSize(7)
                                        .Bold()
                                        .FontColor(HeadingTeal);

                                    right.Item()
                                        .PaddingTop(2)
                                        .Text(final.Id.ToString())
                                        .FontSize(14)
                                        .Bold()
                                        .FontColor(Navy);
                                });
                        });

                        // STATUS
                        content.Item().Row(row =>
                        {
                            row.AutoItem()
                                .Text("Review status: ")
                                .FontSize(8)
                                .FontColor(LabelColor);

                            row.AutoItem()
                                .Text(final.ApprovedStatus.ToString())
                                .FontSize(8)
                                .Bold()
                                .FontColor(HeadingTeal);
                        });

                        // 01: STUDENT INFORMATION
                        content.Item().Element(c =>
                            SectionHeader(c, "01", "STUDENT INFORMATION"));

                        content.Item().Row(row =>
                        {
                            row.RelativeItem().Element(c => Field(c,
                                "First Name", final.StudentInfo.FirstName));

                            row.RelativeItem().Element(c => Field(c,
                                "Middle Name", final.StudentInfo.MiddleName));

                            row.RelativeItem().Element(c => Field(c,
                                "Last Name", final.StudentInfo.LastName));
                        });

                        content.Item().Row(row =>
                        {
                            row.RelativeItem().Element(c => Field(c,
                                "Date of Birth",
                                final.StudentInfo.DateOfBirth.ToString("MMMM d, yyyy")));

                            row.RelativeItem().Element(c => Field(c,
                                "Email", final.Email));

                            row.RelativeItem().Element(c => Field(c,
                                "Gender", final.StudentInfo.Gender.ToString()));
                        });

                        content.Item().Row(row =>
                        {
                            row.RelativeItem().Element(c => Field(c,
                                "Race", final.StudentInfo.Race.ToString()));

                            row.RelativeItem().Element(c => Field(c,
                                "Citizenship",
                                final.StudentInfo.CitizenshipCountry.ToString()));
                        });

                        // 02: CONTACT INFORMATION
                        content.Item().Element(c =>
                            SectionHeader(c, "02", "CONTACT INFORMATION"));

                        content.Item().Row(row =>
                        {
                            row.RelativeItem().Element(c => Field(c,
                                "Primary Phone", final.StudentContact.Phone));

                            row.RelativeItem().Element(c => Field(c,
                                "Alternative Phone",
                                final.StudentContact.AltPhone));
                        });

                        content.Item().Element(c => Field(c,
                            "Street Address",
                            JoinNonEmpty(final.StudentInfo.StreetOneAddress,
                                final.StudentInfo.StreetTwoAddress)));

                        content.Item().Row(row =>
                        {
                            row.RelativeItem().Element(c => Field(c,
                                "City", final.StudentInfo.City));

                            row.RelativeItem().Element(c => Field(c,
                                "State / Province",
                                final.StudentInfo.StateOrProvince));

                            row.ConstantItem(75).Element(c => Field(c,
                                "ZIP Code",
                                final.StudentInfo.Zipcode.ToString()));
                        });

                        // 03: PROGRAM SELECTION
                        content.Item().Element(c =>
                            SectionHeader(c, "03", "PROGRAM & ENROLLMENT"));

                        content.Item().Row(row =>
                        {
                            row.RelativeItem().Element(c => Field(c,
                                "Selected Program",
                                GetDisplayName(final.StudentProgram.SelectedProgram)));

                            row.RelativeItem().Element(c => Field(c,
                                "Start Term",
                                GetDisplayName(final.StudentProgram.StartTerm)));
                        });

                        // 04: ACADEMIC HISTORY
                        content.Item().Element(c =>
                            SectionHeader(c, "04", "ACADEMIC HISTORY"));

                        content.Item().Background(PaleTeal).Padding(8)
                            .Column(academic =>
                            {
                                academic.Spacing(5);

                                academic.Item().Element(c => Field(c,
                                    "High School Transcript",
                                    final.AcademicHistory.HighschoolTranscriptFileName));

                                academic.Item().Element(c => Field(c,
                                    "College Transcript",
                                    final.AcademicHistory.CollegeTranscriptFileName));
                            });

                        // 05: EMERGENCY CONTACTS
                        content.Item().Element(c =>
                            SectionHeader(c, "05", "EMERGENCY CONTACTS"));

                        if (final.EmergencyContact.Count == 0)
                        {
                            content.Item().Text("No emergency contacts provided.")
                                .FontSize(8)
                                .Italic()
                                .FontColor(Muted);
                        }
                        else
                        {
                            foreach (var contact in final.EmergencyContact)
                            {
                                content.Item()
                                    .BorderBottom(0.5f)
                                    .BorderColor(LightTeal)
                                    .PaddingVertical(5)
                                    .Row(row =>
                                    {
                                        row.RelativeItem().Element(c => Field(c,
                                            "Contact Name", contact.ContactName));

                                        row.RelativeItem().Element(c => Field(c,
                                            "Relationship", contact.Relationship));

                                        row.RelativeItem().Element(c => Field(c,
                                            "Phone", contact.Phone));
                                    });
                            }
                        }

                        // ESSAYS BEGIN ON SUBSEQUENT PAGES
                        content.Item().PageBreak();

                        // 06: ESSAY ONE
                        content.Item().Element(c =>
                            EssaySection(c, "06", "PERSONAL STATEMENT",
                                final.Essays.ResponseOne));

                        content.Item().PageBreak();

                        // 07: ESSAY TWO
                        content.Item().Element(c =>
                            EssaySection(c, "07", "ESSAY RESPONSE TWO",
                                final.Essays.ResponseTwo));

                        content.Item().PageBreak();

                        // 08: ESSAY THREE
                        content.Item().Element(c =>
                            EssaySection(c, "08", "ESSAY RESPONSE THREE",
                                final.Essays.ResponseThree));
                    });

                // REPEATED FOOTER
                page.Footer()
                    .PaddingTop(6)
                    .BorderTop(1)
                    .BorderColor(LightTeal)
                    .Row(row =>
                    {
                        row.RelativeItem().Text(
                            "CASCADE STATE UNIVERSITY  |  CONFIDENTIAL")
                            .FontSize(7)
                            .FontColor(Muted);

                        row.AutoItem().Text(text =>
                        {
                            text.Span("Page ").FontSize(7);
                            text.CurrentPageNumber().FontSize(7);
                            text.Span(" of ").FontSize(7);
                            text.TotalPages().FontSize(7);
                        });
                    });
            });
        }).GeneratePdf();
    }

    // SECTION HEADING
    private static void SectionHeader(
        IContainer container,
        string number,
        string title)
    {
        container
            .Background(PaleTeal)
            .BorderLeft(3)
            .BorderColor(Teal)
            .PaddingHorizontal(8)
            .PaddingVertical(5)
            .Row(row =>
            {
                row.ConstantItem(24)
                    .AlignMiddle()
                    .Text(number)
                    .FontSize(9)
                    .Bold()
                    .FontColor(HeadingTeal);

                row.RelativeItem()
                    .AlignMiddle()
                    .Text(title)
                    .FontSize(9)
                    .Bold()
                    .FontColor(Navy);
            });
    }

    // FIELD LABEL + VALUE
    private static void Field(
        IContainer container,
        string label,
        string? value)
    {
        container.Column(column =>
        {
            column.Spacing(2);

            column.Item()
                .Text(label.ToUpperInvariant())
                .FontSize(6.5f)
                .Bold()
                .FontColor(LabelColor);

            column.Item()
                .Text(string.IsNullOrWhiteSpace(value)
                    ? "Not provided"
                    : value)
                .FontSize(8.5f)
                .FontColor(Navy);
        });
    }

    // ESSAY SECTION
    private static void EssaySection(
        IContainer container,
        string number,
        string title,
        string? response)
    {
        container.Column(column =>
        {
            column.Spacing(12);

            column.Item().Element(c =>
                SectionHeader(c, number, title));

            column.Item()
                .Text(string.IsNullOrWhiteSpace(response)
                    ? "No response provided."
                    : response)
                .FontSize(10)
                .FontColor(Navy)
                .LineHeight(1.4f);
        });
    }

    // JOIN ADDRESS LINES WITHOUT EXTRA COMMAS
    private static string JoinNonEmpty(params string?[] values)
    {
        return string.Join(", ",
            values.Where(v => !string.IsNullOrWhiteSpace(v)));
    }

    // GET A HUMAN-READABLE NAME FROM DEGREE / TERM ENTITIES
    private static string GetDisplayName(object? value)
    {
        if (value == null)
            return "Not provided";

        string[] propertyNames =
        {
            "Name",
            "Title",
            "DisplayName",
            "DegreeName",
            "TermName"
        };

        foreach (string propertyName in propertyNames)
        {
            PropertyInfo? property = value.GetType()
                .GetProperty(propertyName);

            if (property?.PropertyType == typeof(string))
            {
                string? result = property.GetValue(value) as string;

                if (!string.IsNullOrWhiteSpace(result))
                    return result;
            }
        }

        return value.ToString() ?? "Not provided";
    }
}
