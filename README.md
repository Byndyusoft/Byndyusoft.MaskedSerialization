# Byndyusoft.MaskedSerialization.Newtonsoft

Tool for serialization with masking sensitive data

| | | |
| ------- | ------------ | --------- |
| [**Byndyusoft.MaskedSerialization.Newtonsoft**](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization.Newtonsoft/) | [![Nuget](https://img.shields.io/nuget/v/Byndyusoft.MaskedSerialization.Newtonsoft.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization.Newtonsoft/) | [![Downloads](https://img.shields.io/nuget/dt/Byndyusoft.MaskedSerialization.Newtonsoft.svg)](https://www.nuget.org/packages/Byndyusoft.MaskedSerialization.Newtonsoft/) |


## Installing

```shell
dotnet add package Byndyusoft.MaskedSerialization.Newtonsoft
```

## Usage

Newtonsoft Json is used to serialize data with masking sensitive data. Use *[Masked]* attribute to define properties to be masked. Example:

```csharp
  public class CompanyDto
  {
      public string Name { get; set; } = default!;

      [Masked]
      public string SecretOwner { get; set; } = default!;

      public Income WhiteIncome { get; set; } = default!;

      [Masked]
      public Income GreyIncome { get; set; } = default!;
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

You can setup serializer settings to enable masking. Another usage example:

```csharp
  var settings = new JsonSerializerSettings();
  MaskedSerializationHelper.SetupSettingsForMaskedSerialization(settings);
  var serialized = JsonConvert.SerializeObject(dto, settings);
```

# Contributing

To contribute, you will need to setup your local environment, see [prerequisites](#prerequisites). For the contribution and workflow guide, see [package development lifecycle](#package-development-lifecycle).

A detailed overview on how to contribute can be found in the [contributing guide](CONTRIBUTING.md).

## Prerequisites

Make sure you have installed all of the following prerequisites on your development machine:

- Git - [Download & Install Git](https://git-scm.com/downloads). OSX and Linux machines typically have this already installed.
- .NET 5.0 or - [Download & Install .NET 5.0](https://dotnet.microsoft.com/download/dotnet/5.0).

## General folders layout

### src
- source code

### tests
- unit-tests


## Package development lifecycle

- Implement package logic in `src`
- Add or addapt unit-tests (prefer before and simultaneously with coding) in `tests`
- Add or change the documentation as needed
- Open pull request in the correct branch. Target the project's `master` branch

# Maintainers

[github.maintain@byndyusoft.com](mailto:github.maintain@byndyusoft.com)
