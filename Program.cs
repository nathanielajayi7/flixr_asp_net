using Flixr;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World! Welcome to FlixrTest");

app.MapGet(
    "/movies/popular",
    async ()
    => await VideoController.getPopularMovies()
);

app.MapGet("/movies/{id}", async (string id) =>
{
    return await VideoController.getMovieById(id);


});

app.MapGet("/movies", async () => await VideoController.getMovies());

app.MapGet("/series", async () => await VideoController.getSeries());

app.Run();