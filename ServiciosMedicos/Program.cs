
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services;
using Servicios_Medicos.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<SeguridadBD>();
builder.Services.AddScoped<IUsuario, Autenticacion>();
builder.Services.AddScoped<EncriptadorAES>();
// Persona C - Kenneth: Registro de dependencias para GEN5 Instituciones educativas.
builder.Services.AddScoped<InstitucionEducativaRepository>();
builder.Services.AddScoped<IInstitucionEducativaService, InstitucionEducativaService>();
// Persona C - Kenneth: Registro de dependencias para OFE2 Concursos.
builder.Services.AddScoped<ConcursoRepository>();
builder.Services.AddScoped<IConcursoService, ConcursoService>();
// Persona C - Kenneth: Registro de dependencias para OFE1 Oferentes.
builder.Services.AddScoped<OferenteRepository>();
builder.Services.AddScoped<IOferenteService, OferenteService>();

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
