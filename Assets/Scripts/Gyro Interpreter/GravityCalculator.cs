using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using MSP_Input;

public class GravityCalculator : MonoBehaviour 
{
	//public GyroAccel gyroAccel;

	public Text x;
	public Text y;
	public Text z;
	public Text w;

	public Text x1;
	public Text y1;
	public Text z1;

	public Text x2;
	public Text y2;
	public Text z2;

	public Rigidbody2D rigidBody;
	public float speed = 9.86f;

	private void FixedUpdte () 
	{
		//if (pitch != null) pitch.text = gyroAccel.pitch.ToString ();
		//if (roll != null) roll.text = gyroAccel.roll.ToString ();
		//if (heading != null) heading.text = gyroAccel.heading.ToString ();
		//transform.Translate(Input.acceleration.x, 0, -Input.acceleration.z);

		x.text = Input.gyro.attitude.x.ToString ();
		y.text = Input.gyro.attitude.y.ToString ();
		z.text = Input.gyro.attitude.z.ToString ();
		w.text = Input.gyro.attitude.w.ToString ();

		x1.text = Input.acceleration.x.ToString ();
		y1.text = Input.acceleration.y.ToString ();
		z1.text = Input.acceleration.z.ToString ();

		x2.text = Input.gyro.gravity.x.ToString ();
		y2.text = Input.gyro.gravity.y.ToString ();
		z2.text = Input.gyro.gravity.z.ToString ();
	}
}
