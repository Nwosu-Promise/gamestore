using GameStoore.Api.Dtos;

namespace GameStoore.Api.Endpoints;

public static class GamesEndpoints
{
  const string GetGameEndpoint = "GetGame";

  private static readonly List<GameDto> games = [
    new(
    1,
    "Street fighter II",
    "Fighting",
    19.99M,
    new DateOnly(1992, 7, 15)
  ),
  new(
    2,
    "Street cook II",
    "Cooking",
    59.99M,
    new DateOnly(1999, 7, 15)
  ),
  new(
    3,
    "Street Racer",
    "Adventure",
    29.99M,
    new DateOnly(1992, 7, 15)
  ),
];

  public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
  {

    var group = app.MapGroup("games");
    // get games
    group.MapGet("/", () => games);

    // get game by id
    group.MapGet("/{id}", (int id) =>
    {
      GameDto? game = games.Find(games => games.Id == id);

      return game is null ? Results.NotFound() : Results.Ok(game);

    }
    ).WithName(GetGameEndpoint);


    // POST /gmaes 
    group.MapPost("/", (CreateGameDto newGame) =>
    {
      GameDto game = new(
        games.Count + 1,
        newGame.Name,
        newGame.Genre,
        newGame.Price,
        newGame.ReleaseDate
      );

      games.Add(game);

      return Results.CreatedAtRoute("GetGame", new { id = game.Id }, game);
    });

    // PUT /games/:id
    group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
    {
      var index = games.FindIndex(game => game.Id == id);

      if (index == -1)
      {
        return Results.NotFound();
      }
      games[index] = new GameDto(
        id,
        updatedGame.Name,
        updatedGame.Genre,
        updatedGame.Price,
        updatedGame.ReleaseDate
      );

      return Results.NoContent();
    });

    group.MapDelete("/{id}", (int id) =>
    {
      games.RemoveAll(game => game.Id == id);

      return Results.NoContent();
    });

    return group;
  }

}
