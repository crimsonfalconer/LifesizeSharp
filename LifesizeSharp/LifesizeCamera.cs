using System;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace LifesizeSharp
{
    public enum Cameras : uint
    {
        Dvi0 = 0x00100000,
        Dvi1 = 0x00100001,
        Dvi2 = 0x00100002,
        Dvi3 = 0x00100003,
        Hdmi0 = 0x00020000,
        VDirect0 = 0x00800000
    };

    public enum ZoomDirection : int
    {
        Out = -1,
        In = 1
    };

    public enum AntiFlicker : int
    {
        [Description("50Hz")]
        FiftyHz = 0,
        [Description("60Hz")]
        SixtyHz = 1,
        Auto = 2
    };



    public class LifesizeCamera
    {
        private readonly CustomWebClient _webClient;
        private readonly LifesizeIconClient _parent;

        public LifesizeCamera(CustomWebClient webClient, LifesizeIconClient parent)
        {
            _webClient = webClient;
            _parent = parent;
        }

        private t LifesizePost<t>(string call, Dictionary<string, object> parameters)
        {
            var jsonRequest = new Request()
            {
                Call = call,
                Parameters = parameters
            };

            var webResponse = _webClient.UploadString("request/" + _parent.SessionId + "/" + call, JsonConvert.SerializeObject(jsonRequest));
            return JsonConvert.DeserializeObject<t>(webResponse);
        }

        // Camera_getConnected
        public bool GetConnected(Cameras device)
        {
            var response = LifesizePost<GetConnected>("Camera_getConnected", new Dictionary<string, object>
            {
                {"dev", device}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);

            // Connected
            if (response.Connected == 1)
                return true;

            // Disconnected
            return false;
        }

        // Camera_getSupported
        public bool GetSupported(Cameras device)
        {
            var response = LifesizePost<GetSupported>("Camera_getSupported", new Dictionary<string, object>
            {
                {"dev", device}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);

            // Connected
            if (response.Supported == 1)
                return true;

            // Disconnected
            return false;
        }

        // Camera_getAntiFlicker
        public AntiFlicker GetAntiFlicker(Cameras device)
        {
            var response = LifesizePost<GetAntiFlicker>("Camera_getAntiFlicker", new Dictionary<string, object>
            {
                {"dev", device}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);

            return (AntiFlicker) response.Value;
        }

        // Camera_getPosition
        public CameraPosition GetPosition(Cameras device)
        {
            var response = LifesizePost<GetPosition>("Camera_getPosition", new Dictionary<string, object>
            {
                {"dev", device}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);

            return new CameraPosition
            {
                Pan = response.Pan,
                Tilt = response.Tilt,
                Zoom = response.Zoom,
                DigitalZoom = response.DigitalZoom,
            };
        }

        // Camera_getPresetPosition
        public CameraPresetPosition GetPresetPosition(int preset)
        {
            var response = LifesizePost<GetPresetPosition>("Camera_getPresetPosition", new Dictionary<string, object>
            {
                {"preset", preset}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);

            return new CameraPresetPosition
            {
                Pan = response.Pan,
                Tilt = response.Tilt,
                Zoom = response.Zoom,
                DigitalZoom = response.DigitalZoom,
                Camera = (Cameras)response.Device
            };
        }

        // Camera_getVerticalFlipEnabled
        public bool GetVerticalFlipEnabled(Cameras device)
        {
            var response = LifesizePost<GetEnabled>("Camera_getVerticalFlipEnabled", new Dictionary<string, object>
            {
                {"dev", device}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);

            // Connected
            if (response.Enabled == 1)
                return true;

            // Disconnected
            return false;
        }

        // Camera_getHorizontalFlipEnabled
        public bool GetHorizontalFlipEnabled(Cameras device)
        {
            var response = LifesizePost<GetEnabled>("Camera_getHorizontalFlipEnabled", new Dictionary<string, object>
            {
                {"dev", device}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);

            // Connected
            if (response.Enabled == 1)
                return true;

            // Disconnected
            return false;
        }

        // Camera_getLockEnabled
        public bool GetLockEnabled(Cameras device)
        {
            var response = LifesizePost<GetEnabled>("Camera_getLockEnabled", new Dictionary<string, object>
            {
                {"dev", device}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);

            // Connected
            if (response.Enabled == 1)
                return true;

            // Disconnected
            return false;
        }

        //ASYNC: Camera_stop
        public void Stop(Cameras device)
        {
            var response = LifesizePost<LifesizeResponse>("Camera_stop", new Dictionary<string, object>
            {
                {"dev", device}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);
        }

        //ASYNC: Camera_pan
        // Speed: -1.0 Automatic based on zoom level
        //        (0.0:100.0) percentage of max-supported speed
        public void Pan(Cameras device, int direction, float speed)
        {
            var response = LifesizePost<LifesizeResponse>("Camera_pan", new Dictionary<string, object>
            {
                {"dev", device},
                {"direction", direction},
                {"speed", speed}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);
        }

        //ASYNC: Camera_tilt
        // Speed: -1.0 Automatic based on zoom level
        //        (0.0:100.0) percentage of max-supported speed
        public void Tilt(Cameras device, int direction, float speed)
        {
            var response = LifesizePost<LifesizeResponse>("Camera_tilt", new Dictionary<string, object>
            {
                {"dev", device},
                {"direction", direction},
                {"speed", speed}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);
        }

        //ASYNC: Camera_zoom
        // Direction: wide=-1 negative/out/wide direction
        //            tele=1 positive/in/tele direction
        // Speed: -1.0 Automatic based on zoom level
        //        (0.0:100.0) percentage of max-supported speed
        public void Zoom(Cameras device, ZoomDirection direction, float speed)
        {
            var response = LifesizePost<LifesizeResponse>("Camera_zoom", new Dictionary<string, object>
            {
                {"dev", device},
                {"direction", direction},
                {"speed", speed}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);
        }

        //ASYNC: Camera_tiltNudge
        // Count: Multiple of minimum nudge distance based on current zoom magnification (negative == negative/down direction, positive == positive/up direction)
        public void TiltNudge(Cameras device, int count)
        {
            var response = LifesizePost<LifesizeResponse>("Camera_tiltNudge", new Dictionary<string, object>
            {
                {"dev", device},
                {"count", count},
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);
        }

        //ASYNC: Camera_panNudge
        // Count: Multiple of minimum nudge distance based on current zoom magnification (negative == positive direction, positive == positive direction)

        public void PanNudge(Cameras device, int count)
        {
            var response = LifesizePost<LifesizeResponse>("Camera_panNudge", new Dictionary<string, object>
            {
                {"dev", device},
                {"count", count},
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);
        }

        //ASYNC: Camera_zoomNudge
        // Count: Multiple of minimum nudge distance (negative == negative/out/wide direction, positive == positive/in/tele direction)
        public void ZoomNudge(Cameras device, int count)
        {
            var response = LifesizePost<LifesizeResponse>("Camera_zoomNudge", new Dictionary<string, object>
            {
                {"dev", device},
                {"count", count},
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);
        }

        //ASYNC: Camera_recallPreset
        public void RecallPreset(int preset)
        {
            var response = LifesizePost<LifesizeResponse>("Camera_recallPreset", new Dictionary<string, object>
            {
                {"preset", preset}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);
        }

        //SYNC: Camera_setPosition
        public void SetPosition(Cameras device, float pan, float tilt, float zoom, int digitalZoom)
        {
            var response = LifesizePost<LifesizeResponse>("Camera_setPosition", new Dictionary<string, object>
            {
                {"dev", device},
                {"pan", pan},
                {"tilt", tilt},
                {"zoom", zoom},
                {"digital_zoom", digitalZoom}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);
        }

        //SYNC: Camera_setLockEnabled
        public void SetLockEnabled(Cameras device, bool enabled)
        {
            var response = LifesizePost<LifesizeResponse>("Camera_setLockEnabled", new Dictionary<string, object>
            {
                {"dev", device},
                {"enabled", (enabled ? 1 : 0)}
            });

            if (response.ReturnValue != 0)
                throw new Exception("Return Value: " + response.ReturnValue);
        }
    }

    public class CameraPosition
    {
        public float? Pan { get; set; }
        public float? Tilt { get; set; }
        public float? Zoom { get; set; }
        public int? DigitalZoom { get; set; }
    }

    public class CameraPresetPosition : CameraPosition
    {
        public Cameras Camera { get; set; }
    }
}