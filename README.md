# Byndyusoft.MaskedSerialization [![Nuget](https://img.shields.io/nuget/v/Byndyusoft.MaskedSerialization.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization/) [![Downloads](https://img.shields.io/nuget/dt/Byndyusoft.MaskedSerialization.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization/)

Tool for serialization by System.Text.Json package with masking sensitive data

## Installing

```shell
dotnet add package Byndyusoft.MaskedSerialization
```

## Usage

System.Text.Json is used to serialize data with masking sensitive data. Use *[Masked]* attribute to define properties to be masked. Example:

```csharp
  public class CompanyDto
  {
      public string Name { get; set; } = default!;

      [Masked]
      public string SecretOwner { get; set; } = default!;

      public Income WhiteIncome { get; set; } = default!;

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
Values of properties *Password* and *SecretInner* of class *TestDto* will be masked with value **"\*"**. Here is example of simple usage:

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

## Not implemented System.Text.Json annotations

If a type has at least one property with the [Masked] attribute, then annotation attributes will be ignored during serialization objects of this type. For example, those are *[JsonIgnore]*, *[JsonPropertyName(...)]*, *[JsonInclude]*, *[JsonConverter(...)]* etc.

# Byndyusoft.MaskedSerialization.Newtonsoft [![Nuget](https://img.shields.io/nuget/v/Byndyusoft.MaskedSerialization.Newtonsoft.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization.Newtonsoft/) [![Downloads](https://img.shields.io/nuget/dt/Byndyusoft.MaskedSerialization.Newtonsoft.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization.Newtonsoft/)

Tool for serialization by Newtonsoft.Json with masking sensitive data

## Installing

```shell
dotnet add package Byndyusoft.MaskedSerialization.Newtonsoft
```

## Usage

Newtonsoft Json is used to serialize data with masking sensitive data. Use *[Masked]* attribute to define properties to be masked. Usage example:

```csharp
  var serialized = MaskedSerializationHelper.SerializeWithMasking(dto);
  // Another usage example
  var settings = new JsonSerializerSettings();
  MaskedSerializationHelper.SetupSettingsForMaskedSerialization(settings);
  var serialized = JsonConvert.SerializeObject(dto, settings);
```

# Byndyusoft.MaskedSerialization.Serilog [![Nuget](https://img.shields.io/nuget/v/Byndyusoft.MaskedSerialization.Serilog.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization.Serilog/) [![Downloads](https://img.shields.io/nuget/dt/Byndyusoft.MaskedSerialization.Serilog.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization.Serilog/)

Tool for logging by Serilog with masking sensitive destructured data

## Installing

```shell
dotnet add package Byndyusoft.MaskedSerialization.Serilog
```

## Usage

You can setup logger configuration to use destructure masking policy:

```csharp
  var loggerConfiguration = new LoggerConfiguration().WithMaskingPolicy().WriteTo.Console();
```

Here is example of simple usage:

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

The output in console will be:

```
  [10:47:31 INF] Used company {"Name": "Mega Big Company", "SecretOwner": "*", "WhiteIncome": {"Description": "White", "SumInDollars": "*", "$type": "IncomeDto"}, "GreyIncome": "*", "$type": "CompanyDto"}
```

# Maintainers

[github.maintain@byndyusoft.com](mailto:github.maintain@byndyusoft.com)
