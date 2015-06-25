using UnityEngine;
using System.Collections;

public class Direction{
	public const int up = 0;
	public const int left = 1;
	public const int down = 2;
	public const int right = 3;

	public static Vector3 ToVector(int dir){
		if (dir == right) {
			return Vector3.right;
		} else if (dir == up) {
			return Vector3.up;
		} else if (dir == left) {
			return Vector3.left;
		} else if (dir == down) {
			return Vector3.down;
		} else {
			return Vector3.zero;
		}
	}

	public static int ValueX(int dir){
		if (dir == right) {
			return 1;
		} else if (dir == left) {
			return -1;
		} else {
			return 0;
		}
	}

	public static int ValueY(int dir){
		if (dir == up) {
			return 1;
		} else if (dir == down) {
			return -1;
		} else {
			return 0;
		}
	}

	public static float Rotation(int dir){
		return 90 * dir;
	}

	public static int RandomDirection(){
		return Random.Range(0,4);
	}
}
