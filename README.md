# Downsize [![Build Status](https://travis-ci.org/Magentaize/Downsize.Net.svg?branch=master)](https://travis-ci.org/Magentaize/Downsize.Net)

Tag-safe HTML and XML text-truncation!

```
PM> Install-Package Downsize.Net
```

## Usage
```csharp
var options = new DownsizeOptions(words: 26);
Downsize.Substring("<p>some markup here...</p>", options);
```