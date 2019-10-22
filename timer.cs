using System;
using System.Diagnostics;
using System.Threading;

class program 
{
    static void Main(string[] args)
    {
        parseArguments(args);
    }

    // Method for handleing arguments
    static void parseArguments(string[] args)
    {
        int argC = args.Length;

        if(argC > 0)
        {
            // Iterate over all arguments
            for(int i = 0; i < argC; i++)
            {         
                // Console.WriteLine("i" + i + ": " + args[i]);

                // Set deal with arguments and start timer 
                if(args[i] == "-timer" || args[i] == "-t")
                {
                    // Initate defaults
                    int skipIndex = 0;
                    float time = 0;
                    string message = "Heur.AdvML.B."; // Placeholder message, value is the code my Anti Virus gave the program when I tried to compile.

                    // Missing time argument, continue
                    if((i + 1) >= argC)
                    {
                        Console.WriteLine("Invalid argument, timer needs a time for countdown.");
                        continue;
                    }

                    try
                    {
                        // Run a loop to check for "h", "m", and "s" with a numberargument after. 
                        // "h" will multiply input to hours, "m" is just minutes, and "s" will divide to seconds
                        // Should no h/m/s be found, use default (minutes)
                        bool useTimeVariable = false;
                        
                        for(int j = (i + 1); j < argC; j++)
                        {
                            // Console.WriteLine("j" + j + ": " + args[j]);

                            if(args[j][0] == Char.Parse("-"))
                                break;
                            else if(args[j] == "h" && (j + 1) < argC)
                            {
                                time += float.Parse(args[j + 1]) * 60;
                                j++;
                                skipIndex += 2;
                                useTimeVariable = true;
                            }
                            else if(args[j] == "m" && (j + 1) < argC)
                            {
                                time += float.Parse(args[j + 1]);
                                j++;
                                skipIndex += 2;
                                useTimeVariable = true;
                            }
                            else if(args[j] == "s" && (j + 1) < argC)
                            {
                                time += float.Parse(args[j + 1]) / 60;
                                j++;
                                skipIndex += 2;
                                useTimeVariable = true;
                            }
                        }

                        // No h/m/s arguments, use the number given
                        if(!useTimeVariable)
                        {
                            time = float.Parse(args[i + 1]);
                            skipIndex++;
                        }
                    }
                    catch
                    {
                        Console.WriteLine("Invalid time for timer, must be a number.");
                        continue;
                    }

                    if(time < 0 || time > 9999)
                    {
                        Console.WriteLine("Invalid time, must be over 0 and under 9999");
                        continue;
                    }

                    // If there is a message after the time argument, override default message
                    int messageIndex = i + skipIndex + 1;
                    if(messageIndex < argC && args[messageIndex][0] != Char.Parse("-"))
                    {
                        message = "\"" + args[messageIndex] + "\"";
                        // Next argument iteration, skip over message argument
                        skipIndex++;
                    }
                    
                    // Set new message with time
                    if(message == "Heur.AdvML.B.")
                    {
                        message = "\"Your timer for " + time + " minutes expired.\"";
                    }

                    // Update user on progress and start a new cmd window with the timer
                    Console.WriteLine("Your timer will start in a new window.");
                    System.Diagnostics.Process.Start("cmd.exe", "/c mode con:cols=96 lines=8 && timer.exe -startTimer " + time + " " + message);

                    // Depending on what has been added for the arguments, update i so we can skip going though all the same arguments again
                    i += skipIndex;
                }
                // Start timer
                else if(args[i] == "-startTimer")
                {
                    setTimer((args[i + 1]), (args[i + 2]));
                    return;
                }
                // Deal with arguments for sw
                else if(args[i] == "-stopWatch" || args[i] == "-sw")
                {
                    Console.WriteLine("Your stopwatch will start in a new window.");
                    System.Diagnostics.Process.Start("cmd.exe", "/c mode con:cols=96 lines=8 && timer.exe -startStopWatch");
                    continue;
                }
                // Start timer
                else if(args[i] == "-startStopWatch")
                {
                    startStopWatch(0);
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
    
    // This menthod sets the timer, counts down, and alerts the user when finished
    // Note that time is a string, to allow arguments to pass though the command line as arguments
    static void setTimer(string time, string message)
    {
        DateTime dateTimeStarted = DateTime.Now;

        float limitMax = float.Parse(time) * 60;
        int limit = (int)Math.Ceiling(limitMax);

        // Timer cycle: write some info, sleep for 1 second, clear, reduce the limit and repeat until limit is reached
        while(limit > 0)
        {
            Console.WriteLine(dateTimeStarted + "\tTimer started, message: " + message + " \n");
            Console.WriteLine("\t" + limit + " seconds left.");

            Thread.Sleep(1 * 1000);
            Console.Clear();
            limit--;
        }

        // Timer is finished, get new dateTime and update user
        DateTime dateTimeFinished = DateTime.Now;
        Console.WriteLine(dateTimeStarted + "\tTimer started, message: " + message);
        Console.WriteLine(dateTimeFinished + "\tYour timer finished after " + time + " minutes (" + limitMax + " seconds);");
        Console.WriteLine("\t" + message + "\n");
        
        // Some beeps
        Console.Beep(523, 300);
        Console.Beep(323, 300);
        Console.Beep(523, 300);
        Console.Beep(623, 300);

        // Akin to the "pause" in cmd
        Console.WriteLine("Press R to restart the timer . . .");
        Console.WriteLine("Press any other key to close this window . . .");
        var input = Console.ReadKey();

        if(input.Key == ConsoleKey.R)
            setTimer(time, message);
    }

    // Method that starts a stopwatch
    static void startStopWatch(int time = 0, DateTime? dateTimeStarted = null)
    {
        if(dateTimeStarted == null)
            dateTimeStarted = DateTime.Now;

        // do-while loop from https://www.reddit.com/r/csharp/comments/agd1rn/how_to_break_a_loop_in_c_by_pressing_ctrl_z_or/
        // While user does not input Ctrl + Z, write info, sleep for 1 second, clear, increment time, and repeat
        var key = default(ConsoleKeyInfo);
        do 
        {
            while (!Console.KeyAvailable) 
            {
                Console.WriteLine(dateTimeStarted + "\tStopwatch started.\n");
                Console.WriteLine("\t" + time + " seconds have passed.");
                Thread.Sleep(1 * 1000);
                Console.Clear();
                time++;
            }
            key = Console.ReadKey();
        } 
        while(key.Key != ConsoleKey.Spacebar);

        Console.Clear();

        // Display updated info to user
        Console.WriteLine(dateTimeStarted + "\tStopwatch started.\n");
        Console.WriteLine("\t" + time + " seconds have passed.");

        // Allow user to continue stopwatch, using  current time and dateTimeStarted, though recursion
        Console.WriteLine("Continue stopwatch? (y/n) . . .");
        var doContinue = Console.ReadKey();

        if(doContinue.Key == ConsoleKey.Y)
            startStopWatch(time, dateTimeStarted);
    }

    // Simple help print
    static void printHelp()
    {
        Console.WriteLine("Note: ? indicates optional arguments.\n");
        Console.WriteLine("Arguments:");
        Console.WriteLine("- -timer, -t + ?[time-unit] + [time] + ?\"[message]\": sets a timer for [time] with [time-unit] value in minutes with the message [message]");
        Console.WriteLine("- -stopwatch, -sw: starts a stopwatch (Spacebar to pause)");
        Console.WriteLine("- -help, -h: prints this usage guide");
    }
}