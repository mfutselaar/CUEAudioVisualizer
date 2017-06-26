using System.Drawing;
using CUE.NET.Brushes;
using CUE.NET.Devices.Generic;
using CUEAudioVisualizer.Plugin;

namespace CUEAudioVisualizer.Plugins
{
    class VUPlugin : IPlugin
    {
        public string Name { get { return "VU"; } }

        public IPluginHost Host { get; set; }

        public VisualizerModes[] ModeList { get { return modeList; } }

        private VisualizerModes[] modeList;

        public VUPlugin()
        {
            modeList = new VisualizerModes[] { new VisualizerModes("VU (Left)", VULeftDelegate),
                new VisualizerModes("VU (Right)", VURightDelegate),
                new VisualizerModes("VU (Rainbow Left)", VURainbowLeftDelegate),
                new VisualizerModes("VU (Rainbow Right)", VURainbowRightDelegate)};
        }

        private void VULeftDelegate()
        {
            double brightness = Utility.Clamp(0.03f + (Host.SongBeat * 0.1f), 0f, 1f); //Keep brightness at least to 3% and clamp to 100% (Should never get anywhere near 100%)
            Color backgroundColor = Utility.CalculateColorBrightness(Host.SecondaryColor, brightness);
            Host.Keyboard.Brush = (SolidColorBrush)backgroundColor;

            float kbWidth = Host.Keyboard.DeviceRectangle.Location.X + Host.Keyboard.DeviceRectangle.Width;
            float immediateVolume = Host.ImmediateVolume;
            foreach (CorsairLed key in Host.Keyboard.Leds)
            {
                //Determine if key should be lit based on overall volume
                RectangleF keyRect = key.LedRectangle;
                PointF keyCenterPos = new PointF(keyRect.Location.X + (keyRect.Width / 2f), keyRect.Location.Y + (keyRect.Height / 2f)); //Sample center of key

                float keyHorVal = (keyCenterPos.X / kbWidth);

                if (immediateVolume >= keyHorVal)
                {
                    //Key should be fully lit
                    key.Color = Host.PrimaryColor;
                }
                else
                {
                    //Do whatever we do for unlit keys, which is currently nothing
                }
            }
        }

        private void VURightDelegate()
        {
            double brightness = Utility.Clamp(0.03f + (Host.SongBeat * 0.1f), 0f, 1f); //Keep brightness at least to 3% and clamp to 100% (Should never get anywhere near 100%)
            Color backgroundColor = Utility.CalculateColorBrightness(Host.SecondaryColor, brightness);
            Host.Keyboard.Brush = (SolidColorBrush)backgroundColor;

            float kbWidth = Host.Keyboard.DeviceRectangle.Location.X + Host.Keyboard.DeviceRectangle.Width;
            float immediateVolume = Host.ImmediateVolume;
            foreach (CorsairLed key in Host.Keyboard.Leds)
            {
                //Determine if key should be lit based on overall volume
                RectangleF keyRect = key.LedRectangle;
                PointF keyCenterPos = new PointF(keyRect.Location.X + (keyRect.Width / 2f), keyRect.Location.Y + (keyRect.Height / 2f)); //Sample center of key

                float keyHorVal = 1f - (keyCenterPos.X / kbWidth);

                if (immediateVolume >= keyHorVal)
                {
                    //Key should be fully lit
                    key.Color = Host.PrimaryColor;
                }
                else
                {
                    //Do whatever we do for unlit keys, which is currently nothing
                }
            }
        }

        private void VURainbowLeftDelegate()
        {
            float kbWidth = Host.Keyboard.DeviceRectangle.Location.X + Host.Keyboard.DeviceRectangle.Width;
            float kbHeight = Host.Keyboard.DeviceRectangle.Location.Y + Host.Keyboard.DeviceRectangle.Height;
            float immediateVolume = Host.ImmediateVolume;
            foreach (CorsairLed key in Host.Keyboard.Leds)
            {
                //Determine if key should be lit based on overall volume
                RectangleF keyRect = key.LedRectangle;
                PointF keyCenterPos = new PointF(keyRect.Location.X + (keyRect.Width / 2f), keyRect.Location.Y + (keyRect.Height / 2f)); //Sample center of key
                float keyVerticalPos = (keyCenterPos.Y / kbHeight);
                float keyHorizontalPos = (keyCenterPos.X / kbWidth);

                float keyHorVal = (keyCenterPos.X / kbWidth);

                if (immediateVolume >= keyHorVal)
                {
                    //Key should be fully lit
                    key.Color = Host.PrimaryColor;
                }
                else
                {
                    //'unlit' keys will be rainbow gradiented
                    float t = (float)(((keyHorizontalPos + (Host.Time * 0.4)) / 2f) + ((keyVerticalPos + (Host.Time * 0.4)) * 0.25f)) % 1f;
                    key.Color = Utility.CalculateRainbowGradient(t);
                }
            }
        }

        private void VURainbowRightDelegate()
        {
            float kbWidth = Host.Keyboard.DeviceRectangle.Location.X + Host.Keyboard.DeviceRectangle.Width;
            float kbHeight = Host.Keyboard.DeviceRectangle.Location.Y + Host.Keyboard.DeviceRectangle.Height;
            float immediateVolume = Host.ImmediateVolume;
            foreach (CorsairLed key in Host.Keyboard.Leds)
            {
                //Determine if key should be lit based on overall volume
                RectangleF keyRect = key.LedRectangle;
                PointF keyCenterPos = new PointF(keyRect.Location.X + (keyRect.Width / 2f), keyRect.Location.Y + (keyRect.Height / 2f)); //Sample center of key
                float keyVerticalPos = (keyCenterPos.Y / kbHeight);
                float keyHorizontalPos = (keyCenterPos.X / kbWidth);

                float keyHorVal = 1f - (keyCenterPos.X / kbWidth);

                if (immediateVolume >= keyHorVal)
                {
                    //Key should be fully lit
                    key.Color = Host.PrimaryColor;
                }
                else
                {
                    //'unlit' keys will be rainbow gradiented
                    float t = (float)(((keyHorizontalPos + (Host.Time * 0.4)) / 2f) + ((keyVerticalPos + (Host.Time * 0.4)) * 0.25f)) % 1f;
                    key.Color = Utility.CalculateRainbowGradient(t);
                }
            }
        }
    }
}
