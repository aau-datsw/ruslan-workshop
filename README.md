# RUSLAN Workshop
Repository til alle underprojekter der bygges under RUSLAN 2019 Workshoppen. 

Systemet bygges som en SOA arkitektur, og hvert underprojekt agerer således som sin egen isolerede service der leverer en API der kan anvendes af andre services. 

![Overordnet arkitektur](img/system_architecture.png)

# Hvad skal det bruges til?
Overordnet skal systemet tilbyde en masse funktionaliteter der ville være brugbare i et stort LAN event som dette. 
Det inkluderer følgende: 
- [ ] Leaderboards - hvem er de bedste til hvert spil?
- [ ] Matchmaking - placér en spiller i en match hvor skill-level er optimalt passende
- [ ] Spilplanlægning - uden at tænke på skill-level, lav en round-robin spilplan
- [ ] Profiler - gør det muligt at oprette, ændre og slette egne profiler og se andres (søgefunktion)
- [ ] Eventstatistikker - hvor mange spiller lige nu i et givent spil? Hvor mange matches har der været? Vis en graf med antal samtidige spillere over tid
- [ ] Mere?

# Services
## Leaderboards
[Gruppe 1](LeaderboardService/)

## Matchmaking
[Gruppe 2](MatchmakingService/)

## Spilplanlægning
[Gruppe 3](TournamentService/)

## Profiler
[Gruppe 4](AccountService/)

## Eventstatistikker
[Gruppe 5](StatisticsService/)

# How to use

*Denne guide virker kun for Mac OS X og Linux brugere - ikke Windows, sorry :-(*
*Hvis du bruger Windows anbefaler vi varmt at du kører en virtuel maskine med Ubuntu 18.04 LTS på (google VirtualBox) eller installerer en variant af Ubuntu ved siden af Windows som dualboot.*

Gør følgende for at kunne besøge `https://ruslan.local` og `https://api.ruslan.local` i din browser for at teste din API: 
## Installér Docker og Docker Compose
Kør `sudo sh scripts/install.sh` i terminalen. Prøv derefter at køre `docker --version` og `docker-compose --version` i terminalen. Hvis du i nogen af tilfældene ikke får et versionsnummer ud, men i stedet en "command not found" eller lignende fejlbesked er noget gået galt i installeringsprocessen. Dette kan normalt fixes ved at manuelt køre hver linje i `scripts/install.sh` hver for sig i terminalen.
## Generér SSL certifikater til lokal udvikling
Kør `sudo sh cert-gen/gen-self-signed.sh ruslan.dk` og `sudo sh cert-gen/gen-self-signed.sh api.ruslan.dk`. Du burde nu have to directories `cert-gen/ruslan.dk/` og `cert-gen/api.ruslan.dk/`. For at emulere server-tilstanden skal disse ligge i `/etc/letsencrypt/live`. Dette har du højst sandsynligt ikke endnu, da du nok ikke har brugt Let's Encrypt på din computer, så lav det med `sudo mkdir -p /etc/letsencrypt/live`. 
Kopiér de to mapper ind med `sudo mv cert-gen/api.ruslan.dk /etc/letsencrypt/live` og `sudo mv cert-gen/ruslan.dk /etc/letsencrypt/live`. For at sikre, at SSL certifikaterne er blevet rykket korrekt, kør `ls /etc/letsencrypt/live` hvor outputtet burde være: 
```
api.ruslan.dk
ruslan.dk
```
## Lad loalhost pege på ruslan.local 
I din `/etc/hosts` fil skal du tilføje følgende to linjer i bunden: 
``` 
127.0.0.1     ruslan.local
127.0.0.1 api.ruslan.local
```
## Kør serveren
Kør `sudo sh restart-local.sh`. 
Besøg herefter `https://ruslan.local`. Du vil se en advarselsbesked om usikkert SSL certifikat. Bypass denne, vi ved godt det ikke er ret rigtigt certifikat vi har genereret. 
Besøg herefter `https://api.ruslan.local` og gør det samme. 

For at besøge Leaderboards API'en, besøg `https://api.ruslan.local/leaderboards/all`. Hvis du får en JSON liste af personer virker alt som det skal. 

![Sample svar fra serveren](img/ruslan_sample_response.png)

# FAQ 
## Jeg har ændret i `seed.sql` for min API, men der sker ikke noget med databasen
Databasens `seed.sql` bliver kun kørt når databasen laves. Da databasen allerede eksistere på disken køres det ikke. For at slette databasen kan du køre `sudo sh scripts/clean_docker.sh`. Bemærk at du mister al data i din database hvis du gør dette!
