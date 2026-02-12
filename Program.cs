var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURAÇÃO DE CORS PARA PRODUÇÃO ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("LiberarReact", policy =>
    {
      
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// ------------------------------------------

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// --- ATIVAÇÃO DO CORS ---
app.UseCors("LiberarReact");
// ------------------------

app.UseAuthorization();
app.MapControllers();

app.Run();


//var builder = WebApplication.CreateBuilder(args);


//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("LiberarReact", policy =>
//    {
//        policy.WithOrigins("http://localhost:3000") // URL padrão do React
//              .AllowAnyHeader()
//              .AllowAnyMethod();
//    });
//});
//// -------------------------------

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseHttpsRedirection();


//app.UseCors("LiberarReact");
//// ------------------------------------------------------------

//app.UseAuthorization();
//app.MapControllers();
//app.Run();
