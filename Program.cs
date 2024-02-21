using System.Net.Sockets;

namespace Seminar1Client;

internal class Program
{
  static TcpClient tcpClient = new TcpClient();
  static bool isRunning = true;
  static long attempts = 10;

  static async Task Main(string[] args)
  {
    tcpClient.Connect("localhost", 55555);

    Console.WriteLine("Запущен");

    _ = Task.Run(() => GetMessageFromServer());

    _ = Task.Run(() => SendMessageToServer());

    await Task.Delay(-1);
  }

  public static void GetMessageFromServer()
  {
    try
    {
      while (isRunning)
      {
        ProcessServer();
      }
    }
    catch (Exception ex)
    {
      Console.WriteLine(ex.Message);
    }
  }

  public static void SendMessageToServer()
  {
    while (isRunning)
    {
      var writer = new StreamWriter(tcpClient.GetStream());
      string? message = Console.ReadLine();

      if (!string.IsNullOrEmpty(message))
      {
        writer.WriteLine(message);
        writer.Flush();
      }
    }
  }

  public static void ProcessServer()
  {
    var reader = new StreamReader(tcpClient.GetStream());

    if (attempts > 0)
    {
      try
      {
        string? message = reader.ReadLine();
        Console.WriteLine(message);
      }
      catch
      {
        tcpClient = new TcpClient();
        Thread.Sleep(3000);
        tcpClient.Connect("localhost", 55555);
        attempts--;
      }
    }
    else
    {
      isRunning = false;
    }
  }
}
