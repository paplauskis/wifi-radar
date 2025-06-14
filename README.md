# WiFi-radar

### Tech stack:
- frontend - Vue.js
- backend - ASP.NET (C#)
- DB - MongoDB

### API endpoints

- GET api/map/search?city={value}
- GET api/map/search?city={value}&radius={value}
- GET api/map/coordinates?city={value}&street={value}&buildingNumber={value}
- POST api/user/{userId}/favorites (wifi object)
- GET api/user/{userId}/favorites
- DELETE api/user/{userId}/favorites/{wifiId}
- POST api/wifi/reviews (per body perduot WifiReviewDto)
- GET api/wifi/reviews?city={value}&street={value}&buildingNumber={value}
- POST /api/wifi/passwords (body - PasswordDto)
- GET api/wifi/passwords?city={value}&street={value}&buildingNumber={value}
- POST api/user/auth/login (body - username, email)
- POST api/user/auth/register (body - username, email)
  
  

###  Funkcionalumai:
- pasirinktam mieste arba tam tikru perimetru gauti nemokamus ir viesai prieinamus wifi tinklus
- leisti naudotojams parasyti komentarus apie tam tikrus wifi tinklus
- naudotojai gali issisaugoti megstamus tinklus
- leisti naudotojams pasidalinti slaptazodziais prie apsaugotu tinklu


Story: Wifi Paieska
Kaip naudotojas, 
noriu ieškoti nemokamų ir viešai prieinamų Wi-Fi tinklų pasirinktoje vietoje ar tam tikru perimetru,
kad galėčiau greitai rasti interneto prieigą keliaudamas ar būdamas naujoje vietoje.

Story: Išsaugoti mėgstamus Wi-Fi tinklus
Kaip naudotojas,
noriu išsaugoti mėgstamus Wi-Fi tinklus į savo profilį,
kad galėčiau greitai juos rasti ateityje ar grįžęs į tą pačią vietą.

Story: Rašyti komentarus apie Wi-Fi tinklus
Kaip naudotojas,
noriu rašyti komentarus apie konkrečius Wi-Fi tinklus,
kad galėčiau pasidalinti savo patirtimi ir padėti kitiems naudotojams įvertinti jų kokybę ar patikimumą.

Story: Dalintis slaptažodžiais prie apsaugotų tinklų 
Kaip naudotojas,
noriu pasidalinti slaptažodžiu prie apsaugoto Wi-Fi tinklo,
kad galėčiau padėti kitiems naudotojams prisijungti prie to tinklo ir skatinti bendruomeniškumą.

Story: Zemelapio narsymas
Kaip naudotojas,
noriu matyti ir naršyti žemėlapį,
kad galėčiau realiai matyti visus wi-fi tinklus

Story: Wifi informacija
Kaip naudotojas, 
noriu matyti tinklų parsisiuntimo ir įkėlimo greičius,
kad galėčiau įvertinti mano lūkesčius atitinkančius wi-fi tinklus

## REST Principles

API atitinka Richardson Maturity Model:

1. **Level 1 - Resources**
   - Each endpoint represents a resource
   - URLs are resource-oriented

2. **Level 2 - HTTP Verbs**
   - GET: Retrieve resources
   - POST: Create resources
   - DELETE: Remove resources
   - Proper status codes (200, 201, 204, 400, 401, 404, 409, 500)

3. **Level 3 - HATEOAS**
   - Self-links in responses
   - Resource relationships

### Response Format

- Success responses include data and self-links
- Error responses include message and status code
- All responses are wrapped in a standard envelope

## Exception Handling

The API uses centralized exception handling through middleware. Common exceptions are mapped to appropriate HTTP status codes:

| Exception Type | HTTP Status | Description |
|----------------|-------------|-------------|
| ArgumentException | 400 | Invalid input parameters |
| UnauthorizedAccessException | 401 | Authentication failed |
| NotFoundException | 404 | Resource not found |
| ConflictException | 409 | Resource conflict |
| EmptyResponseException | 204 | No content available |
| Other Exceptions | 500 | Unexpected server error |