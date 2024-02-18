namespace OpusSharp.Enums
{
    /// <summary>
    /// Defines the opus signal.
    /// </summary>
    public enum OpusSignal : int
    {
        /// <summary>
        /// Auto/default setting.
        /// </summary>
        Auto = -1000,
        /// <summary>
        /// Signal being encoded is voice.
        /// </summary>
        Voice = 3001,
        /// <summary>
        /// Signal being encoded is music.
        /// </summary>
        Music = 3002,
    }
}
