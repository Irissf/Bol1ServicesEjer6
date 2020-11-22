using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bol1ServicesEjer6
{
    class Program
    {
        static Random rand = new Random();
        static readonly object key = new object();
        static char[] wiiii = new char[] { '|', '/', '-', '\\' };

        static int number;
        static int result = 0;
        static bool win = false;
        static bool start = false;
        static bool run = true;
        static void Main(string[] args)
        {
            Thread player1 = new Thread(Player1);
            Thread player2 = new Thread(Player2);
            Thread display = new Thread(Display);

            player1.Start();
            player2.Start();
            display.IsBackground = true;
            display.Start();

            player1.Join();
            player2.Join();


            Console.SetCursorPosition(0, 6);
            if (result < 0) Console.WriteLine("Win player 2");
            else Console.WriteLine("Win player 1");
            Console.ReadLine();
        }

        static public void Player1() //run a false wait
        {
            while (!win)
            {
                lock (key)
                {
                    if (!win)
                    {
                        number = rand.Next(1, 11);
                        if (number == 5 || number == 7)
                        {
                            start = true; //ya cuenta como que no lleva girando desde el inicio pues la paró Player 1
                            if (start && !run)
                            {
                                result = result + 4;
                            }
                            result = result + 1;
                            run = false;
                            if (result >= 20)
                            {
                                result = 20;
                                win = true;
                            }

                        }
                        drawResult(result);
                        Console.SetCursorPosition(0, 0);
                        Console.WriteLine("Player 1 " + number + " ");
                    }

                }
                Thread.Sleep(rand.Next(100, 100 * number));
            }
        }

        static public void Player2() // run a true, pulse
        {
            while (!win)
            {
                lock (key)
                {
                    if (!win)
                    {
                        number = rand.Next(1, 11);
                        if (number == 5 || number == 7)
                        {
                            start = true; //ya no está girando de inicio Player 2 la reactivo
                            if (start && run)
                            {
                                result = result - 4;
                            }
                            result = result - 1;
                            run = true;
                            Monitor.Pulse(key);
                            if (result <= -20)
                            {
                                result = -20;
                                win = true;
                            }
                        }
                        drawResult(result);
                        Console.SetCursorPosition(0, 4);
                        Console.WriteLine("player 2 " + number + " ");
                    }


                }
                    Thread.Sleep(rand.Next(100, 100 * number));
            }
        }

        static public void Display()
        {
            int i = 0;
            while (!win)
            {
                lock (key)
                {
                    if (run)
                    {
                        if (i < 3)
                        {
                            Console.SetCursorPosition(15, 2);
                            Console.WriteLine(wiiii[i]);
                            i++;
                        }
                        else
                        {
                            i = 0;
                        }
                    }
                    else
                    {
                        Monitor.Wait(key);
                    }
                }
                Thread.Sleep(200);
            }

        }

        static public void drawResult(int numberDraw)
        {
            Console.SetCursorPosition(30, 2);
            Console.WriteLine("Result: " + numberDraw + "  ");
        }
    }
}
