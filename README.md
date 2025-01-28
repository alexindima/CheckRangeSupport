# CheckRangeSupport

CheckRangeSupport is a C# console application designed to check server support for the HTTP `Range` header. The application allows sending requests with a specified byte range and analyzing whether the server supports partial requests.

## Features

- **Range Header Support Check:**
    - Sends HTTP requests with a specified byte range.
    - Analyzes the server response.
- **Customizable Range:**
    - Set the start and end of the byte range manually.
    - Default range values: `0-99`.
- **Content Saving:**
    - Option to save received data to a file.
- **Error Handling:**
    - Notifies users of network errors or timeouts.

## Installation and Usage

### System Requirements

- .NET SDK 8.0 or later
- Windows, Linux, or macOS

### Installation

1. Clone the repository:
   ```bash
   git clone https://github.com/alexindima/CheckRangeSupport.git
   cd CheckRangeSupport
   ```
2. Build the project:
   ```bash
   dotnet build
   ```

### Running the Application

1. Start the application:
   ```bash
   dotnet run --project CheckRangeSupport
   ```
2. Follow the console instructions:
    - Enter a URL to check for `Range` header support.
    - Press `Enter` to configure the byte range.
    - Type `exit` to quit the application.

## Example Usage

```plaintext
**********************************************************************
Enter URL or press ENTER to configure (type 'exit' to quit):
https://example.com/file
Server supports Range. Response status: 206 (Partial Content)
Asked for Range: 0-99, length: 100 bytes.

Server returned headers about Range:
Content-Range: bytes 0-99/1000
Content-Length: 100
Accept-Ranges: bytes

Do you want to save the content to a file? (y/N)
y
Content saved to file: response_20250128.bin
```

## Project Structure

- **CheckRangeSupport.csproj:** .NET project file.
- **Program.cs:** Main application code.

## Potential Enhancements

- Add support for multiple ranges in a single request.
- Add custom HTTP request settings (headers, timeouts).
- Log request results to a file.

## License

This project is licensed under the MIT License.

## Authors

- **Dmitrii Aleksin** ([@alexindima](https://github.com/alexindima))

