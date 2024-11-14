using System;
using TuitorialCrud.DTOs;

namespace TuitorialCrud.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpoint = "GetGame";

    private static readonly List<GameDTO> games = [
    new(1,"Street Fighter II", "Fighting",19.99M,new DateOnly(1992,7,15)),
    new(2,"Final Fantasy XIV","Roleplaying",59.99M,new DateOnly(2010,9,2)),
    new(3,"FIFA 23","Sports",199.99M,new DateOnly(2023,7,15)),
];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app){

        var group = app.MapGroup("games").WithParameterValidation();
        //Read or GET Games
        group.MapGet("/", () => games);

        //Read or GET Game by ID
        group.MapGet("/{id}", (int id) =>
        {
            GameDTO? game = games.Find(game => game.Id == id);
            return game is null ? Results.NotFound() : Results.Ok(game);
        })
        .WithName(GetGameEndpoint);

        //Create Game or POST API
        group.MapPost("/", (CreateGameDTO newGame) =>
        {

            GameDTO game = new(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate
            );

            games.Add(game);
            return Results.CreatedAtRoute(GetGameEndpoint, new { id = game.Id }, game);
        }).WithParameterValidation();

        //Update Game or PUT API
        group.MapPut("/{id}", (int id, UpdateGameDTO updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);
            if (index == -1)
            {
                return Results.NotFound();
            }
            games[index] = new GameDTO(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );
            return Results.NoContent();
        });

        //Delete Game or DELETE API
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);
            return Results.NoContent();
        });
        return group;
    }

}
