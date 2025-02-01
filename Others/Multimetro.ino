bool measure=false;

String entry;
char stopp;
void setup() {
  Serial.begin(115200);
}

void loop() {
  // Serial.println("hola Mundo");
  // put your main code here, to run repeatedly:
  if (Serial.available())
  {
    entry=Serial.readStringUntil('\n');
    Serial.println(entry);
    if(entry=="Osciloscope")
    {
      measure=true;
      while(measure)
      {
        int analogV=analogRead(A0);
        Serial.println();
        Serial.print(analogV);
        Serial.print(",");
        Serial.print(micros());
        Serial.print(".");
      }
    }

  }
}




