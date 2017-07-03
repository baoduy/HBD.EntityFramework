﻿#region using

using System.Diagnostics;

#endregion

namespace HBD.Framework
{
    public static class ProcessStartExtension
    {
        //public static void Run(this ProcessStartInfo @this, TextWriter output)
        //{
        //    if (output == null)
        //    {
        //        @this.Run();
        //        return;
        //    }

        //    //@this.RedirectStandardOutput = false;

        //    Console.SetOut(output);

        //    using (var process = Process.Start(@this))
        //    {
        //        process.WaitForExit();
        //        process.Close();
        //    }
        //}

        public static string Run(this ProcessStartInfo @this)
        {
            @this.RedirectStandardOutput = true;

            using (var process = Process.Start(@this))
            {
                if (process == null) return string.Empty;

                var str = process.StandardOutput.ReadToEnd();
                process.WaitForExit();
                process.Close();

                return str;
            }
        }
    }
}