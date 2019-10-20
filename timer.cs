using System;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Media;

class program 
{
    static void Main(string[] args)
    {
        parseArguments(args);
    }

    static void setTimer(string time, string message)
    {
        // String msg = message;
        if(message == "Heur.AdvML.B.")
        {
            message = "Your timer for " + time + " minutes expired.";
        }

        DateTime dateTimeStarted = DateTime.Now;

        float limitMax = float.Parse(time) * 60;
        float limit = limitMax;
        while(limit > 0)
        {
            Console.WriteLine(dateTimeStarted + "\tTimer started, message: " + message);
            Console.WriteLine("\t" + limit + " seconds left.");

            Thread.Sleep(1 * 1000);
            Console.Clear();
            limit--;
        }

        DateTime dateTimeFinished = DateTime.Now;
        Console.WriteLine(dateTimeStarted + "\tTimer started, message: " + message);
        Console.WriteLine(dateTimeFinished + "\t Your timer finished after " + time + " minutes (" + limitMax + " seconds), message: " + message);
        
        Console.Beep(523, 300);
        Console.Beep(323, 300);
        Console.Beep(523, 300);
        Console.Beep(623, 300);

        Console.WriteLine("Press any key to close this window.");
        Console.ReadKey();
    }

    static int printHelp()
    {
        Console.WriteLine("Arguments:");
        Console.WriteLine("- -time, -t + [time] + \"[message]\": sets a timer for [time] value in minutes with the message [message].");
        Console.WriteLine("- -stopwatch, -sw: starts a stopwatch.");
        Console.WriteLine("- -help, -h: prints this usage guide.");

        return 0;
    }

    static void parseArguments(string[] args)
    {
        int argC = args.Length;

        if(argC > 0)
        {
            // Iterate over all arguments
            for(int i = 0; i < argC; i++)
            {                
                // Set deal with arguments and start timer 
                if(args[i] == "-timer" || args[i] == "-t")
                {
                    int skipIndex = 0;
                    float time = 0;
                    string message = "Heur.AdvML.B.";

                    try
                    {
                        time = float.Parse(args[i + 1]);
                        
                        // Skip over time argument
                        skipIndex++;
                    }
                    catch
                    {
                        Console.WriteLine("Invalid time for timer, must be a number.");
                        continue;
                    }

                    // If there is a message after the time argument, override default message.
                    if((i + 2) < argC && args[i + 2][0] != Char.Parse("-"))
                    {
                        message = "\"" + args[i + 2] + "\"";
                        // Skip over message argument
                        skipIndex++;
                    }

                    if(time < 0 || time > 9999)
                    {
                        Console.WriteLine("Invalid time, must be over 0 and under 9999");
                        continue;
                    }

                    Console.WriteLine("Your timer will start in a new window.");
                    System.Diagnostics.Process.Start("cmd.exe", "/c timer.exe -startTimer " + time + " " + message);

                    i += skipIndex;
                }
                // Start timer
                else if(args[i] == "-startTimer")
                {
                    setTimer((args[i + 1]), (args[i + 2]));
                    return;
                }
                // Help print
                else if(args[i] == "-help" || args[i] == "-h")
                {
                    printHelp();
                    continue;
                }
                // Unknown flag
                else if(args[i][0] == Char.Parse("-"))
                    Console.WriteLine("Unknown argument: \"" + args[i] + "\"");
            }
        }
        else
            Console.WriteLine("Too few arguments! See readme.md or run with \"-h\" for help.");
    }
}