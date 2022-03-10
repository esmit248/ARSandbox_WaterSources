/*Shader to add the water via spring, continual wave motion or tidal wave*/
 
#extension GL_ARB_texture_rectangle : enable

uniform int waterSourceType;
uniform float waveAmplitude;
uniform float waveFrequency;
uniform int tidalWave;
uniform sampler2DRect quantitySampler;
uniform sampler2DRect bathymetrySampler;


void main()
{
	// Get current bathymetry height and quantity value
	float B=(texture2DRect(bathymetrySampler,vec2(gl_FragCoord.x-1.0,gl_FragCoord.y-1.0)).r+
	         texture2DRect(bathymetrySampler,vec2(gl_FragCoord.x,gl_FragCoord.y-1.0)).r+
	         texture2DRect(bathymetrySampler,vec2(gl_FragCoord.x-1.0,gl_FragCoord.y)).r+
	         texture2DRect(bathymetrySampler,vec2(gl_FragCoord.xy)).r)*0.25;

	
	vec3 Q = texture2DRect(quantitySampler,gl_FragCoord.xy).rgb;
	
	if(waterSourceType==1){

		float distX=(gl_FragCoord.x -32.5);
		float distY=(gl_FragCoord.y -412.5);
		
		//if(gl_FragCoord.x >25.0 && gl_FragCoord.x < 40.0
		  //  && gl_FragCoord.y >405.0 && gl_FragCoord.y <420.0){
	   
	   if(sqrt(distX*distX+distY*distY) < 20){
	   
		
			float h=Q.x-B;
			if(B<0.01){ // negative values and zero value are "full strength" source
				Q.x=Q.x+0.001;  
			}
			else
			{
				Q.x=Q.x+((0.001*(10-B))/10.0);	
			}

		
		}
	}
	else if(waterSourceType==2)
	{
		if(gl_FragCoord.x >625.0 )
	   {
			Q.x+=0.0004*waveAmplitude;
			if(waveAmplitude>0)
			{
				Q.y=-0.2;
			}
			else
			{
				Q.y=+0.2;
			}
			
		}
	}
 	else if( tidalWave==1)
	{
		if(gl_FragCoord.x >625.0 )
	    {
			Q.x+=1.5;
			Q.y=-2.0;
			
		}
	}

	gl_FragColor=vec4(Q,0.0);
}