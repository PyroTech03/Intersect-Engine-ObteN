﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IntersectClientExtras.GenericClasses;
using Color = Intersect_Library.Color;

namespace Intersect_Client.Classes.General
{
    public static class ClientTime
    {
        private static DateTime _serverTime = DateTime.Now;
        private static ColorF _currentColor = ColorF.White;
        private static long _updateTime = 0;
        private static float _rate = 1f;
        private static long _colorUpdate = 0;
        private static Color _targetColor = Color.Transparent;

        public static void LoadTime(DateTime timeUpdate, Color clr, float rate)
        {
            _serverTime = timeUpdate;
            _targetColor = clr;
            _updateTime = 0;
            _rate = rate;
        }

        public static void Update()
        {
            if (_updateTime < Globals.System.GetTimeMS())
            {
                var ts = new TimeSpan(0, 0, 0, 0, (int) (1000*_rate));
                _serverTime = _serverTime.Add(ts);
                _updateTime = Globals.System.GetTimeMS() + 1000;
            }
            float ecTime = Globals.System.GetTimeMS() - _colorUpdate;
            float valChange = (255 * ecTime / 10000f);
            _currentColor.A = LerpVal(_currentColor.A, _targetColor.A, valChange);
            _currentColor.R = LerpVal(_currentColor.R, _targetColor.R, valChange);
            _currentColor.G = LerpVal(_currentColor.G, _targetColor.G, valChange);
            _currentColor.B = LerpVal(_currentColor.B, _targetColor.B, valChange);
            
            _colorUpdate = Globals.System.GetTimeMS();
        }

        private static float LerpVal(float val,float target, float amt)
        {
            if (val < target)
            {
                if (val + amt > target)
                {
                    val = target;
                }
                else
                {
                    val += amt;
                }
            }

            if (val > target)
            {
                if (val - amt < target)
                {
                    val = target;
                }
                else
                {
                    val -= amt;
                }
            }
            return val;
        }

        public static string GetTime()
        {
            return _serverTime.ToString("h:mm:ss tt");
        }

        public static ColorF GetTintColor()
        {
            return _currentColor;
        }


    }
}