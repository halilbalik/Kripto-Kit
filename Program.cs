var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Kriptografi servislerini ekle
builder.Services.AddScoped<Bilgi_Güvenliği_ve_Kriptoloji.Services.IAESService, Bilgi_Güvenliği_ve_Kriptoloji.Services.AESService>();
builder.Services.AddScoped<Bilgi_Güvenliği_ve_Kriptoloji.Services.IRSAService, Bilgi_Güvenliği_ve_Kriptoloji.Services.RSAService>();
builder.Services.AddScoped<Bilgi_Güvenliği_ve_Kriptoloji.Services.ISHAService, Bilgi_Güvenliği_ve_Kriptoloji.Services.SHAService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
