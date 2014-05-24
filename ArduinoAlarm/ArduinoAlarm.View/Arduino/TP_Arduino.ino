#include <EEPROM.h>



int lastFrame = 0;

int currentHours = 0;
int currentMinutes = 0;
int currentSeconds = 0;
int currentMilliseconds = 0;

int startingAlarmHours = 0;
int startingAlarmMinutes = 0;
int startingAlarmSeconds = 0;
int startingAlarmMilliseconds = 0;

int endingAlarmHours = 0;
int endingAlarmMinutes = 0;
int endingAlarmSeconds = 0;
int endingAlarmMilliseconds = 0;

boolean isActive = false;
int secondsBeforeAlarm = 10;

int timeBeforeNextSos = 0;

int lastTimeActivation = 0;
int timeBeforeNextNotification = 0;

/* READ commands */
// 1 => get date
// 2 => switch ON / OFF alarm
// 3 => plan alarm (between start date and end date)
// 4 => update password
// 5 => are you alive ?
// 6 => update waiting time for the password
// 7 => switch ON / OFF LEDs


/* WRITE commands */
// 1 => ask for the date
// 2 => I am alive (return if we get value)
// 3 => intrusion notification
// 4 => SOS







void setup()
{
 Serial.begin(9600); 
 
 pinMode(2, INPUT);
 pinMode(3, OUTPUT);
 
 pinMode(5, OUTPUT);
 pinMode(6, OUTPUT);
 pinMode(7, OUTPUT);
 
 pinMode(A8, OUTPUT);
 
 digitalWrite(3, HIGH);
 
 writeS(0, "password");

 askDate();
}


void loop()
{
  if (Serial.available())
  {
    byte command = Serial.read();
    
    switch (command)
    {
      case 1:
        getDate();
        break;
      case 2:
        switchBuzzer();
        break;
      case 3:
        planAlarm();
        break;
      case 4:
        setPassword();
        break;
      case 5:
        areYouAlive();
        break;
      case 6:
        setWaitingTime();
        break;
      case 7:
        switchLED();
        break;
    }
  }

  manageDate();
  manageAlarm();
  
  intrusionNotification();
  sos();
}











// WRITE PART

/// Command 1
/// Ask a date to the software
///
void askDate()
{
  // ask for the date
  Serial.write(1);
}

/// Command 2
/// Say "I am alive"
///
void iAmALive()
{
  Serial.write("I am alive\n");
}

/// Command 3
/// Intrusion notification
///
void intrusionNotification()
{
  // check intrusion with movement detector
  
  timeBeforeNextNotification -= millis() - lastFrame;
  
  if (timeBeforeNextNotification <= 0)
  {
    Serial.write("INT\n");
    lastTimeActivation = lastFrame;
    timeBeforeNextNotification = 15000;
  }
}

/// Command 4
/// SOS
///
void sos()
{
  // check SOS with pseudo button
  
  timeBeforeNextSos -= millis() - lastFrame;
  
  if (timeBeforeNextSos <= 0)
  {
    if (digitalRead(2))
    {
      Serial.write("SOS\n");
      timeBeforeNextSos = 30000;
    }
  }
}










// READ PART

/// Command 1
/// Get the date data from the software
///
void getDate()
{
  // get the data from Serial
  currentHours = getStringData().toInt();
  currentMinutes = getStringData().toInt();
  currentSeconds = getStringData().toInt();
  currentMilliseconds = getStringData().toInt();
  
  // refresh the last frame (due to waiting data)
  lastFrame = millis();
}

/// Command 2
/// Switch ON or OFF buzzer
///
void switchBuzzer()
{
  byte value = getByteData();

  // if we want to deactive alarm, we need a password
  /*if (!value)
  {
    String testPassword = getStringData();
    if (testPassword != readS(0))
      return;
  }*/
  
  isActive = value;
}

/// Command 3
/// Plan alarm to tone when the user wants
///
void planAlarm()
{
  // get time for starting the alarm
  startingAlarmHours = getStringData().toInt();
  startingAlarmMinutes = getStringData().toInt();
  startingAlarmSeconds = getStringData().toInt();
  startingAlarmMilliseconds = getStringData().toInt();
  
  // get time for ending the alarm
  endingAlarmHours = getStringData().toInt();
  endingAlarmMinutes = getStringData().toInt();
  endingAlarmSeconds = getStringData().toInt();
  endingAlarmMilliseconds = getStringData().toInt();
}

/// Command 4
/// Set the password
///
void setPassword()
{
  String newPassword = getStringData();
  
  writeS(0, newPassword);
}

/// Command 5
/// Are you alive ?
///
void areYouAlive()
{
  iAmALive();
}

/// Command 6
/// Set waiting time
///
void setWaitingTime()
{
  secondsBeforeAlarm = getStringData().toInt();
  lastTimeActivation = 0;
}

/// Command 7
/// Switch ON or OFF LEDs
///
void switchLED()
{
  byte value = getByteData();
  
  for (byte pin = 5; pin < 8; pin++)
    digitalWrite(pin, value); 
}













// UTILS PART

///
/// Write a String in the ROM
///
void writeS(int startIndex, String text)
{
 byte sLength = text.length();
 
 EEPROM.write(startIndex, sLength);
 
 for (byte i = 0; i < sLength; i++)
   EEPROM.write(i + 1, text.charAt(i));
}

///
/// Read a String in the ROM
///
String readS(int startIndex)
{
 byte sLength = EEPROM.read(startIndex);
 String getS;
 
 for (byte i = 0; i < sLength; i++)
   getS += (char)EEPROM.read(startIndex + i + 1);
 
 return getS;
}

///
/// Get the current byte in the serial
///
byte getByteData()
{
  while (!Serial.available());
  return Serial.read();
}

///
/// Get the current string (line) in the serial
///
String getStringData()
{
  String s;
  char c;
  
  while (true)
  {
    c = Serial.read();
    
    if (c == '\n')
      return s;
    
    if (c > 0)
      s += c;
  }
  
  return s;
}

///
/// Manage date - refresh date every loop (milliseconds, seconds, ...)
///
void manageDate()
{
   currentMilliseconds += millis() - lastFrame;
   if (currentMilliseconds >= 1000)
   {
     currentMilliseconds -= 1000;
     currentSeconds++;
   }
   if (currentSeconds >= 60)
   {
     currentSeconds -= 60;
     currentMinutes++;
   }
   if (currentMinutes >= 60)
   {
     currentMinutes -= 60;
     currentHours++;
   }
   
   lastFrame = millis();
}

///
/// Manage alarm (tone when user needs)
///
void manageAlarm()
{
  //lastTimeActivation -= millis();
  
  if (!isActive)
  {
    noTone(A8);
    return;
  }
  
  if ((lastTimeActivation + (secondsBeforeAlarm * 1000)) == lastFrame)
    tone(A8, 500, 5000);
  
  if (currentHours == startingAlarmHours && currentMinutes == startingAlarmMinutes && currentSeconds == startingAlarmSeconds)
    tone(A8, 200);
  
  if (currentHours == endingAlarmHours && currentMinutes == endingAlarmMinutes && currentSeconds == endingAlarmSeconds)
    noTone(A8);
}
