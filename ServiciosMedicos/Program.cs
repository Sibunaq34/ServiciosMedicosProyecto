using Dapper;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services;
using Servicios_Medicos.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);
DefaultTypeMap.MatchNamesWithUnderscores = true;
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<SeguridadBD>();
builder.Services.AddScoped<IUsuario, Autenticacion>();
builder.Services.AddScoped<EncriptadorAES>();
// Erick - SEG4 Administración de roles
builder.Services.AddScoped<RolesBD>();
builder.Services.AddScoped<IRoles, Roles>();

// Erick - SEG5 Administración de pantallas
builder.Services.AddScoped<PantallasBD>();
builder.Services.AddScoped<IPantallas, Pantallas>();

// Erick - SEG6 Administración de usuarios
builder.Services.AddScoped<UsuariosAdminBD>();
builder.Services.AddScoped<IUsuariosAdmin, UsuariosAdmin>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapRazorPages();
app.Run();
