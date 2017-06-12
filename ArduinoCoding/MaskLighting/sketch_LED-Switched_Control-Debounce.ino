#include <LEDFader.h> 

/*
 * Controls and switches LEDs, for different mask lighting effects.
 * Implements debounce functionality.
 * Uses the LEDFader library.
 */
 
  // Set debug flag
  const bool DEBUG = false;
  
  // Define Pins
  #define LED_PIN 13     // Set pin 9 as the LED output
  #define BUTTON_PIN 2  // Set Pin 2 as the button input
  
  // Define fader options
  #define FADE_TIME 4000  // Fade for 4 seconds, each direction
  #define DIRECTION_UP 1
  #define DIRECTION_DOWN -1

  const int FADE_BRIGHTNESS_MIN = 2;   // Minimum value to fade down to ... setting to low makes it look somewhat "blinky"
  const int FADE_BRIGHTNESS_MAX = 180; // Maximum value to fade up to ... setting to high makes it spend too much time at a bright level
  int direction = DIRECTION_DOWN;      // Start off fading down, because LEDs will already be at full power, since fade comes after constant
  
  LEDFader _ledFader; // Define an LED fader object
  
  // Define blink pin timer(s) to avoid using delay()
  long _lastBlinkTime = 0; // Store last time blink LEDs were updated

  // Set initial LED pin state
  int _ledPinState = LOW;
  
  // Blink intervals (milliseconds)
  const int SLOW_INTERVAL = 1000;
  const int MED_INTERVAL = 500;
  const int FAST_INTERVAL = 250;

  // Random blink time options (milliseconds).  ON and OFF are configured separately
  // to give added flexibility to the look.  By making the maximum OFF time shorter
  // than the maximum ON time, the effect looks better.
  //
  // MIN = minimum time to spend in the ON, or OFF state
  // MAX = maximum time to spend in the ON, or OFF state
  //
  // Note, anything less than 30ms is typically not perceptable, so setting a minimum
  // lower than that actually reduces the perceived randomness of the effect. i.e. those cycles won't be seen
  const int RANDOM_OFF_MIN = 30;
  const int RANDOM_OFF_MAX = 250;
  const int RANDOM_ON_MIN = 30;
  const int RANDOM_ON_MAX = 500;

  // Initialize on and off for random blinking
  int _randomDelayOff = random(1, 250);
  int _randomDelayOn = random(1, 500);

  // Initialize Button stuff
  int _buttonPushCounter = 0;   // Counter for the number of button presses
  int _buttonState = 0;         // Current state of the button
  int _lastButtonState = 0;     // Previous state of the button

  // Initialize Debouncer for button
  // Code based off of SoftwareDebounce, by Jon Schlueter
  // http://playground.arduino.cc/Learning/SoftwareDebounce
  int _debounceCount = 50;        // Number of millis/samples to check
  int _debounceCounter = 0;       // Number of times new value seen
  int _debounceReading = 0;       // Current value read from the Button pin
  int _currentButtonState = LOW;  // Debounced input value
  long _lastButtonSampleTime = 0; // Last time the button pin was sampled
  
void setup()
{
  // Start serial communication
  Serial.begin(9600);
  
  // Initialize random number generator with unique seed
  // Really not necessary, but ensures the pattern is always different, from run to run
  randomSeed(analogRead(0));
  
  // Setup Fader
  _ledFader = LEDFader(LED_PIN); // Create new LED Fader on LED pin

  // Set pin modes
  pinMode(LED_PIN, OUTPUT);
  pinMode(BUTTON_PIN, INPUT);
}

void loop()
{
  // Check the button first
  //ReadButtonStateWithDebounce();
ReadNewDebounce();
  // Adjust LED mode, as needed
  SetLedMode();
}

void SetLedMode()
{
  // Determine current LED mode, based on button counter
  int ledMode = (_buttonPushCounter % 6);  // Six being the number of defined lighting options

  switch(ledMode)
  {
    case 0:
      // Constant mode
      digitalWrite(LED_PIN, HIGH);
      break;
    case 1:
      // Fader mode
      RunFadePin();
      break;
    case 2:
      // Blink mode, slow interval
      Blink(SLOW_INTERVAL);
      break;
    case 3:
      // Blink mode, medium interval
      Blink(MED_INTERVAL);
      break;
    case 4:
      // Blink mode, fast interval
      Blink(FAST_INTERVAL);
      break;
    case 5:
      // Blink randomly
      BlinkRandom();
      break;
  }
}

bool countingUp = true;


