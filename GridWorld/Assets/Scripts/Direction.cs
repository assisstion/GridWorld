using UnityEngine;
using System.Collections;

public class Direction{
	public const int right = 0;
	public const int up = 1;
	public const int left = 2;
	public const int down = 3;

	public static Vector3 ToVector(int dir){
		if (dir == 0) {
			return Vector3.right;
		} else if (dir == 1) {
			return Vector3.up;
		} else if (dir == 2) {
			return Vector3.left;
		} else if (dir == 3) {
			return Vector3.down;
		} else {
			return Vector3.zero;
		}
	}

	public static int ValueX(int dir){
		if (dir == 0) {
			return 1;
		} else if (dir == 2) {
			return -1;
		} else {
			return 0;
		}
	}

	public static int ValueY(int dir){
		if (dir == 1) {
			return 1;
		} else if (dir == 3) {
			return -1;
		} else {
			return 0;
		}
	}

	public static float Rotation(int dir){
		return 90 * dir;
	}
}
