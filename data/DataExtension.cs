using System;
using Microsoft.EntityFrameworkCore;

namespace KpitApi.data;

public static class DataExtension
{

    public static void UpdateDb(this WebApplication app)
    {
        var scope = app.Services.CreateScope();

        var db = scope.ServiceProvider.GetRequiredService<KpitStoreContext>();

        db.Database.Migrate();
    }
    public static void AddDbContext(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<KpitStoreContext>(
            options => options.UseNpgsql(
                builder.Configuration.GetConnectionString("PgsqlConnection")
            )
        );

    }
}
