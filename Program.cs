using TuitorialCrud.Data;
using TuitorialCrud.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var conString = builder.Configuration.GetConnectionString("GameStore");

builder.Services.AddSqlite<GameStoreContext>(conString);

var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
