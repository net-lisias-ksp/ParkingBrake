﻿/*
	This file is part of Parking Brake 
		© 2018-21 Lisias T : http://lisias.net <support@lisias.net>

	THIS FILE is licensed to you under:

		* WTFPL - http://www.wtfpl.net
			* Everyone is permitted to copy and distribute verbatim or modified
 				copies of this license document, and changing it is allowed as long
				as the name is changed.

	THIS FILE is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
*/
using System;
//using KSPe.Util.Log;
using System.Diagnostics;

#if DEBUG
using System.Collections.Generic;
#endif

namespace ParkingBrake
{
    internal static class Log
    {
		//private static readonly Logger log = Logger.CreateForType<Startup>();
		private static class log
		{
			private static void print(string kind, string msg, params object[] @params)
				=> UnityEngine.Debug.LogFormat("[ParkingBrake] " + (null != kind ? (kind + ":") : "") + msg, @params);
			private static void printe(string msg, params object[] @params)
				=> UnityEngine.Debug.LogErrorFormat("[ParkingBrake] ERROR:" + msg, @params);

			internal static void force(string msg, object[] @params) => print(null, msg, @params);
			internal static void info(string msg, object[] @params) => print("INFO", msg, @params);
			internal static void warn(string msg, object[] @params) => print("WARNING", msg, @params);
			internal static void detail(string msg, object[] @params) => print("DETAIL", msg, @params);
			internal static void trace(string msg, object[] @params) => print("TRACE", msg, @params);
			internal static void trace(string msg) => print("TRACE", msg);
			internal static void error(Exception e, string msg) => printe(msg, e.ToString());
			internal static void error(string msg, object[] @params) => printe(msg, @params);
		}

		internal static void force (string msg, params object [] @params)
        {
            log.force (msg, @params);
        }

        internal static void info(string msg, params object[] @params)
        {
            log.info(msg, @params);
        }

        internal static void warn(string msg, params object[] @params)
        {
            log.warn(msg, @params);
        }

        internal static void detail(string msg, params object[] @params)
        {
            log.detail(msg, @params);
        }

        internal static void error(Exception e, string msg)
        {
            log.error(e, msg);
        }

        internal static void error(string msg, params object[] @params)
        {
            log.error(msg, @params);
        }

        [ConditionalAttribute("DEBUG")]
        internal static void dbg(string msg, params object[] @params)
        {
            log.trace(msg, @params);
        }

        #if DEBUG
        private static readonly HashSet<string> DBG_SET = new HashSet<string>();
        #endif

        [ConditionalAttribute("DEBUG")]
        internal static void dbgOnce(string msg, params object[] @params)
        {
            string new_msg = string.Format(msg, @params);
            #if DEBUG
            if (DBG_SET.Contains(new_msg)) return;
            DBG_SET.Add(new_msg);
            #endif
            log.trace(new_msg);
        }
    }
}
