# COVIDProject

Built in .NET Core 3.1 and Visual Studio 3019 Community edition.

Provides APIs for querying data about the COVID-19 pandemic by county or state over given date ranges.
Has functions for aggregating data and providing data over time.

## Data is sourced from:
https://raw.githubusercontent.com/CSSEGISandData/COVID-19/master/csse_covid_19_data/csse_covid_19_time_series/time_series_covid19_confirmed_US.csv

## Application Structure:

### COVIDApp:   
This is the web API project and main application.
It contains the controllers and application setup.
Also has configuration for Open API documentation.

### COVIDData:  
This is the library that provides the data and contains logic for the queries.
Has several namespaces:
   - Models: Data models
   - Interfaces: Interfaces to fetch and query data
   - Exceptions: Custom exceptions for the library
   - Extensions: Utility methods for classes outside our control

The solution also includes a unit test suite written in MSTest and using Moq.
