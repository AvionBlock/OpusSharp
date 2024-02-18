namespace OpusSharp.Enums
{
    /// <summary>
    /// Encoder related CTL's.
    /// </summary>
    public enum EncoderCtl : int
    {
        SET_COMPLEXITY = 4010,
        GET_COMPLEXITY = 4011,
        SET_BITRATE = 4002,
        GET_BITRATE = 4003,
        SET_VBR = 4006,
        GET_VBR = 4007,
        SET_VBR_CONSTRAINT = 4020,
        GET_VBR_CONSTRAINT = 4021,
        SET_FORCE_CHANNELS = 4022,
        GET_FORCE_CHANNELS = 4023,
        SET_MAX_BANDWIDTH = 4004,
        GET_MAX_BANDWIDTH = 4005,
        SET_BANDWIDTH = 4008,
        SET_SIGNAL = 4024,
        GET_SIGNAL = 4025,
        SET_APPLICATION = 4000,
        GET_APPLICATION = 4001,
        GET_LOOKAHEAD = 4027,
        SET_INBAND_FEC = 4012,
        GET_INBAND_FEC = 4013,
        SET_PACKET_LOSS_PERC = 4014,
        GET_PACKET_LOSS_PERC = 4015,
        SET_DTX = 4016,
        GET_DTX = 4017,
        SET_LSB_DEPTH = 4036,
        GET_LSB_DEPTH = 4037,
        SET_EXPERT_FRAME_DURATION = 4040,
        GET_EXPERT_FRAME_DURATION = 4041,
        SET_PREDICTION_DISABLED = 4042,
        GET_PREDICTION_DISABLED = 4043
    }
}
