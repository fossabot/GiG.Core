# GiG.Core.Models

Library for generic Models.

## The `Response` Model

| Property Name             | Type           |
|:--------------------------|:---------------|
| IsSuccess                 | Boolean        |
| Data                      | T              |
| IsError                   | Boolean        |
| ErrorCode                 | String         | 

| Static Method Name        | Return Type    |
|:--------------------------|:---------------|
| Success(T data)           | Response<T>    |
| Fail(string errorCode)    | Response<T>    |

## The `ErrorResponse` Model           

| Property Name             | Type                              |
|:--------------------------|:----------------------------------|
| ErrorSummary              | String                            |
| Errors                    | IDictionary<string, List<string>> |