# Byndyusoft.MaskedSerialization [![Nuget](https://img.shields.io/nuget/v/Byndyusoft.MaskedSerialization.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization/) [![Downloads](https://img.shields.io/nuget/dt/Byndyusoft.MaskedSerialization.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization/)

A tool to mask sensitive data during serialization with the System.Text.Json package

## Installing

```shell
dotnet add package Byndyusoft.MaskedSerialization
```

## Usage

If you use System.Text.Json to serialize data, use *[Masked]* attribute to mark properties that should be masked. Example:

```csharp
  public class CompanyDto
  {
      public string Name { get; set; } = default!;

      [Masked]
      public string SecretOwner { get; set; } = default!;

      public IncomeDto WhiteIncome { get; set; } = default!;

      [Masked]
      public IncomeDto GreyIncome { get; set; } = default!;
  }

  public class IncomeDto
  {
      public string Description { get; set; } = default!;

      [Masked]
      public long SumInDollars { get; set; }
  }
```
Values of properties *SecretOwner* and *GreyIncome* of class *CompanyDto* will be masked with value **"\*"**. Here is an example of simple usage:

```csharp
  var dto = new CompanyDto
                {
                    Name = "Mega Big Company",
                    SecretOwner = "Navalov",
                    WhiteIncome = new IncomeDto
                                      {
                                          Description = "White",
                                          SumInDollars = 1000000
                                      },
                    GreyIncome = new IncomeDto
                                     {
                                         Description = "Black",
                                         SumInDollars = 1000000000
                                     }
                };

  var serialized = MaskedSerializationHelper.SerializeWithMasking(dto);
```

The result will be:

```json
{
   "Name":"Mega Big Company",
   "SecretOwner":"*",
   "WhiteIncome":{
      "Description":"White",
      "SumInDollars":"*"
   },
   "GreyIncome":"*"
}
```

You can setup serializer options to enable masking. Another usage example:

```csharp
  var options = new JsonSerializerOptions();
  MaskedSerializationHelper.SetupSettingsForMaskedSerialization(options);
  var serialized = JsonSerializer.Serialize(dto, options);
```

# Byndyusoft.MaskedSerialization.Newtonsoft [![Nuget](https://img.shields.io/nuget/v/Byndyusoft.MaskedSerialization.Newtonsoft.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization.Newtonsoft/) [![Downloads](https://img.shields.io/nuget/dt/Byndyusoft.MaskedSerialization.Newtonsoft.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization.Newtonsoft/)

A tool to mask sensitive data during serialization with Newtonsoft.Json

## Installing

```shell
dotnet add package Byndyusoft.MaskedSerialization.Newtonsoft
```

## Usage

If you use Newtonsoft.Json to serialize data, use *[Masked]* attribute to mark properties that should be masked. Usage example:

```csharp
  var serialized = MaskedSerializationHelper.SerializeWithMasking(dto);
  // Another usage example
  var settings = new JsonSerializerSettings();
  MaskedSerializationHelper.SetupSettingsForMaskedSerialization(settings);
  var serialized = JsonConvert.SerializeObject(dto, settings);
```

# Byndyusoft.MaskedSerialization.Serilog [![Nuget](https://img.shields.io/nuget/v/Byndyusoft.MaskedSerialization.Serilog.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization.Serilog/) [![Downloads](https://img.shields.io/nuget/dt/Byndyusoft.MaskedSerialization.Serilog.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization.Serilog/)

A tool to mask sensitive destructured data in logging with Serilog

## Installing

```shell
dotnet add package Byndyusoft.MaskedSerialization.Serilog
```

## Usage

You can setup logger configuration to use destructure masking policy:

```csharp
  var loggerConfiguration = new LoggerConfiguration().WithMaskingPolicy().WriteTo.Console();
```

Here is an example of simple usage:

```csharp
public class Service
{
  private readonly ILogger<Service> _logger;

  public Service(ILogger<Service> logger)
  {
    _logger = logger;
  }

  public void LogCompany()
  {
    var dto = new CompanyDto
                  {
                      Name = "Mega Big Company",
                      SecretOwner = "Navalov",
                      WhiteIncome = new IncomeDto
                                        {
                                            Description = "White",
                                            SumInDollars = 1000000
                                        },
                      GreyIncome = new IncomeDto
                                       {
                                           Description = "Black",
                                           SumInDollars = 1000000000
                                       }
                  };
    
    _logger.LogInformation("Used company {@CompanyDto}", dto);
  }
}
```

The output in the console will be:

```
  [10:47:31 INF] Used company {"Name": "Mega Big Company", "SecretOwner": "*", "WhiteIncome": {"Description": "White", "SumInDollars": "*", "$type": "IncomeDto"}, "GreyIncome": "*", "$type": "CompanyDto"}
```

# Contributing

To contribute, you will need to setup your local environment, see [prerequisites](#prerequisites). For a contribution and workflow guide, see [package development lifecycle](#package-development-lifecycle).

A detailed overview on how to contribute can be found in the [contributing guide](CONTRIBUTING.md).

## Prerequisites

Make sure you have installed all of the following prerequisites on your development machine:

- Git - [Download & Install Git](https://git-scm.com/downloads). OSX and Linux machines typically have this already installed.
- .NET Core (version 6.0 or higher) - [Download & Install .NET Core](https://dotnet.microsoft.com/en-us/download/dotnet/6.0).

## Package development lifecycle

- Implement package logic in `src`
- Add or adapt unit-tests in `tests`
- Add or change the documentation as needed
- Open pull request for a correct branch. Target the project's `master` branch

# Maintainers

[github.maintain@byndyusoft.com](mailto:github.maintain@byndyusoft.com)
