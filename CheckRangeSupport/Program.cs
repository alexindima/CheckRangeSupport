using System.Net;
using System.Net.Http.Headers;

namespace CheckRangeSupport;

internal static class Program {
    private static int _startRange;
    private static int _endRange;
    private static readonly HttpClient Client = new();
        
    private static async Task Main() {
        SetRangeValues();
        while (true) {
            Console.WriteLine("**********************************************************************");
            Console.WriteLine("Enter URL or press ENTER to configure (type 'exit' to quit):");
            var input = Console.ReadLine();

            if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase)) {
                Console.WriteLine("Exiting...");
                break;
            }

            if (string.IsNullOrEmpty(input)) {
                Console.WriteLine("Enter start and end values for Range, separated by space:");
                var range = Console.ReadLine();
                    
                var parts = range?.Split(' ');
                switch (parts?.Length) {
                    case 2 when int.TryParse(parts[0], out var value1) && int.TryParse(parts[1], out var value2):
                        SetRangeValues(value1, value2);
                        break;
                    case 1 when int.TryParse(parts[0], out var value):
                        SetRangeValues(value);
                        break;
                    default:
                        Console.Write("Invalid input. Using default values. ");
                        SetRangeValues();
                        break;
                }
            }
            else {
                await TestRangeSupport(input, _startRange, _endRange);
            }
        }
    }

    private static async Task TestRangeSupport(string url, int startRange, int endRange) {
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Range = new RangeHeaderValue(startRange, endRange);

        try {
            HttpResponseMessage response = await Client.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.PartialContent) {
                Console.WriteLine($"Server supports Range. Response status: {response.StatusCode} ({(int)response.StatusCode})");
                Console.WriteLine($"Asked for Range: {startRange}-{endRange}, length: {endRange - startRange + 1} bytes.");
                Console.WriteLine();
                Console.WriteLine("Server returned headers about Range:");
                Console.WriteLine($"Content-Range: {response.Content.Headers.ContentRange}");
                Console.WriteLine($"Content-Length: {response.Content.Headers.ContentLength}");
                Console.WriteLine($"Accept-Ranges: {response.Headers.AcceptRanges}");
                    
                Console.WriteLine("Do you want to save the content to a file? (y/N)");
                var userInput = Console.ReadLine();
                if (!string.IsNullOrEmpty(userInput) && userInput.StartsWith("y", StringComparison.OrdinalIgnoreCase)) {
                    var content = await response.Content.ReadAsByteArrayAsync();
                    var filePath = $"response_{DateTime.Now:yyyyMMddHHmmss}.bin";
                    await File.WriteAllBytesAsync(filePath, content);
                    Console.WriteLine($"Content saved to file: {filePath}");
                }
            }
            else {
                Console.WriteLine($"Server does not support Range as expected. Response status: {response.StatusCode} ({(int)response.StatusCode})");
                Console.WriteLine($"Content-Length: {response.Content.Headers.ContentLength} bytes");
            }
        }
        catch (TaskCanceledException) {
            Console.WriteLine("The request was canceled due to timeout.");
        } catch (Exception e) {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
        
    private static void SetRangeValues(int? value1 = null, int? value2 = null) {
        const int defaultStartRange = 0;
        const int defaultEndRange = 99;
        if (value1 is null && value2 is null) {
            _startRange = defaultStartRange;
            _endRange = defaultEndRange;
        }

        if (value1 is not null && value2 is null) {
            _startRange = defaultStartRange;
            _endRange = (int)value1;
        }

        if (value1 is not null && value2 is not null) {
            _startRange = Math.Min((int)value1, (int)value2);
            _endRange = Math.Max((int)value1, (int)value2);
        }
            
        Console.WriteLine($"Range set to: {_startRange}-{_endRange}");
    }
}