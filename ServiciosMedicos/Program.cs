
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Services;
using ServiciosMedicos.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);
// daniel
builder.Services.AddScoped<PuestosBD>();
builder.Services.AddScoped<RequisitosBD>();
builder.Services.AddScoped<EmpleadosBD>();
builder.Services.AddScoped<AccionesPersonalBD>();

builder.Services.AddScoped<IPuestos, PuestosService>();
//builder.Services.AddScoped<IRequisitos, RequisitosService>();
builder.Services.AddScoped<IEmpleados, EmpleadosService>();
builder.Services.AddScoped<IAccionesPersonal, AccionesPersonalService>();


// daniel
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<SeguridadBD>();
builder.Services.AddScoped<IUsuario, Autenticacion>();
builder.Services.AddScoped<EncriptadorAES>();

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
