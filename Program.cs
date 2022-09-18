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

app.MapGet("/movies", async (context) => await VideoController.getMovies(context));


app.MapGet("/search", async (context) => await VideoController.searchResult(context));

app.MapGet("/series", async (context) => await VideoController.getSeries(context));

app.Run();