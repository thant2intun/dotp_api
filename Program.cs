using AutoMapper;
using DOTP_BE.Data;
using DOTP_BE.Interfaces;
using DOTP_BE.MappingConfig;
using DOTP_BE.Model;
using DOTP_BE.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//swagger
builder.Services.AddControllers().AddNewtonsoftJson(op =>   
            op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddEndpointsApiExplorer();


var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
builder.Services.AddSingleton(emailConfig);

//DBConnection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

//JwtSetting
var jwtSetting = builder.Configuration.GetSection("JWTSetting");
builder.Services.Configure<JWTSetting>(jwtSetting);
var authKey = builder.Configuration.GetValue<string>("JWTSetting:securitykey");
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authKey)),
            ValidateIssuer = false, 
            ValidateAudience = false,
            ValidateLifetime = false
        };
});

var _dbcontext = builder.Services.BuildServiceProvider().GetService<ApplicationDbContext>();

//Service DI
//builder.Services.AddSingleton<IRefreshTokenGenerator>(provider => new RefreshTokenGenerator(_dbcontext));
builder.Services.AddAuthorization();
builder.Services.AddDistributedMemoryCache();
builder.Services.Configure<IISServerOptions>(options =>
{
    options.MaxRequestBodyBufferSize = 104857600; // 100 MB in bytes
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromSeconds(10);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;

});


MappingConfiguration(builder.Services);  // Adding AutoMapper config by Mm for Mapping

//Service DI Instant Sessions - MWL -
builder.Services.AddScoped<IRegistrationOffice, RegistrationOfficeRepo>();
builder.Services.AddScoped<IVehicleWeight, VehicleWeightRepo>();
builder.Services.AddScoped<INRC, NRCRepo>();
builder.Services.AddScoped<ILicenseType, LicenseTypeRepo>();
builder.Services.AddScoped<IJourneyType, JourneyTypeRepo>();
builder.Services.AddScoped<ITownship, TownshipRepo>();
builder.Services.AddScoped<IMDYCars, MDYCarsRepo>();
builder.Services.AddScoped<IKALA_YGNCars,KALA_YGNCarsRepo>();

builder.Services.AddScoped<IOperatorDetail,OperatorDetailRepo>();

builder.Services.AddScoped<IUser, UserRepo>();
builder.Services.AddTransient<IPersonInformation, PersonInformationRepo>();

builder.Services.AddScoped<IVehicleWeightFee, VehicleWeightFeeRepo>();
builder.Services.AddScoped<IFee, FeeRepo>();
builder.Services.AddScoped<IDelivery, DeliveryRepo>();
builder.Services.AddScoped<ICreateCar, CreateCarRepo>();
builder.Services.AddTransient<ILicenseOnly, LicenseOnlyRepo>();
builder.Services.AddTransient<IVehicle, VehicleRepo>();

builder.Services.AddTransient<IVehicleWeightFee, VehicleWeightFeeRepo>();
builder.Services.AddTransient<IFee, FeeRepo>();

//builder.Services.AddTransient<IVehicleWeightFee, VehicleWeightFeeRepo>();
//builder.Services.AddTransient<IFee, FeeRepo>();

// Adding by Mm
builder.Services.AddScoped<IMenus, MenusRepo>();
builder.Services.AddScoped<IRole, RolesRepo>();
builder.Services.AddScoped<IAdminUser, AdminUserRepo>();
builder.Services.AddScoped<IDashboard, DashboardServiceRepo>();
builder.Services.AddControllers().AddNewtonsoftJson();    // Adding for jsonconvert by Mm 
//builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
//     options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

//Adding by ema
builder.Services.AddScoped<IReportOut, ReportOutRepo>();

builder.Services.AddTransient<ITransaction, TransactionRepo>();// Code Comment MWl - 16-01-23
//builder.Services.AddDatabaseDeveloperPageExceptionFilter();  // Code Comment MWl

// Add services to the container.
//builder.Services.AddSwaggerGen(  c => c.SwaggerDoc("v1", new OpenApiInfo{ Title = "Api",Version = "v1",}); );
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
    "v1", new OpenApiInfo
    {
        Title = "Code First DB API",
        Version = "v1"
    });
    // add JWT Authentication
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);    
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

});

var app = builder.Build();

string folder_path = builder.Configuration.GetSection("Upload_FolderPath").Value;
if (!Directory.Exists(folder_path)) { Directory.CreateDirectory(folder_path); }

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(folder_path),
    RequestPath = "/StaticFiles"
});

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    // app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestWebApi"); 
//    app.UseSwaggerUI();
//}

app.UseSwagger();
// app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestWebApi"); 
app.UseSwaggerUI();

app.UseCors(builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
});

// Configure the HTTP request pipeline.
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.MapControllers();

//app.MapControllerRoute(
//    name: "default",
//    pattern: "api/{controller}/{action=Index}/{id?}");

app.Run();


static void MappingConfiguration(IServiceCollection services)
{
    var mapConfig = new MapperConfiguration(mp =>
    {
        mp.AddProfile(new MenuMap());
        mp.AddProfile(new RoleMap());
        mp.AddProfile(new AdminUserMap());
        mp.AddProfile(new RegistrationOfficeMap());
    });

    IMapper mapper = mapConfig.CreateMapper();
    services.AddSingleton(mapper);
}
