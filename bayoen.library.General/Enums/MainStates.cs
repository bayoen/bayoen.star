namespace bayoen.library.General.Enums
{
    public enum MainStates : int
    {
        Invalid = -3,
        MissingProcess,
        MissingAddress,

        None,
        Offline,
        Title,

        MainMenu,
        Adventure,
        SoloArcade,
        MultiArcade,
        Option,
        Online,
        Lessons,

        Loading,
        CharacterSetection,

        PuzzleLeague,
        FreePlay,

        OnlineReplay,
        LocalReplay,
    }
}
