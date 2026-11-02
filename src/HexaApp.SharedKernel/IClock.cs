namespace HexaApp.SharedKernel;

/// <summary>Horloge injectée : le domaine ne lit jamais l'heure système directement, les tests la fixent.</summary>
public interface IClock
{
    DateTimeOffset UtcNow { get; }
}
