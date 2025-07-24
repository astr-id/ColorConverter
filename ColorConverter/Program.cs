using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using ColorConverter.Data; 
using ColorConverter.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("MyCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization();

builder.Services.AddDbContext<ColorContext>(opt =>
    opt.UseInMemoryDatabase("ColorConverterDb"));

builder.Services.AddScoped<IColorService, ColorService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Exemple: Bearer {token}"
    });

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

    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "ColorConverter API",
        Description = "Une API pour convertir des couleurs entre differents formats (HEX, RGB, HSL, LAB, etc.)",
        TermsOfService = new Uri("https://google.com"),
        Contact = new OpenApiContact
        {
            Name = "Clostrid",
            Email = "clostrid@mail.com",
            Url = new Uri("https://google.com")
        }
    });

    // Ajout des commentaires XML (si activ�s dans .csproj)
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.InjectStylesheet("/css/swagger-custom.css"); 
    });
}

app.UseHttpsRedirection();
app.UseCors("MyCorsPolicy");
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseDefaultFiles();
app.MapControllers();

app.Run();
