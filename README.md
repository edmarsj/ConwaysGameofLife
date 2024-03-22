# Conways Game of Life api by Edmar Souza Jr

## Summary

In this project I've implemented the Game of Life and structured the project trying to respect the SQRS principles alongside other good coding practices. 

Yes, it took a lot more time than I was expecting and I probably did more than what I was supposed to do. :smile:

Hope you have a good time reviewing the code, and sorry if it takes too long...

## Project Setup
This project used DynamoDB to store its persistent data. 

*I've provided an aws profile and credentials via email that you can use.*

But in case you want to setup the table yourself, you can use the aws cli to do so.

```powershell
cd aws
aws dynamodb create-table --cli-input-json file://create-table.json --region us-west-1
```
You'll also need to configure a profile if you're using the provided one:

```powershell
# You can change the profile name on the api appsettings.json
$ aws configure --profile gameoflife
AWS Access Key ID [None]:********KGGNW2
AWS Secret Access Key [None]:********HKHoB
Default region name [None]:us-west-1
Default output format [None]:
```

You can either run the project on visual studio or via dotnet cli:

```powershell
# build
dotnet build ConwaysGameofLife.sln
# run unit tests
dotnet test ConwaysGameofLife.sln
# run api project
dotnet run --project ./src/ConwaysGameofLife.API/ConwaysGameofLife.API.csproj
```

## Testing the project

- You can use swagger: `http://localhost:5166/swagger/index.html` or `https://localhost:44340/swagger/index.html` (if running inside visual studio)
- I'm also providing a Postman collection at `/postman/game-of-life.postman_collection.json` (this one uses `https://localhost:44340/*` paths) with some sample requests you can use. 
- There is a powershell file at `/powershell/showGenerations.ps1` that you can use to visualize the code working, you can start it using:

```powershell
PS D:\git\ConwaysGameofLife> cd powershell
PS D:\git\ConwaysGameofLife\powershell> .\showGenerations.ps1 <BOARD_ID>
```
You can get a list of all board ids in the system  via the endpoint:
- `GET /api/Board`

### Other information

- There are two ways of uploading the board initial configuration:
  - `POST /api/Board`: this endpoint will reguire the board name and the initial state as a int[,] matrix. Eg.
```json
{
    "name": "Beehive",
    "initialState": [
        [ 0,0,0,0,0,0 ],
        [ 0,0,1,1,0,0 ],
        [ 0,1,0,0,1,0 ],
        [ 0,0,1,1,0,0 ],
        [ 0,0,0,0,0,0 ]
    ]
}
```

  - `POST /api/Board/upload`: Requires the board name and a file. Boards need to be sequences of 0s and 1s, they need to have more than one row, and all rows should have the same number of columns. Eg.
```
00000
00100
00010
01110
00000
  ```
  There are sample of supported files int `/samplePatterns` folder. 
  - By default the endpoints will return the board state in ascii characters, some endpoints allow you to return a matrix instead