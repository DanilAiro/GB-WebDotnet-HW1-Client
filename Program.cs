using Client;
using System.Net;

namespace Seminar1Client;
public class Program
{
  
  static async Task Main(string[] args)
  {
    ChatClient chatClient = new();
    CancellationTokenSource cts = new();
    CancellationToken ct = cts.Token;

    try
    {
      chatClient.Run(ct);

      while (true)
        if (chatClient.ThreadException != null)
          throw chatClient.ThreadException;
    }
    catch (Exception ex) 
    {
      Console.WriteLine("Соединение разорвано, попробуйте переподключиться");
    }
    finally
    {
      cts.Cancel();
      chatClient.Dispose();
    }
  }
}