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

app.MapGet("/movies", async (HttpContext context) =>
{
    return await VideoController.getMovies(context);
});


app.MapGet("/search", async (HttpContext context) =>
{
    return await VideoController.searchResult(context);
});

app.MapGet("/series", async (HttpContext context) =>
{
    return await VideoController.getSeries(context);
});

app.Run();