/*
  Fire Truck Lights

  Used to control LEDs, which will be attached to a miniature fire truck.
  Runs through an initialization cycle, and then begins blinking the lights
  in a pre-determined pattern.

  The circuit:
  * Designed for an Adafruit Trinket (3v), but can be used on any device
    with the requisite pins.
  * Uses a 3.7v LiPo battery.
  * Each pin controls a set of colored LEDs, as defined below.

  Created Jan 06, 2017
  By Will Knoll
*/

// Define Pins
// Unfortunately, using pin #1 causes the Trinket onboard LED to blink as well
const byte LED_FRONT_WHITE = 0;   // Set pin 0 as the front White LED output
const byte LED_FRONT_BLUE  = 1;   // Set pin 1 as the front Blue LED output
const byte LED_FRONT_RED   = 2;   // Set pin 2 as the front Red LED output
const byte LED_REAR_LEFT   = 3;   // Set pin 3 as the Left Rear LED output
const byte LED_REAR_RIGHT  = 4;   // Set pin 4 as the Right Rear LED output

// Set min and max brightness
// (Not really used}
const int MIN_BRIGHTNESS = LOW;
const int MAX_BRIGHTNESS = HIGH;

// Define blink pin timer(s) to avoid using delay()
long _lastFrontBlinkTime = 0; // Store last time front LEDs were updated
long _lastRearBlinkTime = 0; // Store last time rear LEDs were updated
long _lastBlueBlinkTime = 0; // Store the blue LEDs separately

// Set initial LED pin states
int _frontRedState = MAX_BRIGHTNESS;
int _frontBlueState = MAX_BRIGHTNESS;
int _frontWhiteState = MIN_BRIGHTNESS;
int _rearLeftState = MAX_BRIGHTNESS;
int _rearRightState = MIN_BRIGHTNESS;

// Blink intervals (milliseconds)
const int SLOW_INTERVAL = 1000;
const int MED_INTERVAL = 500;
const int FAST_INTERVAL = 250;
const int INSANE_INTERVAL = 125;

void setup() {
  // Set pin modes
  pinMode(LED_FRONT_RED, OUTPUT);
  pinMode(LED_FRONT_BLUE, OUTPUT);
  pinMode(LED_FRONT_WHITE, OUTPUT);
  pinMode(LED_REAR_LEFT, OUTPUT);
  pinMode(LED_REAR_RIGHT, OUTPUT);

  // Ensure LEDs are off to start
  TurnAllOff();

  // Run through all the lights
  InitSequence();

  // Delay a smidge
  delay(500);
}

void loop() {
  // Blink the Red and White LEDs in front
  AlternateBlink(FAST_INTERVAL, LED_FRONT_RED, _frontRedState, LED_FRONT_WHITE, _frontWhiteState, _lastFrontBlinkTime);
  // Blink the Blue LEDs in front
  Blink(INSANE_INTERVAL, LED_FRONT_BLUE, _frontBlueState, _lastBlueBlinkTime);
  // Blink the Red LEDs in back
  AlternateBlink(SLOW_INTERVAL, LED_REAR_LEFT, _rearLeftState, LED_REAR_RIGHT, _rearRightState, _lastRearBlinkTime);
}

// Turn all of the LEDs off
void TurnAllOff() {
  digitalWrite(LED_FRONT_RED, MIN_BRIGHTNESS);
  digitalWrite(LED_FRONT_BLUE, MIN_BRIGHTNESS);
  digitalWrite(LED_FRONT_WHITE, MIN_BRIGHTNESS);
  digitalWrite(LED_REAR_LEFT, MIN_BRIGHTNESS);
  digitalWrite(LED_REAR_RIGHT, MIN_BRIGHTNESS);
}

// Turn all of the LEDs on
void TurnAllOn() {
  digitalWrite(LED_FRONT_RED, MAX_BRIGHTNESS);
  digitalWrite(LED_FRONT_BLUE, MAX_BRIGHTNESS);
  digitalWrite(LED_FRONT_WHITE, MAX_BRIGHTNESS);
  digitalWrite(LED_REAR_LEFT, MAX_BRIGHTNESS);
  digitalWrite(LED_REAR_RIGHT, MAX_BRIGHTNESS);  
}

// Just for show, run a quick initialization sequence
void InitSequence() {
  // Cycle through all the lights
  int ledPins[] = { LED_FRONT_RED, LED_FRONT_WHITE, LED_FRONT_BLUE, LED_REAR_LEFT, LED_REAR_RIGHT };

  for (int currentPin = 0; currentPin < 5; currentPin++) {
    digitalWrite(ledPins[currentPin], MAX_BRIGHTNESS);
    delay(MED_INTERVAL);
    digitalWrite(ledPins[currentPin], MIN_BRIGHTNESS);
    delay(MED_INTERVAL);
  }

  // Blink everything a few times
  for (int i = 0; i < 5; i++) {
    TurnAllOn();
    delay(FAST_INTERVAL);
    TurnAllOff();
    delay(FAST_INTERVAL);
  }
}

// Given one LED pin, blink it at a set interval
// <param name="name">description</param> 
void Blink(int interval, byte led, int& ledPinState, long& lastBlinkTime)
{
  unsigned long now = millis();

  if (now - lastBlinkTime >= interval)
  {
    // Save the last time we blinked the LED
    lastBlinkTime = now;

    // Swap to opposite state
    ledPinState = (ledPinState == LOW) ? HIGH : LOW;

    // Set the LED
    digitalWrite(led, ledPinState);
  }
}

// Given two LED pins, alternate them at a set interval
void AlternateBlink(int interval, byte ledOne, int& ledOneState, byte ledTwo, int& ledTwoState, long& lastBlinkTime)
{
  unsigned long now = millis();

  if (now - lastBlinkTime >= interval)
  {
    // Save the last time we blinked the LED
    lastBlinkTime = now;

    // Swap to opposite states
    ledOneState = (ledOneState == LOW) ? HIGH : LOW;
    ledTwoState = (ledTwoState == LOW) ? HIGH : LOW;

    // Set the LED
    digitalWrite(ledOne, ledOneState);
    digitalWrite(ledTwo, ledTwoState);
  }
}
