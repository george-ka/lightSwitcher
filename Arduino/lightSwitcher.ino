 /**
 *  Receives a command via Serial and turns on and off light switches
 *  a command is a byte
 *  commands are listed below as constants
 *  To turn on a switch, a command must be between TURN_ON_OFFSET and NUMBER_OF_SWITCHES + TURN_ON_OFFSET
 *  To turn off a switch, a command must be between TURN_OFF_OFFSET and NUMBER_OF_SWITCHES + TURN_OFF_OFFSET
 *  SHOW_STATE_COMMAND prints current state of switches
 * 	TURN_ALL_OFF_COMMAND and TURN_ALL_ON_COMMAND are self explanatory
 */

#define NUMBER_OF_SWITCHES 16
#define TURN_ON_OFFSET 48
#define TURN_OFF_OFFSET 97
#define SHOW_STATE_COMMAND = 5
#define TURN_ALL_OFF_COMMAND = 6
#define TURN_ALL_ON_COMMAND = 7
#define OFF = 0
#define ON = 1

int inByte = 0; // incoming serial byte
int pin = 0;
// list of connected pins to control light
int pins[NUMBER_OF_SWITCHES] = { 23, 25, 27, 29, 31, 33, 35, 37, 39, 41, 43, 45, 47, 49, 51, 53 };
int states[NUMBER_OF_SWITCHES];

void setup() {
  // init list of switches
  for (int i = 0; i < NUMBER_OF_SWITCHES; i++)
  {
    pinMode(pins[i], OUTPUT);
    // Let's write high for now
    // It means turn all lights off, because of the logic of the control
    digitalWrite(pins[i], HIGH);
	states[i] = OFF;
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
  
  Serial.print("Received command:");
  Serial.println(inByte);
  
  // Turn on switch
  if (inByte >= TURN_ON_OFFSET 
    && inByte < TURN_ON_OFFSET + NUMBER_OF_SWITCHES)
  {
    changeSwitchState(inByte - TURN_ON_OFFSET, true);
  }
  // Turn off 
  else if (inByte >= TURN_OFF_OFFSET 
	&& inByte < TURN_OFF_OFFSET + NUMBER_OF_SWITCHES)
  {
    changeSwitchState(inByte - TURN_OFF_OFFSET, false);
  }
  else if (inByte == SHOW_STATE_COMMAND)
  {
	Serial.print("State:");
	for (int i = 0; i < NUMBER_OF_SWITCHES; i++)
	{
		Serial.print(i);
		Serial.print("=");
		Serial.print(states[i]);
		Serial.print(" ");
	}
	
	Serial.println("");
  }
  else if (inByte == TURN_ALL_ON_COMMAND)
  {
	for (int i = 0; i < NUMBER_OF_SWITCHES; i++)
	{
		changeSwitchState(i, true);
	}
	
	Serial.println("Turned all on");
  }
  else if (inByte == TURN_ALL_OFF_COMMAND)
  {
	for (int i = 0; i < NUMBER_OF_SWITCHES; i++)
	{
		changeSwitchState(i, false);
	}
	
	Serial.println("Turned all off");
  }
}

void changeSwitchState(int index, bool isOn) {
	pin = pins[index];
    digitalWrite(pin, isOn ? LOW : HIGH);
	states[index] = isOn ? ON : OFF;
    Serial.print("Turn ");
	Serial.print(isOn ? "ON" : "OFF");
	Serial.print(" pin:");
    Serial.print(pin);
	Serial.print(" switch:");
    Serial.println(index);
}

void establishContact() {
  while (Serial.available() == 0) {
    Serial.print('A');   // send a capital A
    delay(300);
  }
  
  Serial.print(" Connected\n");
}
