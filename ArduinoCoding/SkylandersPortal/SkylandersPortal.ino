/*
  Skylanders Portal

  Used to control a NeoPixel Jewel, to color-fade similar to the Skylanders 
  portal base.

  Based on rainbow color functions found on the internet.
  
  The circuit:
  * Designed for an Adafruit Trinket (5v), but can be used on any device
    with the requisite pins.
  * Uses a 3.7v LiPo battery.
  * Assumes use of a NeoPixel Jewel, but will work for other NeoPixel
    form factors, jut adjust the number of lights accordingly.
    NOTE:  The jewel is a GRB device

  Created Jan 12, 2017
  By Will Knoll
*/

#include <Adafruit_NeoPixel.h>
 #ifdef __AVR_ATtiny85__ // Trinket, Gemma, etc.
 #include <avr/power.h>
#endif

const byte DATA_PIN = 0;  // Set 0 as the data output pin, for driving the NeoPixel
const int PIXELS = 7;     // Number of pixels on ring/strip
 
// Parameter 1 = number of pixels in strip
// Parameter 2 = pin number for data
// Parameter 3 = pixel type flags, add together as needed:
//   NEO_KHZ800  800 KHz bitstream (most NeoPixel products w/WS2812 LEDs)
//   NEO_KHZ400  400 KHz (classic 'v1' (not v2) FLORA pixels, WS2811 drivers)
//   NEO_GRB     Pixels are wired for GRB bitstream (most NeoPixel products)
//   NEO_RGB     Pixels are wired for RGB bitstream (v1 FLORA pixels, not v2)
Adafruit_NeoPixel strip = Adafruit_NeoPixel(PIXELS, DATA_PIN, NEO_GRB + NEO_KHZ800);

void setup() {
  #ifdef __AVR_ATtiny85__ // Trinket, Gemma, etc.
    if(F_CPU == 16000000) clock_prescale_set(clock_div_1);
  #endif
  
  strip.begin();  // Initialize the NeoPixel device
  strip.show(); // Begin with all pixels to 'off'

  // Adjust brightness, if desired
  //strip.setBrightness(30);

  // Run through some colors, for fun
  InitSequence();
}
 
void loop() {
  static int RainbowDelay = 400;
  
  RainbowDisplay(RainbowDelay);
}

// Just for show, run a quick initialization sequence
void InitSequence() {
  static int delayBetweenColors = 500;
  static int lightUpDelay = 200;

  // Red
  ColorWipe(strip.Color(255, 0, 0), lightUpDelay);
  delay(delayBetweenColors);
  
  // Green
  ColorWipe(strip.Color(0, 255, 0), lightUpDelay);
  delay(delayBetweenColors);
  
  // Blue
  ColorWipe(strip.Color(0, 0, 255), lightUpDelay);
  delay(delayBetweenColors);

  // Twinkle all the pixels for a bit
  for (int i = 0; i < 50; i++) {
    Twinkle();
  }
}

// Fill the dots one after the other with a color
void ColorWipe(uint32_t color, uint8_t wait) {
  for(uint16_t i=0; i < strip.numPixels(); i++) {
      strip.setPixelColor(i, color);
      strip.show();
      delay(wait);
  }
}

// Twinkle random colors on the pixels
void Twinkle() {
  // Pick a random pixel
  int ranPixel = random(0, PIXELS);

  // Set to a random color
  strip.setPixelColor(ranPixel, strip.Color(random(0,255),random(0,255),random(0,255))); 

  // Turn it on
  strip.show();

  delay(100);

  // Turn it back off
  strip.setPixelColor(ranPixel, strip.Color(0,0,0));
  strip.show();
}

// Fade between colors, for a rainbow effect
void RainbowDisplay(uint8_t wait) {
  uint16_t i, j;
 
  for(j=0; j < 256; j++) {
    for(i=0; i < strip.numPixels(); i++) {
      strip.setPixelColor(i, Wheel((i+j) & 255));
    }
    strip.show();
    delay(wait);
  }
}
 
// Input a value 0 to 255 to get a color value.
// The colours are a transition r - g - b - back to r.
uint32_t Wheel(byte WheelPos) {
  if(WheelPos < 85) {
   return strip.Color(WheelPos * 3, 255 - WheelPos * 3, 0);
  } else if(WheelPos < 170) {
   WheelPos -= 85;
   return strip.Color(255 - WheelPos * 3, 0, WheelPos * 3);
  } else {
   WheelPos -= 170;
   return strip.Color(0, WheelPos * 3, 255 - WheelPos * 3);
  }
}
