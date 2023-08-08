using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using StudyPlatform.Data;
using StudyPlatform.Data.Models;
using StudyPlatform.Services.Category;
using StudyPlatform.Services.Category.Interfaces;
using StudyPlatform.Services.Course;
using StudyPlatform.Services.Course.Interfaces;
using StudyPlatform.Services.LearningMaterial;
using StudyPlatform.Services.LearningMaterial.Interfaces;
using StudyPlatform.Services.Lesson;
using StudyPlatform.Services.Lesson.Interfaces;
using StudyPlatform.Services.Roles;
using StudyPlatform.Services.Roles.Interfaces;
using StudyPlatform.Services.TeacherLesson;
using StudyPlatform.Services.TeacherLesson.Intefaces;
using StudyPlatform.Services.Users;
using StudyPlatform.Services.Users.Interfaces;
using static StudyPlatform.Common.GeneralConstants;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
        builder.Services.AddDbContext<StudyPlatformDbContext>(options =>
            options.UseSqlServer(
                connectionString,
                db => db.MigrationsAssembly("StudyPlatform.Data")) // or just "StudyPlatform.Data"
        );
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
        {
            options.SignIn.RequireConfirmedAccount
                = builder.Configuration.GetValue<bool>("Identity:SignIn:RequireConfirmedAccount");
            options.Password.RequireDigit
                = builder.Configuration.GetValue<bool>("Identity:Password:RequireDigit");
            options.Password.RequireLowercase
                = builder.Configuration.GetValue<bool>("Identity:Password:RequireLowercase");
            options.Password.RequireUppercase
                = builder.Configuration.GetValue<bool>("Identity:Password:RequireUppercase");
            options.Password.RequiredLength
                = builder.Configuration.GetValue<int>("Identity:Password:RequiredLength");
        })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<StudyPlatformDbContext>()
            .AddUserManager<UserManager<ApplicationUser>>()
            .AddRoleManager<RoleManager<IdentityRole<Guid>>>()
            .AddEntityFrameworkStores<StudyPlatformDbContext>();
        builder.Services.AddControllersWithViews(options =>
        {
            options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
        });


        builder.Services.AddHttpContextAccessor();
        builder.Services.AddMemoryCache();

        builder.Services.AddScoped<ICategoryViewService, CategoryViewService>();
        builder.Services.AddScoped<ICategoryViewFormService, CategoryViewFormService>();
        builder.Services.AddScoped<ICourseViewService, CourseViewService>();
        builder.Services.AddScoped<ICourseViewFormService, CourseViewFormService>();
        builder.Services.AddScoped<ITeacherService, TeacherService>();
        builder.Services.AddScoped<ITeacherFormService, TeacherFormService>();
        builder.Services.AddScoped<ITeacherLessonService, TeacherLessonService>();
        builder.Services.AddScoped<IUserService, UserService>();
        builder.Services.AddScoped<ILearningMaterialService, LearningMaterialService>();
        builder.Services.AddScoped<ILearningMaterialFormService, LearningMaterialFormService>();
        builder.Services.AddScoped<ILessonViewService, LessonViewService>();
        builder.Services.AddScoped<ILessonFormService, LessonFormService>();
        builder.Services.AddScoped<IRoleService, RoleService>();

        builder.Services.AddAutoMapper(typeof(Program).Assembly);

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("Home/Error/500");
            app.UseStatusCodePagesWithRedirects("Home/Error?statusCode={0}");
            //app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseStatusCodePages();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "LearningMaterial ShowLearningMaterial",
                pattern: "LearningMaterial/ShowLearningMaterial/{id}/{linkname}",
                defaults: new { Controller = "LearningMaterial", Action = "ShowLearningMaterial" }
            );

            endpoints.MapControllerRoute(
                name: "Category Specific",
                pattern: "Category/{id}/{categoryName}",
                defaults: new { Controller = "Category", Action = "GetById" }
            );

            endpoints.MapControllerRoute(
                name: "Course Specific",
                pattern: "Course/{id}/{courseName}",
                defaults: new { Controller = "Course", Action = "GetCourse" }
            );

            endpoints.MapControllerRoute(
                name: "Lesson Specific",
                pattern: "Lesson/{id}/{lessonName}",
                defaults: new { Controller = "Lesson", Action = "GetLesson" }
            );

            endpoints.MapControllerRoute(
                name: "Account Profile Viewer",
                pattern: "Account/{username}",
                defaults: new { Controller = "Account", Action = "GetProfile" }
            );

            endpoints.MapDefaultControllerRoute();
            endpoints.MapRazorPages();
        });

        using (var scope = app.Services.CreateScope())
        {
            RoleManager<IdentityRole<Guid>> roleManager
                = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

            ICollection<string> roles
                = new List<string>() { AdministratorRoleName, TeacherRoleName, UserRoleName };

            foreach (string role in roles)
            {
                bool roleExists = await roleManager.RoleExistsAsync(role);

                if (!roleExists)
                {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole<Guid>(role));

                    if (!result.Succeeded)
                    {
                        throw new InvalidOperationException($"Could not create new role: {role}");
                    }
                }
            }
        }

        app.Run();
    }
}