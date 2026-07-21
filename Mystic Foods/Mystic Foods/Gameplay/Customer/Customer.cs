namespace Mystic_Foods.Gameplay.Customer
{
    public sealed record Customer(
        int Id,
        string Name,
        int RequiredRecipeId,
        string HappySpritePath,
        string NeutralSpritePath,
        string GrumpySpritePath,
        string DialogueFirst,
        string DialogueSecond,
        string DialogueCorrect,
        string DialogueWrong
    );
}