# WiFi-radar

### Tech stack:
- frontend - Vue.js
- backend - ASP.NET
- DB - MongoDB

### API endpoints

- GET api/map/search?city={value}
- GET api/map/search?city={value}&radius={value}
- POST api/user/favorites/{userId} (wifi object)
- GET api/user/favorites/{userId}
- DELETE api/user/favorites/{userId} (wifi id)
- POST api/wifi/review/{wifiId} (per body perduot user, comment, rating)
- GET api/wifi/review/{wifiId}
- DELETE api/wifi/review/{wifiId} (userId/token)
- POST /api/wifi/password/{wifiId} (body - password, userId)
- GET api/wifi/password/{wifiId}
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
