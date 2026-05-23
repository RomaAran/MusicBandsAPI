using Kursovaya_var6.Data;
using Kursovaya_var6.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddDbContext<MBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

var app = builder.Build();

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<MBContext>();

    context.Bands.RemoveRange(context.Bands);
    context.SaveChanges();

    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Bands', RESEED, 0)");

    context.GroupNames.RemoveRange(context.GroupNames);
    context.SaveChanges();

    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('GroupNames', RESEED, 0)");
    

    context.Leaders.RemoveRange(context.Leaders);
    context.SaveChanges();

    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Leaders', RESEED, 0)");

    context.Genres.RemoveRange(context.Genres);
    context.SaveChanges();

    context.Database.ExecuteSqlRaw("DBCC CHECKIDENT ('Genres', RESEED, 0)");
    context.SaveChanges();

    context.GroupNames.AddRange(
        new GroupName { Name = "Metallica" },
        new GroupName { Name = "Nirvana" },
        new GroupName { Name = "Queen" },
        new GroupName { Name = "Linkin Park" },
        new GroupName { Name = "Rammstein" },
        new GroupName { Name = "Coldplay" },
        new GroupName { Name = "Imagine Dragons" },
        new GroupName { Name = "Radiohead" },
        new GroupName { Name = "AC/DC" },
        new GroupName { Name = "The Beatles" }
    );

    context.Leaders.AddRange(
        new Leader { Name = "James Hetfield" },
        new Leader { Name = "Kurt Cobain" },
        new Leader { Name = "Freddie Mercury" },
        new Leader { Name = "Chester Bennington" },
        new Leader { Name = "Till Lindemann" },
        new Leader { Name = "Chris Martin" },
        new Leader { Name = "Dan Reynolds" },
        new Leader { Name = "Thom Yorke" },
        new Leader { Name = "Angus Young" },
        new Leader { Name = "John Lennon" }
    );

    context.Genres.AddRange(
        new Genre { Name = "Metal" },
        new Genre { Name = "Rock" },
        new Genre { Name = "Alternative" },
        new Genre { Name = "Industrial Metal" },
        new Genre { Name = "Pop Rock" }
    );

    context.SaveChanges();

    context.Bands.AddRange(
        new Band { GroupNameId = 1, LeaderId = 1, AlbumsCount = 11, GenreId = 1 },
        new Band { GroupNameId = 2, LeaderId = 2, AlbumsCount = 3, GenreId = 2 },
        new Band { GroupNameId = 3, LeaderId = 3, AlbumsCount = 15, GenreId = 2 },
        new Band { GroupNameId = 4, LeaderId = 4, AlbumsCount = 7, GenreId = 3 },
        new Band { GroupNameId = 5, LeaderId = 5, AlbumsCount = 8, GenreId = 4 },
        new Band { GroupNameId = 6, LeaderId = 6, AlbumsCount = 9, GenreId = 5 },
        new Band { GroupNameId = 7, LeaderId = 7, AlbumsCount = 6, GenreId = 3 },
        new Band { GroupNameId = 8, LeaderId = 8, AlbumsCount = 10, GenreId = 3 },
        new Band { GroupNameId = 9, LeaderId = 9, AlbumsCount = 18, GenreId = 2 },
        new Band { GroupNameId = 10, LeaderId = 10, AlbumsCount = 13, GenreId = 2 }
    );

    context.SaveChanges();
}

app.MapGet("/", () => Results.Redirect("/index.html"));

app.Run();