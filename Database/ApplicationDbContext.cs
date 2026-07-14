

using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database.Models.Application;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Students;

namespace StudentPortalPracticeTwo.Database;


public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // USER Accounts - created after final application is approved
    public DbSet<UserModel> UserDb { get; set; }
    public DbSet<UserContactModel> UserContactDb { get; set; }

    // FINAL Application - created after student submits DRAFT application
    public DbSet<ApplicationModel> ApplicationDb { get; set; }
    public DbSet<StudentInfoModel> StudentInfoDb { get; set; }
    public DbSet<StudentContactModel> StudentContactDb { get; set; }

    // DRAFT Application - used to save student progress in their application
    public DbSet<DraftApplicationModel> DraftApplicationDb { get; set; }
    public DbSet<DraftStudentInfoModel> DraftStudentInfoDb { get; set; }
    public DbSet<DraftStudentContactModel> DraftStudentContact { get; set; }

    // DEGREES 
    public DbSet<Degree> DegreeDb { get; set; }
    public DbSet<Course> CourseDb { get; set; }
    public DbSet<ClassSession> ClassSessionDb { get; set; }


    // MODEL BUILDER
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // USER
        modelBuilder.Entity<UserModel>()
            .HasOne(x => x.ContactDetails)
            .WithOne(x => x.User)
            .HasForeignKey<UserContactModel>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserModel>()
            .HasOne(x => x.OriginalFinalApplication)
            .WithMany()
            .HasForeignKey(x => x.FinalApplicationId);
        
        // Final
        modelBuilder.Entity<ApplicationModel>()
            .HasOne(x => x.StudentInfo)
            .WithOne(x => x.Application)
            .HasForeignKey<StudentInfoModel>(x => x.Id);

        modelBuilder.Entity<ApplicationModel>()
            .HasOne(x => x.StudentContact)
            .WithOne(x => x.Application)
            .HasForeignKey<StudentContactModel>(x => x.Id);

        // Draft
        modelBuilder.Entity<DraftApplicationModel>()
            .HasOne(x => x.DraftStudentInfo)
            .WithOne(x => x.Application)
            .HasForeignKey<DraftStudentInfoModel>(x => x.Id);

        modelBuilder.Entity<DraftApplicationModel>()
            .HasOne(x => x.DraftStudentContact)
            .WithOne(x => x.Application)
            .HasForeignKey<DraftStudentContactModel>(x => x.Id);

        // Degree
        modelBuilder.Entity<Degree>()
            .HasMany(x => x.Courses)
            .WithMany(x => x.Degrees);

        modelBuilder.Entity<Course>()
            .HasMany(x => x.Sessions)
            .WithOne(x => x.Course)
            .HasForeignKey(x => x.CourseId);
    }
}





