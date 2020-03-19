 /**
 *  Receives a command via Serial and turns on and off light switches
 *  I should probably save it to EPROM to remember which lights were on
 */

#define NUMBER_OF_SWITCHES 16
#define TURN_ON_OFFSET 48
#define TURN_OFF_OFFSET 97

int inByte = 0;         // incoming serial byte
int pin = 0;
// list of connected pins to control light
int pins[NUMBER_OF_SWITCHES] = { 23, 25, 27, 29, 31, 33, 35, 37, 39, 41, 43, 45, 47, 49, 51, 53 };

void setup() {
  // init list of switches
  for (int i = 0; i < NUMBER_OF_SWITCHES; i++)
  {
    pinMode(pins[i], OUTPUT);
    // Let's write high for now
    // It means turn all lights off, because of the logic of the control
    digitalWrite(pins[i], HIGH);
  }
  
  // start serial port at 9600 bps:
  Serial.begin(9600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for native USB port only
  }
  
  establishContact();  // send a byte to establish contact until receiver responds
}

void loop() {
  if (Serial.available() == 0) 
  {
    delay(100);
    return;
  }

  inByte = Serial.read();
  Serial.println(inByte);
  // Turn on switch
  if (inByte >= TURN_ON_OFFSET 
    && inByte < TURN_ON_OFFSET + NUMBER_OF_SWITCHES)
  {
    pin = pins[inByte - TURN_ON_OFFSET];
    digitalWrite(pin, LOW);
    Serial.print("Turn on pin:");
    Serial.println(pin);
  }
  // Turn off 
  else if (inByte >= TURN_OFF_OFFSET && inByte < TURN_OFF_OFFSET + NUMBER_OF_SWITCHES)
  {
    pin = pins[inByte - TURN_OFF_OFFSET];
    digitalWrite(pin, HIGH);
    Serial.print("Turn off pin:");
    Serial.println(pin);
  }
}

void establishContact() {
  while (Serial.available() == 0) {
    Serial.print('A');   // send a capital A
    delay(300);
  }
  
  Serial.print(" Connected\n");
}
