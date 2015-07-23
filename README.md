Alle Angaben ohne Gewähr. Die Einrichtung wurde bisher nur auf wenigen Machinen vorgenommen.

#### Einrichtung:
### Allgeein

Es muss das Git Repository heruntergeladen werden.
ascendancy.git runterladen, stable auschecken und, falls Serverbetrieb, die Weltdaten in server/data herunterladen.

git clone ssh://username@derfalke.no-ip.biz/home/git/ascendancy.git
cd ascendancy
checkout stable
cd server/data
git clone ssh://username@derfalke.no-ip.biz/home/git/ascendancy-world.git


### Windows (Android & Server)
Benötigt:
- Installiertes VS2012 (für Server und Client) oder höher (nur Client - Server benötigt VS2012) 
https://www.visualstudio.com/downloads/download-visual-studio-vs

- Xamarin Studio (Empfohlen) oder Xamarin für Visual Studio - minimal Probe- oder Indie-Zugang
http://xamarin.com/studio

- Beliebiger Android Emulator ODER Genymotion (Empfehlung) ODER echtes Gerät
https://www.genymotion.com/

Es muss das Git Repository heruntergeladen werden. Die Git-Befehle für Linux gelten auch hier:
ascendancy.git runterladen, stable auschecken und, falls Serverbetrieb, die Weltdaten in server/data herunterladen.

### Mac OS (iOS & Android & Server)
Benötigt:
- Xamarin Studio (Empfohlen) oder Xamarin für Visual Studio - minimal Probe- oder Indie-Zugang
http://xamarin.com/studio

(TODO: Chris bitte hinzufügen)

### Linux (Server)

Offizielles Mono Repo hinzufügen:

sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
echo "deb http://download.mono-project.com/repo/debian wheezy main" | sudo tee /etc/apt/sources.list.d/mono-xamarin.list
sudo apt-get update

Mono und Abhängigkeiten isntallieren (Mono, IDE, Webserver und PCL für die Core-Bibliothek):

sudo apt-get install mono monodevelop mono-xsp4  referenceassemblies-pcl 


#### Eigenen Server aufsetzen
Der Server kann nun mit einer beliebigen Xamarin, MonoDevelop Version oder VisualStudio 2012 kompiliert und ausgeführt werden.

Damit der Server von einem Client genutzt wird, muss der Server von aussen erreichbar sein. Es muss gegebenfalls eine Port Weiterleitung eingerichtet werden.
Im Client unter ClientConstants muss die Server Adresse angepasst werden. (in ClientConstants.cs die Konstanten DATA_SERVER = Apache/Datenserver; LOGIC_SERVER = Spieleserver ändern) 


#### Probleme:
- Falls keine Verbindung zum Server existiert, läd er nur den Startbildschirm -> Netzwerk einrichten, im Smartphone-Browser testen und App neustarten
Es kann kontrolliert werden, ob der Server läuft, in dem folgende Adressen im Webbrowser gestartet werden:
http://derfalke.no-ip.biz/
http://derfalke.no-ip.biz:9000
- Falls kein GPS existiert, sieht man nur grüne Wiese -> GPS ändern. Gültige Koordinaten: Erfurt und Umgebung



#### Aufbau
Das Projekt Ascendancy besteht aus 3 kompilierbaren Projekten (server, client, test) und einer Bibliothek (core).
Das test-Projekt diente zum ersten testen der Funktionalität, kann im aktuellen Stand aber ignoriert werden, da der Client mittlerweile diese Funktionalität nutzt.

#### Core
Der Core beinhaltet sämtliche Kernfunktionalität, welche sich Server und Client teilen (sollten):
- Spielelemente (Entititäten, Regionen, Definitionen)
- Verwalten der Spielelementen (Manager)
- Positionenstransformationen der verschiedenen Koordinatensysteme (LatLon, Spielfeld-Position, Region+Cell)
- Die komplette Spielelogik (In Form von Actions, plus alle zusätzlichen Algorithmen wie A* zur Wegfindung)
- Klassen für den Datentransfer 

#### Client 
- GPS
- Anzeige des Logos
- Laden von Definitionen vom Server
    
- Login beim Server
- Anzeige des Spielfelds anhand der vom Server geladenen Spielinformationen
- Laden der Entities und Aktionen vom Server (sowie Anzeige)
- Rudimentäre Implementation von Aktions-Animationen (Bewegungen)

#### Server
- Verwaltung und Authentifizierung von Spielern (registrierung, sessions, einloggen)
- Verwaltung welche Aktionen ausgeführt werden können
- Verwaltung welche Spieler welche Informationen erhalten (Ortsabhängig, und abhängig welche der Spieler diese bereits erhielt))
- Vermeidung von Threading-Problemen bei mehreren Spielern (siehe ascendancy/docs/SKBS_Bernd_Schmidt.pdf)
- Speicherung von persistenten Zuständen (Datenbank, implementiert aber ungenutzt)
- Scripts zum Konvertieren von OpenStreetMaps-Daten in nutzbare JSON Dateien


#### Rechtliches, Copyright und Lizenzrechtliches
siehe ascendancy/licences
- Kartendaten von OpenStreetMaps (@OpenStreetMaps Contributors)
- Bilder von Battle of Wesnoth
(TODO: Norman bitte hinzufügen)
