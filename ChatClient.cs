using System.Net.Sockets;

namespace Client;
public class ChatClient():IStartable
{
  public Exception? ThreadException { get; set; }
  public TcpClient? tcpClient = new();
  
  public async Task Run(CancellationToken ct)
  {
    try
    {
      tcpClient.Connect("localhost", 55555);

      Console.WriteLine("Клиент запущен");

      Task.Run(() => GetMessageFromServer(tcpClient), ct);

      Task.Run(() => SendMessageToServer(tcpClient), ct);

      Task.Delay(-1);
    }
    catch (Exception ex)
    {
      this.ThreadException = ex.InnerException ?? ex;
    }
  }

  public void GetMessageFromServer(TcpClient tcpClient)
  {
    try
    {
      while (true)
      {
        var reader = new StreamReader(tcpClient.GetStream());
        string message = reader.ReadLine() ?? throw new NullReferenceException();
        
        Console.WriteLine(message);
      } 
    }
    catch (Exception ex)
    {
      this.ThreadException = ex.InnerException ?? ex;
    }
  }

  public void SendMessageToServer(TcpClient tcpClient)
  {
    try
    {
      while (true)
      {
        var writer = new StreamWriter(tcpClient.GetStream());
        string? message = Console.ReadLine();

        if (!string.IsNullOrEmpty(message))
        {
          if (message.Equals("exit", StringComparison.InvariantCultureIgnoreCase) ||
              message.Equals("quit", StringComparison.InvariantCultureIgnoreCase))
          {
            Environment.Exit(0);
          }
          else
          {
            writer.WriteLine(message);
            writer.Flush();
          }
        }
      }
    }
    catch (Exception ex)
    {
      this.ThreadException = ex.InnerException ?? ex;
    }
  }

  public void Dispose()
  {
    tcpClient?.Dispose();
    tcpClient = null;
  }
}
