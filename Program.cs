using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using NuitInfo2022;
using NuitInfo2022.Models.Entities;
using System.IO;
using System.Net.Sockets;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllersWithViews();
        builder.Services.AddDirectoryBrowser();

        builder.Configuration
            .SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("Secrets.json");

        builder.Services.AddDbContext<ApplicationDbContext>(options => 
        options.UseSqlServer(builder.Configuration.GetConnectionString("DBString"),
            sqlServerOptionsAction: option => option.EnableRetryOnFailure()
        ));

        builder.Services.AddSession();

        var app = builder.Build();
        

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        var fileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.WebRootPath, "GameData"));
        var requestPath = "/GameData";

        // Set up custom content types - associating file extension to MIME type
        var provider = new FileExtensionContentTypeProvider();
        // Add new mappings
        provider.Mappings[".wasm.br"] = "application/wasm";
        provider.Mappings[".js.br"] = "application/javascript";
        provider.Mappings[".data.br"] = "application/octet";

        // Enable displaying browser links.
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = fileProvider,
            RequestPath = requestPath,
            ContentTypeProvider = provider,
            ServeUnknownFileTypes= true,
            RedirectToAppendTrailingSlash = true,
            OnPrepareResponse = (context) =>
            {
                context.Context.Response.Headers["content-encoding"] = context.Context.Request.Path.HasValue && context.Context.Request.Path.Value.EndsWith(".br") ? "br" : context.Context.Response.Headers["content-encoding"];
                context.Context.Response.Headers["content-type"] = context.Context.Request.Path.HasValue && context.Context.Request.Path.Value.EndsWith(".wasm.br") ? "application/wasm" : context.Context.Response.Headers["content-type"];
                context.Context.Response.Headers["content-type"] = context.Context.Request.Path.HasValue && context.Context.Request.Path.Value.EndsWith(".js.br") ? "application/javascript" : context.Context.Response.Headers["content-type"];
                context.Context.Response.Headers["content-type"] = context.Context.Request.Path.HasValue && context.Context.Request.Path.Value.EndsWith(".data.br") ? "application/octet" : context.Context.Response.Headers["content-type"];
            }
        });
        
        app.UseDirectoryBrowser(new DirectoryBrowserOptions
        {
            FileProvider = fileProvider,
            RequestPath = requestPath
        });

        app.UseStaticFiles();
        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseAuthorization();

        app.UseSession();


    
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        app.Run();
        
    }
}