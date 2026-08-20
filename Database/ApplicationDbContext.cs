

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StudentPortalPracticeTwo.Database.Models.Application;
using StudentPortalPracticeTwo.Database.Models.Degrees;
using StudentPortalPracticeTwo.Database.Models.Users.Admin;
using StudentPortalPracticeTwo.Database.Models.Users.Faculty;
using StudentPortalPracticeTwo.Database.Models.Users.Students;
using StudentPortalPracticeTwo.Database.Models.Users;

namespace StudentPortalPracticeTwo.Database;


public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // USER Accounts - created after final application is approved
    public DbSet<UserModel> UserDb { get; set; }
    public DbSet<Student> StudentDb { get; set; }
    public DbSet<Faculty> FacultyDb { get; set; }
    public DbSet<AdminModel> AdminDb { get; set; }
    public DbSet<UserContactModel> UserContactDb { get; set; }
    public DbSet<UserEmergencyContactModel> UserEmergencyDb { get; set; }
    public DbSet<UserProgramModel> UserProgram { get; set; }

    // Faculty
    public DbSet<Faculty> FacultyUsers { get; set; }

    // FINAL Application - created after student submits DRAFT application
    public DbSet<ApplicationModel> ApplicationDb { get; set; }
    public DbSet<StudentInfoModel> StudentInfoDb { get; set; }
    public DbSet<StudentContactModel> StudentContactDb { get; set; }
    public DbSet<EmergencyContactModel> EmergencyContactDb { get; set; }
    public DbSet<StudentProgram> StudentProgramDb { get; set; }
    public DbSet<StudentEssayModel> EssayDb { get; set; }

    // DRAFT Application - used to save student progress in their application
    public DbSet<DraftApplicationModel> DraftApplicationDb { get; set; }
    public DbSet<DraftStudentInfoModel> DraftStudentInfoDb { get; set; }
    public DbSet<DraftStudentContactModel> DraftStudentContact { get; set; }
    public DbSet<DraftEmergencyContactModel> DraftEmergencyContact { get; set; }
    public DbSet<DraftStudentProgram> DraftStudentProgram { get; set; }
    public DbSet<DraftAcademicHistoryModel> DraftAcademicHistoryDb { get; set; }
    public DbSet<DraftStudentEssayModel> DraftEssayDb { get; set; }

    // DEGREES 
    public DbSet<Degree> DegreeDb { get; set; }
    public DbSet<Course> CourseDb { get; set; }
    public DbSet<ClassSession> ClassSessionDb { get; set; }
    public DbSet<Term> TermDb { get; set; }


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
            .HasOne(x => x.IdentityUser)
            .WithMany()
            .HasForeignKey(x => x.IdentityUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserModel>()
            .HasMany(x => x.EmergencyContact)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);



        // Students
        modelBuilder.Entity<Student>()
            .HasOne(x => x.OriginalFinalApplication)
            .WithMany()
            .HasForeignKey(x => x.FinalApplicationId);

        modelBuilder.Entity<Student>()
            .HasOne(x => x.MyProgram)
            .WithOne(x => x.User)
            .HasForeignKey<UserProgramModel>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserProgramModel>()
            .HasOne(x => x.MyDegree)
            .WithMany(x => x.StudentPrograms)
            .HasForeignKey(x => x.DegreeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserProgramModel>()
            .HasMany(x => x.CompletedCourses)
            .WithMany();

        modelBuilder.Entity<UserProgramModel>()
            .HasMany(x => x.CurrentSessions)
            .WithMany(x => x.StudentProgramModels);

        modelBuilder.Entity<UserProgramModel>()
            .HasMany(x => x.RegisteredSessions)
            .WithMany(x => x.RegisteredStudentProgramModels);

        // Faculty
        modelBuilder.Entity<Faculty>()
            .HasMany(x => x.ClassSessions)
            .WithOne(x => x.Instructor)
            .HasForeignKey(x => x.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        // Admins
        

        // Final
        modelBuilder.Entity<ApplicationModel>()
            .HasOne(x => x.StudentInfo)
            .WithOne(x => x.Application)
            .HasForeignKey<StudentInfoModel>(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicationModel>()
            .HasOne(x => x.StudentContact)
            .WithOne(x => x.Application)
            .HasForeignKey<StudentContactModel>(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicationModel>()
            .HasMany(x => x.EmergencyContact)
            .WithOne(x => x.Application)
            .HasForeignKey(x => x.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicationModel>()
            .HasOne(x => x.StudentProgram)
            .WithOne(x => x.Application)
            .HasForeignKey<StudentProgram>(x => x.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ApplicationModel>()
            .HasOne(x => x.AcademicHistory)
            .WithOne(x => x.Application)
            .HasForeignKey<AcademicHistoryModel>(x => x.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<ApplicationModel>()
            .HasOne(x => x.Essays)
            .WithOne(x => x.Application)
            .HasForeignKey<StudentEssayModel>(x => x.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);
        

        // Draft
        modelBuilder.Entity<DraftApplicationModel>()
            .HasOne(x => x.DraftStudentInfo)
            .WithOne(x => x.Application)
            .HasForeignKey<DraftStudentInfoModel>(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DraftApplicationModel>()
            .HasOne(x => x.DraftStudentContact)
            .WithOne(x => x.Application)
            .HasForeignKey<DraftStudentContactModel>(x => x.Id)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DraftApplicationModel>()
            .HasMany(x => x.DraftEmergencyContact)
            .WithOne(x => x.Application)
            .HasForeignKey(x => x.DraftApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DraftApplicationModel>()
            .HasOne(x => x.DraftProgramSelection)
            .WithOne(x => x.Application)
            .HasForeignKey<DraftStudentProgram>(x => x.ApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DraftStudentProgram>()
            .HasOne(x => x.SelectedProgram)
            .WithMany()
            .HasForeignKey(x => x.SelectedProgramId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DraftStudentProgram>()
            .HasOne(x => x.StartTerm)
            .WithMany()
            .HasForeignKey(x => x.StartTermId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DraftApplicationModel>()
            .HasOne(x => x.DraftAcademicHistory)
            .WithOne(x => x.DraftApplication)
            .HasForeignKey<DraftAcademicHistoryModel>(x => x.DraftApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<DraftApplicationModel>()
            .HasOne(x => x.DraftEssays)
            .WithOne(x => x.DraftApplication)
            .HasForeignKey<DraftStudentEssayModel>(x => x.DraftApplicationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Degree
        modelBuilder.Entity<Degree>()
            .HasMany(x => x.Courses)
            .WithMany(x => x.Degrees);

        modelBuilder.Entity<Course>()
            .HasMany(x => x.Sessions)
            .WithOne(x => x.Course)
            .HasForeignKey(x => x.CourseId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ClassSession>()
            .HasOne(x => x.Term)
            .WithMany(x => x.ClassSessions)
            .HasForeignKey(x => x.TermId)
            .OnDelete(DeleteBehavior.Restrict);

    }
}





