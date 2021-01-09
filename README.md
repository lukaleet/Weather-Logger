# Weather-Logger
Alpha version of Weather Logger. More info soon

UPDATE 30.12.2020:
Project seems to work, but I've decided to suspend it. I won't provide any documentation or support. My aim was to make project which combines microcontrollers (logging and sending temperature via Arduino C/C++ based language with ESP8266, which is WiFi module), backend server system using C# with validation and simple frontend client with html and very, very simple JS. I wanted to read temperatures at home from any location in the world with Internet access. I've given up on this because there are better systems for that like Domoticz with rich documentation and support.

---------------------------------------------------------------------

The idea was to log temperature (and other quantities if needed) via ESP8266 based board (in this example Wemos D1 mini, different ESP8266 based, NodeMCU-like board would probably work too, but I'm not sure; I haven't tested compatibility). With DS18B20 (aka Dallas Temperature) sensors attached to it's Input/Output pin/s via One Wire interface. 

NodeMCU ESP8266 inspired boards are cheap, so the idea was to make meshed logging system, for example having:

- one board with sensors attached to it in location X
- one board with sensors attached to it in location Y
etc.

And log gathered data in json format to the database using C# .Net Core web API and then fetch the data and display it on client of choice. In this case very simple website.

---------------------------------------------------------------------

Difficulties I faced:
Most of Arduino third party libraries have very poor documentation, especially "ArduinoJson". A lot of example videos (and even web docs) are/were outdated, because of that it took me numerous of tries before I suceeded to actually log data from few sensors. And then even more tries to actually serialize the data to json successfully.

Anyway, I'm satisfied with the work done. I managed to develop and finish the project the way I liked it. 
Projects of this type usually uses MySQL and PHP combined with Arduino/compatible boards, I wanted to make it happen with technologies of my choice. It wasn't straightforward, I'm not even sure if I did it correctly. But it worked and I'm happy with the result.

---------------------------------------------------------------------

Where's the diagram?
For people who actually got here to gain some info, sorry for lack of wiring diagram. I used pencil and paper to design it, because I couldn't find simple and free program (Fritzing is not free anymore and I don't have backup of old build). It was very simple though, only temperatures sensors connected to one pin using One Wire + resistor. I guess you can figure diagram out by reading .ino file.
