/**
 * Load and Display an OBJ Shape. 
 * 
 * The loadShape() command is used to read simple SVG (Scalable Vector Graphics)
 * files and OBJ (Object) files into a Processing sketch. This example loads an
 * OBJ file of a rocket and displays it to the screen. 
 */


PShape ship;

float ry;
float rx;
float rz;
int j;
 
public void setup() {
  size(1024, 768, P3D);
    
  ship = loadShape("Ship_v3.obj");
}

public void draw() {
  background(0);
  lights();
  //translate(58, 48, 0);
  
  rotateY(map(mouseX, 0, width, 0, PI));
  rotateZ(map(mouseY, 0, height, 0, -PI));
  stroke(255);
  
  
  translate(width/2, height/2 + 3600, 0/*-200*10*/);
  //rotateZ(PI);
  //translate(0, 0, 1200);
  
  rotateY(rx);
  rotateX(rz);
  
  if (keyPressed) {
    if (key=='e'){
      pushMatrix();
      
      
      rotateX(PI/2);
      j=(int) (ry*300)/600;
      translate(0,(ry*300)%600,0);
      
      for (int i=0; i<6;i++){
        if(5-i==j%6)
          stroke(255,0,0);
        cylinder(200,600,12);
        if(5-i==j%6)
          stroke(255);
        translate(0, -600, 0/*-200*10*/);  
        
      }
    
      popMatrix();
      
      //translate(0, 3600, 0/*-200*10*/);
    }
  }
  rotateZ(ry);
  
  strokeWeight(2);
  stroke(0,255,0);
  line(0, 0, -10000, 0, 0, 10000);
  
  stroke(255,0,0);
  sphere(40);
  
  rotateX(PI);
  translate(0, -1800, 0);
  shape(ship);
  
  ry += 0.02;
}

void cylinder(float w, float h, int sides)
{
  float angle;
  float[] x = new float[sides+1];
  float[] z = new float[sides+1];
 
  //get the x and z position on a circle for all the sides
  for(int i=0; i < x.length; i++){
    angle = TWO_PI / (sides) * i;
    x[i] = sin(angle) * w;
    z[i] = cos(angle) * w;
  }
 
  //draw the top of the cylinder
  beginShape(TRIANGLE_FAN);
 
  vertex(0,   -h/2,    0);
 
  for(int i=0; i < x.length; i++){
    vertex(x[i], -h/2, z[i]);
  }
 
  endShape();
 
  //draw the center of the cylinder
  beginShape(QUAD_STRIP); 
 
  for(int i=0; i < x.length; i++){
    vertex(x[i], -h/2, z[i]);
    vertex(x[i], h/2, z[i]);
  }
 
  endShape();
 
  //draw the bottom of the cylinder
  beginShape(TRIANGLE_FAN); 
 
  vertex(0,   h/2,    0);
 
  for(int i=0; i < x.length; i++){
    vertex(x[i], h/2, z[i]);
  }
 
  endShape();
}

void keyPressed() {
  if (key == CODED) {
    if (keyCode == LEFT) {
      rx += 0.02;
    } else if (keyCode == RIGHT) {
      rx -= 0.02;
    } else if (keyCode == UP) {
      rz += 0.02;
    } else if (keyCode == DOWN) {
      rz -= 0.02;
    }
  }
}
