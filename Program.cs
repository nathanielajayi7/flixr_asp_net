using Flixr;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World! Welcome to FlixrTest");

app.MapGet("/videos", async () => await VideoController.getMovies());

app.MapGet("/series", async () => await VideoController.getSeries());

app.Run();