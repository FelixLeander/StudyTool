using Serilog;
using StudyTool.BusinessLogic;
using StudyTool.Data;

namespace StudyTool;

public class Startup
{
    public static void Main(string[] args)
    {
        var loggerConfig = new LoggerConfiguration()
            .MinimumLevel.Verbose()
                .WriteTo.Console()
                .WriteTo.File($"logs/{AppValues.AppNameUsermachineFriendly}.log", Serilog.Events.LogEventLevel.Information, rollingInterval: RollingInterval.Day)
                .CreateLogger();

        try
        {
            Log.Information("Starting with command line arguments {args}.", args);

            var builder = WebApplication.CreateBuilder(args);
            builder.Host.UseSerilog(loggerConfig);

            if (builder.Environment.IsDevelopment())
                builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //Configure CardDatabase
            builder.Services.AddEntityFrameworkSqlite()
                .AddDbContext<DatabaseContext>();

            using (var dbContext = new DatabaseContext())
            {
                dbContext.Database.EnsureCreated();
            }

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<HashManager>();
            builder.Services.AddScoped<DataAccessor>(); //Since the only persistent data is a logger, the instance could be marked as a singleton.

            var app = builder.Build();
            app.UseSerilogRequestLogging();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Common/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();
            app.MapRazorPages();
            app.Run();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Log.Fatal(ex, "Application terminated unexpectedly:");
            Console.WriteLine(ex.Message);
        }
        finally
        {
            Console.ForegroundColor = default;
            Log.CloseAndFlush();
        }
    }
}