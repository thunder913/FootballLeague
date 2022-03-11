# FootballLeague

### How to run

1. Make sure the correct *Database Connection* is set n the appsettings.json file
2. Run the following command:
-Update-Database
3. Run the application and swagger should open

#### API Documentation:
You can make requests to the following API's:
- /City
- /Country
- /League
- /Team
- /Match
#### For all the PUT, GET and DELETE requests you must add the id in the url as following:
 -/Endpoint/**{id}**
#### You can also send a GET request with parameters, to get all the cities, matches, countries etc...
#### When sending a POST request, you must include the following parameters for each of the endpoints:
- City - cityName, countryId
- Country - countryName
- League - leagueName, countryId
- Match - homeTeamId, awayTeamId, homeGoals, awayGoals, date(in the format dd/MM/yyyy hh:mm)
- Team - teamName, cityId, leagueId
#### **In all the above request, you must provide a valid leagueId, cityId, teamId, where required, otherwise you will get 400 in response with the correct error text.**

#### In order to get the matches for a team you can send a request to:
- /match/teamMatches/{teamId}
#### In order to get the ranking you can send a request to:
- /league/getRanking/{leagueId}
