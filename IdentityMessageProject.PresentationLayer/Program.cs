using IdentityMessageProject.BusinessLayer.Container;
using IdentityMessageProject.DataAccessLayer.Context;
using IdentityMessageProject.EntityLayer.Concrete;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityMessageContext>();
//AddEntityFrameworkStores: identity kütüphanesine ait yapacağımız işlemlere izin verecek olan komut
builder.Services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<IdentityMessageContext>().AddDefaultTokenProviders();

//ekleme yapılacak

builder.Services.ContainerDependencies();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Geliştirme ortamında daha ayrıntılı hata mesajları gösterir
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Hata sayfasını gösterir
}
else
{
    // Üretim ortamında genel hata sayfasına yönlendirir
    app.UseExceptionHandler("/Error/Index"); // Diğer hatalar için yönlendirme
    app.UseHsts(); // HTTP Strict Transport Security (HSTS)
}

//404 ve diğer HTTP hataları için yönlendirme
app.UseStatusCodePagesWithRedirects("/Error/Index");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
