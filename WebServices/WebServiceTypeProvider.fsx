#r "System.Runtime.Serialization"
#r "System.ServiceModel"
#r "FSharp.Data.TypeProviders"

open System
open System.ServiceModel
open Microsoft.FSharp.Linq
open Microsoft.FSharp.Data.TypeProviders

type WeatherService = WsdlService<"http://wsf.cdyne.com/WeatherWS/Weather.asmx?WSDL">

let weatherServiceClient = WeatherService.GetWeatherSoap()

let forecast = weatherServiceClient.GetCityForecastByZIP("59101")

for forecast in forecast.ForecastResult do
    printfn "%A %s" forecast.Date forecast.Desciption
    printfn "High %A" forecast.Temperatures.DaytimeHigh
    