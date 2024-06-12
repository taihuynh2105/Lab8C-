using System;
using System.Threading;

public class DeadlockExample
{
    static object lock1 = new object();
    static object lock2 = new object();

    static void Thread1FunctionLab8()
    {
        bool lock1Acquired = false;
        bool lock2Acquired = false;
        try
        {
            Monitor.TryEnter(lock1, TimeSpan.FromMilliseconds(500), ref lock1Acquired);
            if (lock1Acquired)
            {
                Console.WriteLine("Thread 1 locked lock1");
                Thread.Sleep(100); // Giả lập xử lý công việc
                Console.WriteLine("Thread 1 is waiting for lock2");

                Monitor.TryEnter(lock2, TimeSpan.FromMilliseconds(500), ref lock2Acquired);
                if (lock2Acquired)
                {
                    Console.WriteLine("Thread 1 locked lock2");
                }
                else
                {
                    Console.WriteLine("Thread 1 could not lock lock2");
                }
            }
            else
            {
                Console.WriteLine("Thread 1 could not lock lock1");
            }
        }
        finally
        {
            if (lock2Acquired)
            {
                Monitor.Exit(lock2);
            }
            if (lock1Acquired)
            {
                Monitor.Exit(lock1);
            }
        }
    }

    static void Thread2FunctionLab8()
    {
        bool lock1Acquired = false;
        bool lock2Acquired = false;
        try
        {
            Monitor.TryEnter(lock2, TimeSpan.FromMilliseconds(500), ref lock2Acquired);
            if (lock2Acquired)
            {
                Console.WriteLine("Thread 2 locked lock2");
                Thread.Sleep(100); // Giả lập xử lý công việc
                Console.WriteLine("Thread 2 is waiting for lock1");

                Monitor.TryEnter(lock1, TimeSpan.FromMilliseconds(500), ref lock1Acquired);
                if (lock1Acquired)
                {
                    Console.WriteLine("Thread 2 locked lock1");
                }
                else
                {
                    Console.WriteLine("Thread 2 could not lock lock1");
                }
            }
            else
            {
                Console.WriteLine("Thread 2 could not lock lock2");
            }
        }
        finally
        {
            if (lock1Acquired)
            {
                Monitor.Exit(lock1);
            }
            if (lock2Acquired)
            {
                Monitor.Exit(lock2);
            }
        }
    }

    public static void Main()
    {
        Thread thread1 = new Thread(Thread1FunctionLab8);
        Thread thread2 = new Thread(Thread2FunctionLab8);

        thread1.Start();
        thread2.Start();

        thread1.Join();
        thread2.Join();

        Console.WriteLine("End of Main");
    }
}