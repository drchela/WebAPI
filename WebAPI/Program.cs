using Microsoft.EntityFrameworkCore;
using WebAPI.Data;

//declarar cors para permitir solicitudes desde nuestra app
//Declarar el lugar donde viene el front

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

var builder = WebApplication.CreateBuilder(args);

//agregando la configuracion de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:5173") // agrega la URL de tu frontend
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Add services to the container.
// agregamos el servicio de Entity Framework Core SQL server y la cadena de conexión
builder.Services.AddDbContext<ApiDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbWebApi")));
builder.Services.AddControllers();

var app = builder.Build();


// Configure the HTTP request pipeline.
app.UseCors(MyAllowSpecificOrigins);
app.UseAuthorization();

app.MapControllers();

app.Run();
