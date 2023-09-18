using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using NLog;
using WebApiCourse.WebFramework.Middlewares;
using WebFramework.Configuration.Swagger;

namespace WebFramework.Configuration
{
    public static class WebApplicationExtensions
    {

        public static WebApplication Configure(this WebApplication app)
        {
            var logger = LogManager.Setup().LoadConfigurationFromFile("nlog.config").GetCurrentClassLogger();

            try
            {
                // Exception handling middleware should come first
                app.UseNAPExceptionHandlerMiddleware();

                // Enable HTTPS and HSTS for secure connections
                app.UseHttpsRedirection();
                app.UseHsts();

                // CORS middleware should be configured before routing
                app.UseCors();

                // Serve static files (e.g., HTML, CSS, JS)
                app.UseStaticFiles();

                // Routing should be before authentication and authorization
                app.UseRouting();

                // Authentication middleware should come after routing
                app.UseAuthentication();

                // Authorization middleware should follow authentication
                app.UseAuthorization();

                // Razor Pages for server-side rendering
                app.MapRazorPages();

                // Swagger UI for development environment
                if (app.Environment.IsDevelopment())
                {
                    app.UseSwaggerAndUI();
                }

                // Controller routing
                app.MapControllers();

                // Serve a fallback file if no routes match
                app.MapFallbackToFile("index.html");

                // Custom headers middleware (consider its position carefully)
                app.UseNAPCustomHeadersMidleware();

                // Additional custom middleware (if needed) can be added here

                return app;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }
    }
}
