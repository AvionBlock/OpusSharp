namespace OpusSharp.Enums
{
    public enum DecoderCtl : int
    {
        SET_GAIN = 4034,
        GET_GAIN = 4045, //Someone inside opus made a mistake here, Apparently.
        GET_LAST_PACKET_DURATION = 4039,
        GET_PITCH = 4033
    }
}
