using System;
using System.Threading;

namespace MultiThreadCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to multiThreading");

            using (var m1 = new Mutex(false,"OnlyInstanceOfTheApp"))
            {
                if(!m1.WaitOne(3000,false))
                {
                    Console.WriteLine("One instance is already running atm !! There can be only one simultaneously");
                    Console.ReadLine();
                }

                BaseClass myInstance = new BaseClass();

                //System.OverflowException: 'Arithmetic operation resulted in an overflow.'
                //Normaly with this setup you would get this error above. Because multiple threads are overwriting eachothers values.
                //To solve this issues we use lock.

                Thread thread1 = new Thread(new ThreadStart(myInstance.DivideLoop));
                Thread thread2 = new Thread(new ThreadStart(myInstance.DivideLoop));

                thread1.Start();
                thread2.Start();

                Console.Read();

            }

        }

        public class BaseClass
        {
            int num1 = 0;
            int num2 = 0;
            int num3 = 0;

            public void DivideLoop()
            {
                for (int i = 0; i < int.MaxValue; i++)
                {
                    lock(this)
                    {
                        num1 = 1;
                        num2 = 1;
                        num3 = 0;

                        num3 = num2 / num1;

                        num2 = 0;
                        num1 = 0;
                    }
                    
                }
            }


        }
    }
}
