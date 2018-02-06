// --------------------------------------------------------------------------------------------
// <copyright file="Program.cs" from='2009' to='2015' company='SIL International'>
//      Copyright ( c ) 2015, SIL International. All Rights Reserved.
//
//      Distributable under the terms of either the Common Public License or the
//      GNU Lesser General Public License, as specified in the LICENSING.txt file.
// </copyright>
// <author>Greg Trihus</author>
// <email>greg_trihus@sil.org</email>
// Last reviewed:
//
// <remarks>
//
// </remarks>
// --------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Mono.Options;

namespace BuildStep
{
    class Program
    {
        private static int _verbosity;
        private static readonly Regex Exp = new Regex(@"([0-9]+\.[0-9]+\.[0-9]+)\.[0-9]+");

        static void Main(string[] args)
        {
            string action = null;
            string rootFolder = null;
            string releaseFolder = "../output/Release";
            string basePath = null;
            string applicationFileName = null;
            bool showHelp = false;

            var param = new OptionSet
            {
                {"a|Action=", "the action 'UpdateAssemblies' or 'FileComponents'",
                    v =>
                    {
                        if (new List<string>{"filecomponents"}.Contains(v.ToLower()))
                            action = v.ToLower();
                        else
                            throw new ArgumentException("unknown action");
                        DebugMessage("Action: {0}", action);
                    }},
                {"r|RootFolder=", "all items and subfolders are scanned for AssemblyInfo.cs",
                    v =>
                    {
                        if (Directory.Exists(v))
                            rootFolder = v;
                        else
                            throw new ArgumentException("RootFolder missing");
                        DebugMessage("RootFoler: {0}", rootFolder);
                    }},
                {"o|ReleaseFolder=", "path to output release files (optionally relative to base path)",
                    v =>
                    {
                        if (!string.IsNullOrEmpty(basePath) && Directory.Exists(Path.Combine(basePath, v)))
                            releaseFolder = v;
                        else if (Directory.Exists(v))
                            releaseFolder = v;
                        else
                            throw new ArgumentException("ReleaseFolder missing");
                        DebugMessage("ReleaseFoler: {0}", releaseFolder);
                    }},
                {"b|BasePath=", "location of FileLibrary.xml",
                    v =>
                    {
                        if (File.Exists(Path.Combine(v, "FileLibrary.xml")))
                            basePath = v;
                        else
                            throw new ArgumentException("FileLibrary.xml not present at BasePath");
                        DebugMessage("BasePath: {0}", basePath);
                    }},
                {"f|ApplicationFileName=", "name of the application file", v => { applicationFileName = v; DebugMessage("ApplicationFileName: {0}", applicationFileName); }},
                { "h|Help",  "show this message and exit",
                   v => showHelp = v != null },
                { "v", "increase debug message verbosity",
                   v => { if (v != null) ++_verbosity; } },


            };
            try
            {
                var extra = param.Parse(args);
                if (extra.Count != 0 || showHelp)
                {
                    ShowHelp(param);
                    return;
                }
            }
            catch (OptionException e)
            {
                Console.Write("BuildStep: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `BuildStep --Help' for more information.");
                return;
            }

            switch (action)
            {
                case "filecomponents":
                    var componentsAct = new FileComponents{ApplicationFileName = applicationFileName, BasePath = basePath, ReleaseFolder = releaseFolder};
                    if (!componentsAct.Execute())
                        throw new ApplicationException("FileComponents action failed.");
                    break;
                default:
                    ShowHelp(param);
                    break;
            }
        }

        static void ShowHelp(OptionSet param)
        {
            Console.WriteLine("Usage: BuildStep [OPTIONS]+");
            Console.WriteLine("Perform action using parameters");
            Console.WriteLine();
            Console.WriteLine("Options:");
            param.WriteOptionDescriptions(Console.Out);
        }

        static void DebugMessage(string format, params object[] args)
        {
            if (_verbosity > 0)
            {
                Console.Write("# ");
                Console.WriteLine(format, args);
            }
        }

    }
}
