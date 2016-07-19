using System.Collections.Generic;
using System.Runtime.Serialization;

namespace LifesizeSharp
{
    [DataContract]
    public class LifesizeResponse
    {
        [DataMember(Name = "_rv")]
        public int ReturnValue { get; set; }
    }

    [DataContract]
    public class Request
    {
        [DataMember(Name = "call")]
        public string Call { get; set; }

        [DataMember(Name = "params")]
        public Dictionary<string, object> Parameters { get; set; }
    }

    [DataContract]
    public class RequestString
    {
        [DataMember(Name = "call")]
        public string Call { get; set; }

        [DataMember(Name = "params")]
        public object Parameters { get; set; }
    }

    [DataContract]
    public class LifesizeLoginResponse : LifesizeResponse
    {
        [DataMember(Name = "session")]
        public string Session { get; set; }
    }

    [DataContract]
    public class GetConnected : LifesizeResponse
    {
        [DataMember(Name = "connected")]
        public int Connected { get; set; }
    }

    [DataContract]
    public class GetSupported : LifesizeResponse
    {
        [DataMember(Name = "supported")]
        public int Supported { get; set; }
    }

    [DataContract]
    public class GetAntiFlicker : LifesizeResponse
    {
        [DataMember(Name = "value")]
        public int Value { get; set; }
    }

    [DataContract]
    public class GetPosition : LifesizeResponse
    {
        [DataMember(Name = "pan")]
        public float? Pan { get; set; }

        [DataMember(Name = "tilt")]
        public float? Tilt { get; set; }

        [DataMember(Name = "zoom")]
        public float? Zoom { get; set; }

        [DataMember(Name = "digital_zoom")]
        public int? DigitalZoom { get; set; }
    }

    [DataContract]
    public class GetPresetPosition : GetPosition
    {
        [DataMember(Name = "dev")]
        public uint? Device { get; set; }
    }

    [DataContract]
    public class GetEnabled : LifesizeResponse
    {
        [DataMember(Name = "enabled")]
        public int Enabled { get; set; }
    }

    [DataContract]
    public class GetPresentationState : LifesizeResponse
    {
        [DataMember(Name = "state")]
        public string State { get; set; }
    }

    [DataContract]
    public class EnableChanged : LifesizeResponse
    {
        [DataMember(Name = "enabled")]
        public int Enabled { get; set; }

        [DataMember(Name = "dev")]
        public Cameras Device { get; set; }
    }

    [DataContract]
    public class StateChanged : LifesizeResponse
    {
        [DataMember(Name = "state")]
        public string State { get; set; }
    }
}