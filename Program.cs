var builder = WebApplication.CreateBuilder(args);

// --- CONFIGURAÇÃO DE CORS PARA PRODUÇÃO ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("LiberarReact", policy =>
    {
        // AllowAnyOrigin permite que seu site no Netlify acesse a API
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

// Removemos o "if IsDevelopment" do Swagger caso você queira testar a API 
// direto pelo navegador no Render, mas você pode manter se preferir.
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

//// --- ADICIONE ESTA LINHA AQUI (Antes do UseAuthorization) ---
//app.UseCors("LiberarReact");
//// ------------------------------------------------------------

//app.UseAuthorization();
//app.MapControllers();
//app.Run();
