using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;


namespace Happy5ChatTest.DataContext
{
    public static class DbExtension
    {
        public static void UpdateDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {

                using (var context = serviceScope.ServiceProvider.GetService<APIDbContext>())
                {
                    context.Database.Migrate();
                }


            }


        }
    }
}
