using Dapper;
using Servicios_Medicos.Repository;
using Servicios_Medicos.Services;
using Servicios_Medicos.Services.Abstract;
using ServiciosMedicos.Services;
using ServiciosMedicos.Services.Abstract;

DefaultTypeMap.MatchNamesWithUnderscores = true;

var builder = WebApplication.CreateBuilder(args);
DefaultTypeMap.MatchNamesWithUnderscores = true;
// daniel
builder.Services.AddScoped<PuestosRepository>();
builder.Services.AddScoped<RequisitosRepository>();
builder.Services.AddScoped<EmpleadosRepository>();
builder.Services.AddScoped<AccionesPersonalRepository>();

builder.Services.AddScoped<IPuestos, PuestosService>();
builder.Services.AddScoped<IRequisitos, RequisitosService>();
builder.Services.AddScoped<IEmpleados, EmpleadosService>();
builder.Services.AddScoped<IAccionesPersonal, AccionesPersonalService>();


// daniel
// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<SeguridadRepository>();
builder.Services.AddScoped<BitacoraConsultaRepository>();
builder.Services.AddScoped<UbicacionRepository>();
builder.Services.AddScoped<IUbicacion, UbicacionServices>();
builder.Services.AddScoped<IUsuario, AutenticacionServices>();
builder.Services.AddScoped<EncriptadorAESServices>();
builder.Services.AddScoped<ParametroBD>();
builder.Services.AddScoped<IParametro, ParametroServices>();

// Erick - SEG4 Administración de roles
builder.Services.AddScoped<RolesRepository>();
builder.Services.AddScoped<IRoles, RolesServices>();

// Erick - SEG5 Administración de pantallas
builder.Services.AddScoped<PantallasRepository>();
builder.Services.AddScoped<IPantallas, PantallasServices>();

// Erick - SEG6 Administración de usuarios
builder.Services.AddScoped<UsuariosAdminRepository>();
builder.Services.AddScoped<IUsuariosAdmin, UsuariosAdminServices>();
builder.Services.AddScoped<IBitacora, BitacoraServices>();
// Persona C - Kenneth: Registro de dependencias para GEN5 Instituciones educativas.
builder.Services.AddScoped<InstitucionEducativaRepository>();
builder.Services.AddScoped<IInstitucionEducativaService, InstitucionEducativaService>();
// Persona C - Kenneth: Registro de dependencias para OFE2 Concursos.
builder.Services.AddScoped<ConcursoRepository>();
builder.Services.AddScoped<IConcursoService, ConcursoService>();
// Persona C - Kenneth: Registro de dependencias para OFE1 Oferentes.
builder.Services.AddScoped<OferenteRepository>();
builder.Services.AddScoped<IOferenteService, OferenteService>();
// Persona C - Kenneth: Registro de dependencias para OFE3 Preparacion academica.
builder.Services.AddScoped<PreparacionAcademicaRepository>();
builder.Services.AddScoped<IPreparacionAcademicaService, PreparacionAcademicaService>();

// GEN3 - Compañías
builder.Services.AddScoped<BitacoraRepository>();
builder.Services.AddScoped<CompaniaRepository>();
builder.Services.AddScoped<CompaniaService>();

// OFE4 - Experiencia laboral
builder.Services.AddScoped<ExperienciaLaboralRepository>();
builder.Services.AddScoped<ExperienciaLaboralService>();

// EMP4 - Áreas
builder.Services.AddScoped<AreaRepository>();
builder.Services.AddScoped<AreaService>();

// OFE5 - Entrevistas
builder.Services.AddScoped<EntrevistaRepository>();
builder.Services.AddScoped<EntrevistaService>();

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