int debounce_counter;
int debounced_output = 0;
void ReadNewDebounce()
{
  //input raw_input - read button
  //output debounced_output - filtered data
  // call every ms
  
  
  // Ensure we've moved to the next millisecond
  if (millis() != _lastButtonSampleTime)
  {
  if (digitalRead(BUTTON_PIN)) 
  {
    debounce_counter++;
  }
  else
  {
    debounce_counter--;
  }
  
  if (debounce_counter > 10)
  {
    debounce_counter = 10;
    debounced_output = 1;
    // Button push detected, increment our button press counter
      _buttonPushCounter++;
  }
  
  if (debounce_counter < 0)
  {
    debounce_counter = 0;
    debounced_output = 0;
  }
    _lastButtonSampleTime = millis(); 
  }
}

void ReadButtonStateWithDebounce()
{
  // Ensure we've moved to the next millisecond
  if (millis() != _lastButtonSampleTime)
  {
    // Check the pin for a signal
    _debounceReading = digitalRead(BUTTON_PIN);

    // Based on the signal reading, adjust the counter up or down, 
    // to ensure we've remained in the same state long enough
    if (_debounceReading == _currentButtonState && _debounceCounter > 0)
    {
      _debounceCounter--;
    Serial.print("Count:  ");
    Serial.print(_debounceCounter);
    Serial.print(" Reading:  ");
    Serial.print(_debounceReading);
    Serial.print(" State:  ");
      Serial.println(_currentButtonState);
    countingUp = false;
    }
    if(_debounceReading != _currentButtonState)
    {
       _debounceCounter++; 
      Serial.print("Count:  ");
    Serial.print(_debounceCounter);
    Serial.print(" Reading:  ");
    Serial.print(_debounceReading);
    Serial.print(" State:  ");
      Serial.println(_currentButtonState);
    countingUp = true;
    }
    
    // If the Input has shown the same value for long enough let's switch it
    if (_debounceCounter >= _debounceCount)
    {
      _debounceCounter = 0;
      _currentButtonState = !_currentButtonState;//_debounceReading;
    
    if (!countingUp)
    {
    // Skip this button press
    Serial.println("Counting down, skipping button press");
  }
  else
  {
      // Button push detected, increment our button press counter
      _buttonPushCounter++;

        Serial.println("switch");
        Serial.print("number of button pushes:  ");
        Serial.println(_buttonPushCounter);
    }
    }

    _lastButtonSampleTime = millis();
  }
  
 
}

void RunFadePin()
{
  _ledFader.update();

  // LED no longer fading, switch direction
  if (!_ledFader.is_fading())
  {
    // Fade down
    if (direction == DIRECTION_UP)
    {
      _ledFader.fade(FADE_BRIGHTNESS_MIN, FADE_TIME);
      direction = DIRECTION_DOWN;
    }
    // Fade up
    else
    {
      _ledFader.fade(FADE_BRIGHTNESS_MAX, FADE_TIME);
      direction = DIRECTION_UP;
    }
  }
}

void Blink(int interval)
{
  unsigned long now = millis();

  if (now - _lastBlinkTime >= interval)
  {
    // Save the last time we blinked the LED
    _lastBlinkTime = now;

    // Swap to opposite state
    _ledPinState = (_ledPinState == LOW) ? HIGH : LOW;

    // Set the LED
    digitalWrite(LED_PIN, _ledPinState);
  }
}

void BlinkRandom()
{
  unsigned long now = millis();

  if (_ledPinState == LOW)
  {
    if (now - _lastBlinkTime >= _randomDelayOff)
    {
      // OFF time is up, time to turn the LED back on
      // Set our ON duration
      _randomDelayOn = random(RANDOM_ON_MIN, RANDOM_ON_MAX);
      
      // Turn the LED back on
      digitalWrite(LED_PIN, HIGH);

      // Reset the pin state
      _ledPinState = HIGH;

      // Update previous ms counter
      _lastBlinkTime = now;
    }

    // Time isn't up yet
    return;
  }

  if (_ledPinState == HIGH)
  {
    if (now - _lastBlinkTime >= _randomDelayOn)
    {
      // ON time is up, time to turn the LED back off
      // Set our OFF duration
      _randomDelayOff = random(RANDOM_OFF_MIN, RANDOM_OFF_MAX);

      // Turn the LED back off
      digitalWrite(LED_PIN, LOW);
      
      // Reset the pin state
      _ledPinState = LOW;

      // Update previous ms counter
      _lastBlinkTime = now;
    }
  }
}


