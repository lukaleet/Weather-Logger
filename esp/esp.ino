#include <OneWire.h>
#include <DallasTemperature.h>

// We use ArduinoJson to store data as json object which is then mapped to the API object and stored in the database
#include <ArduinoJson.h>

// Setup a oneWire instance to communicate with any OneWire device
OneWire oneWire(D5);

// Pass oneWire reference to DallasTemperature library
DallasTemperature sensors(&oneWire);

int deviceCount = 0;

// Load Wi-Fi library
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>


// Replace with your network credentials
const char* ssid     = "Your Wifi Network goes here";
const char* password = "Your network password goes here";

// Set server name string
String serverName = "http://192.168.1.101:5000/api/temperature/upload?Token=sampletoken";

// START OF CLASS AND CLASS MEMBERS
// ....................................................................................................................................................

class Temperature {
  private:
    DeviceAddress address;
    void printAddress();
    
  public:
    float temperatureValue;
    String location;
    String addressAsString;
    
    Temperature(int index);
    void printInfo();  
    float measureTemperature(int index);
    String addressToString();
};

// Constructor of above class, takes index as a parameter in order to get address of device at that index from sensors object
// The address gets transformed to a string for easier data manipulation
Temperature::Temperature(int index){
  sensors.getAddress(this->address, index);
  this->addressAsString = this->addressToString();
}

// Method which prints info from class fields
void Temperature::printInfo(){
  Serial.println("_______________________________________________");
  Serial.println("Printing device info: ");
  Serial.println("Device location is: " + this->location);
  Serial.println("Device unique address is: " + this->addressAsString);
  Serial.println("Device measured temperature of: " + String(this->temperatureValue));
  Serial.println("_______________________________________________");
}

void Temperature::printAddress(){
    for (uint8_t i = 0; i < 8; i++)
  {
    if (this->address[i] < 16) Serial.print("0");
    Serial.print(this->address[i], HEX);
  }
  Serial.println();
}

// Method which measures temperatures at sensor
float Temperature::measureTemperature(int index){
//  sensors.requestTemperatures();
  this->temperatureValue = sensors.getTempCByIndex(index);
}

// Method which transform address from and DeviceAddress object to a String object
String Temperature::addressToString() {
  String addressToReturn;
  
  for (uint8_t i = 0; i < 8; i++)
  {
    if (this->address[i] < 16) Serial.print("0");
    addressToReturn += String(this->address[i], HEX); 
  } 
  
  return addressToReturn;
}

// END OF CLASS AND CLASS MEMBERS
// ....................................................................................................................................................


// Define amount of devices, does not work as plug and play :///
Temperature* temperatures[3];

void setup() {
  sensors.requestTemperatures();
  // put your setup code here, to run once:
  Serial.begin(115200);
  Serial.println();
  beginSensors();

  for (int i = 0; i < deviceCount; i++) {
    temperatures[i] = new Temperature(i);
    temperatures[i]->location = "pokoj";
    temperatures[i]->measureTemperature(i);
    temperatures[i]->printInfo();
  }
}

void loop() {
  // put your main code here, to run repeatedly:

    for (int i = 0; i < deviceCount; i++) {
    temperatures[i]->measureTemperature(i);
  }
  
  delay(5000);
  post();
  
}

void beginSensors() {
  sensors.begin();

  // locate devices on the bus
  Serial.println("Locating devices...");
  Serial.print("Found ");
  deviceCount = sensors.getDeviceCount();
  Serial.print(deviceCount, DEC);
  Serial.print(" devices.");
  Serial.println("");
}

void connectToWiFi() {
  WiFi.begin(ssid, password);

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.println("Waiting for connection...");
  }
  
  Serial.print("Connected to WiFi network with IP Address: ");
  Serial.println(WiFi.localIP());
}

// Make post call to the api
void post() {
  connectToWiFi();
  HTTPClient http;
  String postMessage;
  
  DynamicJsonDocument doc(2048);

  for (int i = 0; i < deviceCount; i++) {

    // Your Domain name with URL path or IP address with path
    http.begin(serverName);

     // Specify content-type header
    http.addHeader("Content-Type", "application/json");
    
    // Assign object values to the json keys
    doc["temperatureValue"] = temperatures[i]->temperatureValue;
    // to do make it as byte array or string
    doc["address"] = temperatures[i]->addressAsString;
    doc["sensorLocation"] = temperatures[i]->location;

    // Serialize it
    serializeJson(doc, postMessage);
    Serial.println("Post message: " + postMessage);

    // Make post request and retrieve response code 
    int httpResponseCode = http.POST(postMessage);

    Serial.print("HTTP Response code: ");
    Serial.println(httpResponseCode);

    // Clear post message, because it's a loop, and adding up to it is a bad thing
    // It's because only first Post request would be proper
    postMessage = "";
    // Clear Json object
    doc.clear();

    http.end();
  }
}
