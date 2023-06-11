public delegate void onPossessedCharacterChanged(Character character);
public static class EventManager
{
    public static event onPossessedCharacterChanged OnPossessedCharacterChanged;

    public static void OnPossessedCharacterChangedCall(Character character)
    {
        OnPossessedCharacterChanged?.Invoke(character);
    }
}
